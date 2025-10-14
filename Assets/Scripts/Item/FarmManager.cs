using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance;   // 单例
    public List<SeedSaveData> plantedSeeds = new List<SeedSaveData>();//保存数据的list
    public List<RectTransform> farmGridAreas = new List<RectTransform>();
    private Boolean restoreBool = true; // 只有一开始会恢复所有的植物

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
        if (scene.name == "FarmScene" && restoreBool == true)
        {
            RestoreSeeds();
            restoreBool = false;
        }

        // 每次换场景重新收集
        farmGridAreas.Clear();

        GameObject[] cells = GameObject.FindGameObjectsWithTag("FarmGrid");
        foreach (var cell in cells)
        {
            RectTransform rt = cell.GetComponent<RectTransform>();
            if (rt != null) farmGridAreas.Add(rt);
        }
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

    public void RestoreSeeds()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "FarmScene")
            return;
        
        foreach (var seedData in plantedSeeds)
        {
            ItemData item = GameManager.Instance.itemDatabase.GetItemByName(seedData.seedId);
            // Debug.Log("item name: " + item);
            if (item == null) continue;

            Vector3 pos = new Vector3(seedData.posX, seedData.posY, seedData.posZ);
            GameObject plantedSeed = Instantiate(item.emptyPrefab, pos, Quaternion.identity);

            SeedManager manager = plantedSeed.AddComponent<SeedManager>();

            long binaryTime = long.Parse(seedData.plantedDate);
            DateTime plantedTime = DateTime.FromBinary(binaryTime);

            manager.Restore(item, plantedTime, seedData.growDuration);
        }
    }

}
