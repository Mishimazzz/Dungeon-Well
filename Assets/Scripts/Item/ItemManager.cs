using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Level characterLevel = Level.E; // TODO: import主角的等级
    public ButtonManage buttonManage;
    public TimeStopController timeStopController;
    private float executeTime;

    // items UI
    public List<ItemDisplay> itemSlots;
    public Dictionary<ItemData, int> itemDict = new Dictionary<ItemData, int>();
    public GameObject itemDisplayPrefab;
    public Transform canvas;
    //bag position
    public List<Vector3> itemPositions = new List<Vector3> { new Vector3(-5, -85, 0), new Vector3(85, -85, 0), new Vector3(175, -85, 0), new Vector3(265, -85, 0),
                                                            new Vector3(-5, -185, 0), new Vector3(85, -185, 0), new Vector3(175, -185, 0), new Vector3(265, -185, 0) };

    private int currentItemPositionIndex = 1;
    private Vector3 firstPosition = new Vector3(-5, -85, 0);

    // seedbox position
    public List<Vector3> seedBoxPosition = new List<Vector3> { new Vector3(-256, -129, 0) };
    private int currentSeedPositionIndex = 0;
    public GameObject GridPrefab;
    //test
    public ItemData coinItemData; // Assign the coin ItemData in the Inspector
    private int totalItemHavest;//havest # of items except coin 

    private HashSet<string> spawnedItems = new HashSet<string>();

    public ItemDatabase itemDatabase;//item大列表

    private List<GameObject> DestroyItems = new List<GameObject>();// 之后instanitiate物品之后需要删除的

    public void SpawItem()
    {
        /*
            -> if the setting time <= 1 minutes, only got coins.
            -> setting time 1 - 5 minutes, 1 item + coins
            -> setting time 6 - 15 minutes, 2 LowLevel + 1 Mid +  coins
            -> setting time 16 - 30 minutes, 4 LowLevel + 2 Mid + coins + 1 HighLevel
            -> setting time 31 - 45  minutes, 7 LowLevel + 3 Mid + coins + 2 HighLevel
            -> setting time 45 - 60  minutes, 10 LowLevel + 4 Mid + coins + 3 HighLevel + 1 UltraRare
            -> after 1 hour, 10 LowLevel + 4 Mid + coins + 3 HighLevel + 1 UltraRare x per hour
        */

        //get player execute time
        executeTime = GetTime();

        //basic setting
        int LowLevelCount = 0;
        int MidLevelCount = 0;
        int HighLevelCount = 0;
        int UltraRareCount = 0;

        if (executeTime <= 0.01)//1
        {
            GiveCoin(1);
        }
        else if (executeTime <= 0.08)//5
        {
            LowLevelCount = 1;
            GiveCoin(2);
        }
        else if (executeTime <= 15)//15
        {
            LowLevelCount = 2;
            MidLevelCount = 1;
            GiveCoin(3);
        }
        else if (executeTime <= 30)
        {
            LowLevelCount = 4;
            MidLevelCount = 2;
            HighLevelCount = 1;
            GiveCoin(4);
        }
        else if (executeTime <= 45)
        {
            LowLevelCount = 7;
            MidLevelCount = 3;
            HighLevelCount = 2;
            GiveCoin(5);
        }
        else if (executeTime <= 60)
        {
            LowLevelCount = 10;
            MidLevelCount = 4;
            HighLevelCount = 3;
            UltraRareCount = 1;
            GiveCoin(5);
        }
        else
        {
            int hourCount = Mathf.FloorToInt(executeTime / 60f);
            LowLevelCount = 10;
            MidLevelCount = 4;
            HighLevelCount = 3;
            UltraRareCount = hourCount;
            GiveCoin(5 * hourCount);
        }

        // totalItemHavest = LowLevelCount + MidLevelCount + HighLevelCount + UltraRareCount;
        // //load item prefab and UI, icon
        // for (int i = 0; i < totalItemHavest; i++)
        // {
        //     Vector3 position = GetNextItemPosition();
        //     SpawnGrid(position);
        //     GameObject go = Instantiate(itemDisplayPrefab, canvas);
        //     DestroyItems.Add(go);
        //     go.transform.localPosition = position;

        //     ItemDisplay display = go.GetComponent<ItemDisplay>();
        //     itemSlots.Add(display);
        //     go.SetActive(false);
        // }

        SpawnItemsByRarity(Rare.LowLevel, LowLevelCount);
        SpawnItemsByRarity(Rare.MidLevel, MidLevelCount);
        SpawnItemsByRarity(Rare.HighLevel, HighLevelCount);
        SpawnItemsByRarity(Rare.UltraRare, UltraRareCount);
    }

    public void SpawnItemsByRarity(Rare rareLevel, int count)
    {
        while (count > 0)
        {
            // 1. 随机一个向下兼容等级
            Level level = GetRandomLevel();
            Debug.Log("Level: " + level);
            Debug.Log("rareLevel: " + rareLevel);
            Debug.Log("count: " + count);

            // 2. 根据等级 + 稀有度，从数据库抽物品
            SpawnItem(level, rareLevel);

            count--;
        }
    }

    public void SpawnItem(Level level, Rare rare)
    {
        List<ItemData> list = itemDatabase.GetItems(level, rare);
        if (list == null || list.Count == 0) return;

        float total = 0f;
        foreach (var item in list) total += item.probability;

        float rand = Random.Range(0f, total);
        float current = 0f;

        foreach (var item in list)
        {
            current += item.probability;
            if (rand <= current)
            {
                Vector3 position = GetNextItemPosition(item);

                // 生成 Grid
                SpawnGrid(position);

                // 生成 UI
                GameObject go = Instantiate(itemDisplayPrefab, canvas);
                DestroyItems.Add(go);
                go.transform.localPosition = position;

                ItemDisplay display = go.GetComponent<ItemDisplay>();
                itemSlots.Add(display);
                go.SetActive(false);

                // 逻辑层：加入物品
                AddItem(item, 1);
                break;
            }
        }
    }

    public void SpawnGrid(Vector3 position)
    {
        //generate grid
        GameObject go = Instantiate(GridPrefab, canvas);
        DestroyItems.Add(go);
        go.transform.localPosition = position;
        go.transform.SetSiblingIndex(8);//second layer,你得数一下Time里头的层数
    }

    public float GetTime()
    {
        float returnTime = 0f;
        if (buttonManage.TotalFullExecuteTime != 0f)
        {
            returnTime = buttonManage.TotalFullExecuteTime;
        }
        else if (timeStopController.TotalExecuteTime != 0f)
        {
            returnTime = timeStopController.TotalExecuteTime;
        }

        return returnTime;
    }

    public void GiveCoin(int amount)
    {
        /*
            -> setting time 1 - 5 minutes, 1 coins
            -> setting time 6 - 15 minutes, 2 coins
            -> setting time 16 - 30 minutes, 3 coins
            -> setting time 31 - 45  minutes, 4 coins
            -> setting time 45 - 60  minutes, 5 coins
            -> after 1 hour, 600 coins x per hour
            *后续可以根据分钟，秒数的变化去改，这里只是写了逻辑,看看生成能不能work
        */
        GameObject go = Instantiate(itemDisplayPrefab, canvas);
        DestroyItems.Add(go);
        go.transform.localPosition = firstPosition;
        SpawnGrid(firstPosition);
        ItemDisplay display = go.GetComponent<ItemDisplay>();
        if (display == null)
        {
            Debug.LogError("itemDisplayPrefab 上没有 ItemDisplay 脚本！");
            return;
        }
        // Debug.Log("add");
        itemSlots.Add(display);
        go.SetActive(false);
        while (amount > 0)
        {
            // Debug.Log("amount:" + amount);
            AddItem(coinItemData, 1);
            amount--;
        }
    }

    // 添加物品
    public void AddItem(ItemData data, int amount)
    {
        if (!itemDict.ContainsKey(data))
            itemDict[data] = 0;
        itemDict[data] += amount;
        RefreshUI();
    }

    public void RefreshUI()
    {
        //Debug.Log("itemDict count: " + itemDict.Count);
        int i = 0;
        foreach (var kv in itemDict)
        {
            //Debug.Log($"显示物品: {kv.Key.name}, 数量: {kv.Value}");
            // if(itemSlots[0] == null) Debug.Log("itemSlots[0] = ", itemSlots[0]);

            if (i < itemSlots.Count)
            {
                Sprite icon = null;
                if (kv.Key.prefab != null)
                {
                    var sr = kv.Key.prefab.GetComponent<SpriteRenderer>();
                    if (sr != null) icon = sr.sprite;
                }
                itemSlots[i].SetItem(icon, kv.Value);
                itemSlots[i].gameObject.SetActive(true);
                // 只在第一次遇到该物品时生成 grid
                if (!spawnedItems.Contains(kv.Key.name))
                {
                    SpawnGrid(itemSlots[i].transform.localPosition);
                    spawnedItems.Add(kv.Key.name);
                }
                i++;
            }
        }
        for (; i < itemSlots.Count; i++)
            itemSlots[i].gameObject.SetActive(false);
    }

    private Vector3 GetNextItemPosition(ItemData data)
    {
        if (data.isSeed == Seed.Yes)
        {
            if (currentSeedPositionIndex >= seedBoxPosition.Count)
                currentSeedPositionIndex = 0;
            return seedBoxPosition[currentSeedPositionIndex++];
        }
        else
        {
            if (currentItemPositionIndex >= itemPositions.Count)
                currentItemPositionIndex = 0;
            return itemPositions[currentItemPositionIndex++];
        }
    }

    //选择一个向下兼容，且随机的等级
    private Level GetRandomLevel()
    {
        //向下兼容的等级list
        List<Level> levels = new List<Level>();

        foreach (Level lv in System.Enum.GetValues(typeof(Level)))
        {
            if ((int)lv <= (int)characterLevel)
            {
                levels.Add(lv);
            }
        }

        //随机
        int index = Random.Range(0, levels.Count);
        return levels[index];
    }
    

    public void ClearAllItemDisplays()
    {
        // 删除所有 ItemDisplayPrefab(Clone) 和 Grid(Clone)
        foreach (var obj in DestroyItems)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedItems.Clear();
        itemSlots.Clear();
        itemDict.Clear();
        spawnedItems.Clear();
        currentItemPositionIndex = 1;
    }
}