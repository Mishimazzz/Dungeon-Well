using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDatabase itemCommonList;
    public ItemDatabase itemRareList;
    public ItemDatabase itemUltraRareList;
    public ButtonManage buttonManage;
    public TimeStopController timeStopController;
    private float executeTime;

    // items UI
    public List<ItemDisplay> itemSlots;
    private Dictionary<ItemData, int> itemDict = new Dictionary<ItemData, int>();
    public GameObject itemDisplayPrefab;
    public Transform canvas;
    private Vector3 firstPosition = new Vector3(-685, 140, 0);
    public List<Vector3> itemPositions = new List<Vector3> { new Vector3(-685, 140, 0), new Vector3(-345, 140, 0), new Vector3(-5, 140, 0), new Vector3(335, 140, 0) };
    private int currentItemPositionIndex = 1;
    public GameObject GridPrefab;
    //test
    public ItemData coinItemData; // Assign the coin ItemData in the Inspector
    private int totalItemHavest;//havest # of items except coin 

    public void SpawItem()
    {
        /*
            -> if the setting time <= 1 minutes, only got coins.
            -> setting time 1 - 5 minutes, 1 item + coins
            -> setting time 6 - 15 minutes, 3 item + coins
            -> setting time 16 - 30 minutes, 6 item + coins + 1 rare item
            -> setting time 31 - 45  minutes, 10 item + coins + 2 rare item
            -> setting time 45 - 60  minutes, 16 item + coins + 2 rare item + 1 ultra Rare
            -> after 1 hour, 16 items + coins + 3 rare + 1 ultra rare x per hour
        */

        //get player execute time
        executeTime = GetTime();

        //basic setting
        int itemCount = 0;
        int rareCount = 0;
        int ultraRareCount = 0;

        if (executeTime <= 0.01)//1
        {
            GiveCoin(1);
        }
        else if (executeTime <= 0.02)//5
        {
            itemCount = 1;
            GiveCoin(2);
        }
        else if (executeTime <= 0.03)//15
        {
            itemCount = 3;
            GiveCoin(3);
        }
        else if (executeTime <= 30)
        {
            itemCount = 6;
            rareCount = 1;
            GiveCoin(4);
        }
        else if (executeTime <= 45)
        {
            itemCount = 10;
            rareCount = 2;
            GiveCoin(5);
        }
        else if (executeTime <= 60)
        {
            itemCount = 16;
            rareCount = 2;
            ultraRareCount = 1;
            GiveCoin(5);
        }
        else
        {
            int hourCount = Mathf.FloorToInt(executeTime / 60f);
            itemCount = 16;
            rareCount = 3;
            ultraRareCount = hourCount;
            GiveCoin(5 * hourCount);
        }

        totalItemHavest = itemCount + rareCount + ultraRareCount;
        //load item prefab and UI, icon
        for (int i = 0; i < totalItemHavest; i++)
        {
            Vector3 position = GetNextItemPosition();
            // SpawnGrid(position);
            GameObject go = Instantiate(itemDisplayPrefab, canvas);
            go.transform.localPosition = position;

            ItemDisplay display = go.GetComponent<ItemDisplay>();
            if (display == null)
            {
                Debug.LogError("itemDisplayPrefab 上没有 ItemDisplay 脚本！");
                return;
            }
            itemSlots.Add(display);
            go.SetActive(false);
        }

        SpawnItemsByRarity(Rare.Common, itemCount);
        SpawnItemsByRarity(Rare.Rare, rareCount);
        SpawnItemsByRarity(Rare.UltraRare, ultraRareCount);
    }

    public void SpawnItemsByRarity(Rare rareLevel, int count)
    {
        if (rareLevel == Rare.Common && count > 0)
        {
            while (count > 0)
            {
                SpawnItem(itemCommonList);
                count--;
            }
        }
        else if (rareLevel == Rare.Rare && count > 0)
        {
            while (count > 0)
            {
                SpawnItem(itemRareList);
                count--;
            }
        }
        else if (rareLevel == Rare.UltraRare && count > 0)
        {
            while (count > 0)
            {
                SpawnItem(itemUltraRareList);
                count--;
            }
        }
    }

    public void SpawnItem(ItemDatabase itemDatabase)
    {
        float total = 0f;
        foreach (var item in itemDatabase.Iteams)
            total += item.probability;

        float rand = Random.Range(0f, total);
        float current = 0f;

        foreach (var item in itemDatabase.Iteams)
        {
            current += item.probability;
            if (rand <= current)
            {
                AddItem(item, 1);
                break;
            }
        }
    }

    public void SpawnGrid(Vector3 position)
    {
        //generate grid
        GameObject go = Instantiate(GridPrefab, canvas);
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
        Debug.Log("itemDict count: " + itemDict.Count);
        int i = 0;
        foreach (var kv in itemDict)
        {
            Debug.Log($"显示物品: {kv.Key.name}, 数量: {kv.Value}");
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
                SpawnGrid(itemSlots[i].transform.localPosition);
                i++;
            }
        }
        for (; i < itemSlots.Count; i++)
            itemSlots[i].gameObject.SetActive(false);
    }

    private Vector3 GetNextItemPosition()
    {
        // Debug.Log(currentItemPositionIndex);
        // Debug.Log(itemPositions.Count);
        if (currentItemPositionIndex >= itemPositions.Count)
            currentItemPositionIndex = 0;

        // Debug.Log("get"+ itemPositions[currentItemPositionIndex]);
        return itemPositions[currentItemPositionIndex++];
    }

    public void ClearAllItemDisplays()
    {
        // 删除所有 ItemDisplayPrefab(Clone) 和 Grid(Clone)
        var objs = GameObject.FindObjectsOfType<GameObject>();
        foreach (var obj in objs)
        {
            if (obj.name == "ItemDisplayPrefab(Clone)")
            {
                Destroy(obj);
            }
            if (obj.name == "Grid(Clone)")
            {
                Destroy(obj);
            }
        }
        itemSlots.Clear();
        itemDict.Clear();
        currentItemPositionIndex = 1; 
    }
}