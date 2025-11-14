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
        // 先收集格子
        farmGridAreas.Clear();

        GameObject[] cells = GameObject.FindGameObjectsWithTag("FarmGrid");
        foreach (var cell in cells)
        {
            RectTransform rt = cell.GetComponent<RectTransform>();
            if (rt != null) farmGridAreas.Add(rt);
        }

        // 再恢复种子 —— 必须在格子收集之后
        if (scene.name == "FarmScene" && restoreBool == true)
        {
            RestoreSeeds();
            restoreBool = false;
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

    private RectTransform FindNearestCell(Vector3 pos)
    {
        RectTransform nearest = null;
        float minDist = float.MaxValue;

        foreach (var cell in farmGridAreas)
        {
            float d = Vector2.Distance(cell.position, pos);
            if (d < minDist)
            {
                minDist = d;
                nearest = cell;
            }
        }

        return nearest;
    }


    public void RestoreSeeds()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "FarmScene")
            return;
        
        foreach (var seedData in plantedSeeds)
        {
            ItemData item = GameManager.Instance.itemDatabase.GetItemByName(seedData.seedId);
            Debug.Log("item name: " + item);
            if (item == null) continue;

            Vector3 pos = new Vector3(seedData.posX, seedData.posY, seedData.posZ);
            GameObject plantedSeed = Instantiate(item.emptyPrefab, pos, Quaternion.identity);

            SeedManager manager = plantedSeed.AddComponent<SeedManager>();

            // 找最近的格子
            RectTransform nearest = null;
            float minDist = float.MaxValue;

            foreach (var cell in farmGridAreas)
            {
                float d = Vector2.Distance(cell.position, pos);
                if (d < minDist)
                {
                    minDist = d;
                    nearest = cell;
                }
            }

            // 配置 ownerCell
            if (nearest != null)
            {
                FarmGridCell cell = nearest.GetComponent<FarmGridCell>();
                if (cell != null)
                {
                    cell.occupied = true;
                    manager.ownerCell = cell;
                }
            }

            long binaryTime = long.Parse(seedData.plantedDate);
            DateTime plantedTime = DateTime.FromBinary(binaryTime);

            manager.Restore(item, plantedTime, seedData.growDuration);
        }
    }

}
