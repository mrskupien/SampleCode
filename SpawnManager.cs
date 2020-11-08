using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager2 : MonoBehaviour
{
    private static SpawnManager2 _instance;
    public static SpawnManager2 Instance
    {
        get
        {
            if(_instance== null)
            {
                Debug.Log("SpawnManager does not exist");
            }
            return _instance;
        }
    }

    public GameObject objectHolder;

    private List<GameObject> _spawnPositions = new List<GameObject>();
    private List<GameObject> _shuffledSpawnPositions = new List<GameObject>();
    private int _index;
    private int _seed = 0;

    private int _spawnPointsNumber = 12;
    private float _spawnRadius = 5;
    private float _spawnHeight = 13;

    private void Awake()
    {
        _instance = this;
        SpawnPositions();
    }

    private void Start()
    {
        System.Func<GameObject> onRequestBait = () => PoolManager.Instance.RequestBait();
        System.Func<GameObject> onRequestHook = () => PoolManager.Instance.RequestHook();

        StartCoroutine(StartSpawningObject(onRequestBait, 1));
        StartCoroutine(StartSpawningObject(onRequestHook, 2));
    }

    private void SpawnPositions()
    {
        // x=R*cos(t), y=R*sin(t), t in rad
        float angleBetweenSpawnPoints = 2 * Mathf.PI / _spawnPointsNumber;
        float angleOfSpawnPoint = -Mathf.PI / 2;

        for (int i = 0; i <= _spawnPointsNumber; i++)
        {
            angleOfSpawnPoint += angleBetweenSpawnPoints;
            float x = _spawnRadius * Mathf.Cos(angleOfSpawnPoint);
            float y = _spawnRadius * Mathf.Sin(angleOfSpawnPoint);

            GameObject newSpawnPosition = new GameObject("SpawnPosition " + i);
            newSpawnPosition.transform.position = new Vector3(x, this.transform.position.y, y);
            newSpawnPosition.transform.rotation = Quaternion.Euler(-90, (90 - Mathf.Rad2Deg * angleOfSpawnPoint), -90);
            newSpawnPosition.transform.parent = this.transform;

            _spawnPositions.Add(newSpawnPosition);
        }

        _shuffledSpawnPositions = UtilityHelper.ShuffleList(_spawnPositions, _seed);
    }

    private IEnumerator StartSpawningObject(System.Func<GameObject> requestObject, float delay)
    {
        while (!GameManager.Instance.IsGameOver)
        {
            yield return new WaitForSeconds(delay);

            //get gameobject as spawner parameters
            GameObject spawner = GetRandomSpawner();
            Vector3 spawnerPos = new Vector3(spawner.transform.position.x, spawner.transform.position.y + _spawnHeight, spawner.transform.position.z);
            Quaternion spawnerRot = spawner.transform.rotation;

            //request object from pool manager
            GameObject newObject = requestObject();

            newObject.transform.position = spawnerPos;
            newObject.transform.rotation = spawnerRot;
            newObject.transform.parent = objectHolder.transform;
        }
    }

    private GameObject GetRandomSpawner()
    {
        _index++;
        _index %= _shuffledSpawnPositions.Count;
        return _shuffledSpawnPositions[_index];
    }

}
