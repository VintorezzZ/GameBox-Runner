using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{        
    private static T _instance;

    public static bool Exist { get { return _instance != null; } }

    public static T Instance
    {
        get
        {
            if (_instance == null) 
                Debug.LogError(typeof(T).Name + " instance not found.");
                
            return _instance;
        }
    }

    public void InitializeSingleton()
    {
        _instance = this as T;
    }
}

public class Singleton<T> where T : new()
{
    private static T _instance;

    public static bool Exist { get { return _instance != null; } }

    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(typeof(T).Name + " instance not found.");

            return _instance;
        }
    }

    public static void InitializeSingleton()
    {
        _instance = new T();
    }
}
    
    
public class SingletonAutoCreateBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{        
    private static T _instance;

    public static bool Exist { get { return _instance != null; } }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }
                
            return _instance;
        }
    }
}