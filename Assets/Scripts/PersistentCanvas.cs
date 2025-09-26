using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvases : MonoBehaviour
{
    private static GameObject uiCanvasInstance;
    private static GameObject timeCanvasInstance;

    [Header("Assign the two Canvases you want to persist")]
    public GameObject UICanvas;
    public GameObject TimeCanvas;

    private void Awake()
    {
        // UICanvas 保留
        if (UICanvas != null)
        {
            if (uiCanvasInstance == null)
            {
                uiCanvasInstance = UICanvas;
                DontDestroyOnLoad(uiCanvasInstance);
            }
            else if (UICanvas != uiCanvasInstance)
            {
                Destroy(UICanvas);
            }
        }

        // TimeCanvas 保留
        if (TimeCanvas != null)
        {
            if (timeCanvasInstance == null)
            {
                timeCanvasInstance = TimeCanvas;
                DontDestroyOnLoad(timeCanvasInstance);
            }
            else if (TimeCanvas != timeCanvasInstance)
            {
                Destroy(TimeCanvas);
            }
        }
    }
}


