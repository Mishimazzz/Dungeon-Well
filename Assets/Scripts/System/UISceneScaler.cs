using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class UISceneScaler : MonoBehaviour
{
    [Header("自动收集到的 CanvasScaler")]
    public List<CanvasScaler> scalers = new List<CanvasScaler>();

    [Header("三档尺寸（UI 与 场景一起变）")]
    public Vector2[] uiResolutions = {
        new Vector2(800, 500),     // 尺寸 0（最小）
        new Vector2(1250, 800),    // 尺寸 1（中等）
        new Vector2(1920, 1080)    // 尺寸 2（最大）
    };

    public Vector3[] sceneScales = {
        new Vector3(0.6f, 0.6f, 1f), // 0
        new Vector3(0.8f, 0.8f, 1f), // 1
        new Vector3(1f, 1f, 1f)      // 2
    };

    private int currentIndex = 1; // 初始在中间档（1）

    void Start()
    {
        ApplyScale(currentIndex);
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshScalers();
        ApplyScale(currentIndex);
    }

    void RefreshScalers()
    {
        scalers.Clear();
        scalers.AddRange(FindObjectsOfType<CanvasScaler>(true)); // true = 包括隐藏的
    }

    public void ScaleUp() // 相当于“放大按钮”
    {
        currentIndex = Mathf.Clamp(currentIndex + 1, 0, uiResolutions.Length - 1);
        ApplyScale(currentIndex);
    }

    public void ScaleDown() // 相当于“缩小按钮”
    {
        currentIndex = Mathf.Clamp(currentIndex - 1, 0, uiResolutions.Length - 1);
        ApplyScale(currentIndex);
    }

    private void ApplyScale(int index)
    {
        // UI 三个 canvas 一起缩放
        foreach (var scaler in scalers)
        {
            scaler.referenceResolution = uiResolutions[index];
        }
    }
}
