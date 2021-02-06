using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public enum GameState { PREGAME, RUNNING, PAUSED }

    public GameObject[] SystemPrefabs;
    public Events.EventGameState OnGameStateChanged;

    private List<GameObject> _instantiatedSystemPrefabs = new List<GameObject>();
    private List<AsyncOperation> _loadOperations = new List<AsyncOperation>();
    private string _currentLevelName;
    private GameState _currentGameState = GameState.PREGAME;
    public GameState CurrentGameState
    {
        get {
            return _currentGameState;
        }
        private set {
            _currentGameState = value;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        InstantiateSystemPrefabs();

        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
    }

    private void Update()
    {
        if(_currentGameState == GameState.PREGAME)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
            TooglePause();
    }


    private void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;
        switch(_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1f;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0f;
                break;
            default:
                break;
        }
        Debug.Log("Current state: " + _currentGameState);
        OnGameStateChanged?.Invoke(_currentGameState, previousGameState);
    }

    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        foreach(var prefab in SystemPrefabs)
        {
            prefabInstance = Instantiate(prefab);
            _instantiatedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if(ao == null)
        {
            Debug.LogError("[GameManager] is unable to load level " + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
    }
    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        if(_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if(_loadOperations.Count == 0)
                UpdateState(GameState.RUNNING);
        }
        Debug.Log("Load complete");
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if(ao == null)
        {
            Debug.LogError("[GameManager] is unable to unload level " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    private void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload complete");
    }

    private void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if(!fadeOut)
            UnloadLevel(_currentLevelName);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach(var go in _instantiatedSystemPrefabs)
        {
            Destroy(go);
        }
        _instantiatedSystemPrefabs.Clear();
    }

    public void StartGame()
    {
        LoadLevel("Main");
    }

    public void TooglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
