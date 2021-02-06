using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance { 
        get {
            if(_instance == null)
                Debug.LogError(typeof(T) + " is null");
            return _instance; 
        } 
    }

    public static bool IsInitialized { get { return _instance != null; } }

    protected virtual void Awake()
    {
        if(_instance != null)
            Debug.LogError((T)this + " is trying to instantiate a second instance of a singleton class");
        else
        {
            _instance = (T)this;
            Init();
        }
    }

    protected virtual void Init()
    {
        //optional to override without need to OnAwake
    }

    protected virtual void OnDestroy()
    {
        if(_instance == this)
            _instance = null;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }
}
