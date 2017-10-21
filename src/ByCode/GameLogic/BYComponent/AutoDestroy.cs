using GameLogic;
using UnityEngine;


public class AutoDestroy : MonoBehaviour
{
    public float delay = 0;
    public bool destroyAfterLaunchered;

    void Awake()
    {
        if (destroyAfterLaunchered)
        {
            if (Launcher.Instance != null)
                Destroy(gameObject, 0);
        }
        else Destroy(gameObject, delay);
    }
}

