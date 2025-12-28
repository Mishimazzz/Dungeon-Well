using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvases : MonoBehaviour
{
    private static PersistentCanvases instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            // 场景里如果又生成了一份 UIRoot，就把新的删掉
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);   // 整个 UIRoot（包含所有 Canvas）一起保留
    }
}


