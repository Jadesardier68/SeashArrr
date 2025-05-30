using UnityEngine;

/// <summary> Singleton class used to access object instance </summary>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    /// <summary> Gets the Singleton instance. </summary>
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType(typeof(T)) as T;
                if (_instance == null)
                { return null; }
            }
            return _instance;
        }
    }

    /// <summary> Awake is called when the script instance is being loaded </summary>
    private void Awake()
    {
        if (_instance == null)
        { _instance = this as T; }
        else if (_instance != this as T)
        {
            Destroy(gameObject);
            return;
        }
        _instance.AwakeSingleton();
    }

    /// <summary> Awakes the singleton. Replaces Unity Awake method </summary>
    public virtual void AwakeSingleton() { }
}