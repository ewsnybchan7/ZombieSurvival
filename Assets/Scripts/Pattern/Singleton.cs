using UnityEngine;


public class Singleton<Class> : MonoBehaviour where Class : MonoBehaviour
{
    [SerializeField]
    protected bool _persistent = true;

    public static Class Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<Class>();

            return m_Instance;
        }
    }

    protected virtual void Start()
    {
        if (_persistent)
            DontDestroyOnLoad(gameObject);
    }

    static Class m_Instance;
}