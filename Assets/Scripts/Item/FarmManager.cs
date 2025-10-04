using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance;   // 单例
    public List<SeedSaveData> plantedSeeds = new List<SeedSaveData>();//保存数据的list
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
    // 检查生长状态
    public void CheckGrowth()
    {
        foreach (var seed in plantedSeeds)
        {
            DateTime plantedTime = DateTime.FromBinary(long.Parse(seed.plantedDate));
            double elapsed = (DateTime.Now - plantedTime).TotalSeconds;

            if (elapsed >= seed.growDuration)
            {
                Debug.Log($"种子 {seed.seedId} 已成熟！");
            }
            else
            {
                Debug.Log($"种子 {seed.seedId} 还在生长，进度：{elapsed}/{seed.growDuration}");
            }
        }
    }

    // 种下种子
    public void PlantSeed(string seedId, float growDuration)
    {
        SeedSaveData seed = new SeedSaveData
        {
            seedId = seedId,
            plantedDate = DateTime.Now.ToBinary().ToString(),
            growDuration = growDuration
        };
        plantedSeeds.Add(seed);

        Debug.Log($"种下种子 {seedId}, 生长需要 {growDuration} 秒");
    }
}
