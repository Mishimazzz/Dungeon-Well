using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance;   // 单例
    public List<RectTransform> farmGridAreas = new List<RectTransform>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 每次换场景重新收集
        farmGridAreas.Clear();

        GameObject[] cells = GameObject.FindGameObjectsWithTag("FarmGrid");
        foreach (var cell in cells)
        {
            RectTransform rt = cell.GetComponent<RectTransform>();
            if (rt != null) farmGridAreas.Add(rt);
        }

        Debug.Log($"[{scene.name}] 收集到 {farmGridAreas.Count} 个 FarmGrid");
    }

    void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
