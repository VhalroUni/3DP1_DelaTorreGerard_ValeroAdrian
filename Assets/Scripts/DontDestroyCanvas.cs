using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{
    static public DontDestroyCanvas instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
          
            Destroy(gameObject);
        }
    }

    
    void Update()
    {
        
    }
}
