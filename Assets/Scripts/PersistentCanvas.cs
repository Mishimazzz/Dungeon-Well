using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas instance;
    public GameObject TimeCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(TimeCanvas);
        }
        else
        {
            Destroy(TimeCanvas);
            Destroy(gameObject);
        }
    }
}

