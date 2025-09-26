using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    public GameObject canvas;

    private void Awake()
    {
        DontDestroyOnLoad(canvas);
    }
}

