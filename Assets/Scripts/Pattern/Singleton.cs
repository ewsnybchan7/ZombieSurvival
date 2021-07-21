using UnityEngine;


public class Singleton<Class> : MonoBehaviour where Class : MonoBehaviour
{
    [SerializeField]
    protected bool _persistent = true;

    protected delegate void AwakeOp();
    protected event AwakeOp AwakeOperation;

    public static Class Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<Class>();

            return m_Instance;
        }
    }

    static Class m_Instance;

    protected virtual void Awake()
    {
        if (_persistent)
            DontDestroyOnLoad(gameObject);

        AwakeOperation?.Invoke();
    }
}