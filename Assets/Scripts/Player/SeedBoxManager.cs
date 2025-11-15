using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBoxManager : MonoBehaviour
{
    public static SeedBoxManager seedBoxManager;
    public GameObject itemDisplayPrefab;    // 拖你的物品显示Prefab
    public Transform seedBoxPanel; // 拖你的种子仓库Panel（父物体）
    public GameObject SeedBoxPanel;
    public List<ItemDisplay> seedSlots = new List<ItemDisplay>();
    public bool needRefreshSeedBox = false;
    public List<Vector3> seedBoxPosition = new List<Vector3> { new Vector3(-35, -55, 0), new Vector3(62, -55, 0) };

    void Start()
    {
        if (HarvestItem.Instance.pendingSeedBoxRefresh)
        {
            needRefreshSeedBox = true;
            HarvestItem.Instance.pendingSeedBoxRefresh = false;
        }
    }
    
    void Awake()
    {
        seedBoxManager = this;
    }
    void Update()
    {
        if (needRefreshSeedBox && SeedBoxPanel.activeSelf)
        {
            RefreshSeedBoxUI();
            needRefreshSeedBox = false; // 刷新一次后重置
        }
    }

    public void RefreshSeedBoxUI()
    {
        int i = 0;
        foreach (var kv in HarvestItem.Instance.playerBag)
        {
            if (kv.Key.isSeed == Seed.No)
                continue; // 跳过普通物品

            ItemDisplay display;
            if (i < seedSlots.Count)
            {
                display = seedSlots[i];
            }
            else
            {
                GameObject go = Instantiate(itemDisplayPrefab, seedBoxPanel);
                go.transform.localPosition = seedBoxPosition[i % seedBoxPosition.Count];
                display = go.GetComponent<ItemDisplay>();
                seedSlots.Add(display);
            }

            // 设置icon和数量
            Sprite icon = null;
            if (kv.Key.prefab != null)
            {
                var sr = kv.Key.prefab.GetComponent<SpriteRenderer>();
                if (sr != null) icon = sr.sprite;
            }
            display.SetItem(icon, kv.Value, kv.Key);
            display.gameObject.SetActive(true);

            i++;
        }
        // 隐藏多余的格子
        for (; i < seedSlots.Count; i++)
        {
            seedSlots[i].gameObject.SetActive(false);
        }
    }
}
