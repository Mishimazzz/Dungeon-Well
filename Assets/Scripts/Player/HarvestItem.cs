using System.Collections.Generic;
using UnityEngine;

public class HarvestItem : MonoBehaviour
{
  private Dictionary<ItemData, int> playerBag = new Dictionary<ItemData, int>();
  public static HarvestItem Instance;

  public GameObject gridPrefab;           // 拖你的背包格子Prefab
  public GameObject itemDisplayPrefab;    // 拖你的物品显示Prefab
  public Transform bagPanel;              // 拖你的背包Panel（父物体）
  public GameObject BagPanel;
  public bool needRefreshBag = false;
  private int currentBagPositionIndex = 0;


  public Transform seedBoxPanel; // 拖你的种子仓库Panel（父物体）
  public GameObject SeedBoxPanel;
  public GameObject seedGridPrefab;
  private List<GameObject> seedGridObjs = new List<GameObject>();
  public List<ItemDisplay> seedSlots = new List<ItemDisplay>();
  public bool needRefreshSeedBox = false;


  public List<GameObject> gridObjs = new List<GameObject>();
  public List<ItemDisplay> bagSlots = new List<ItemDisplay>();

  public List<Vector3> itemPositions = new List<Vector3>
  {
    new Vector3(-470, -50, 0),
    new Vector3(-388, -50, 0),
    new Vector3(-304, -50, 0),
    new Vector3(-470, -130, 0),
    new Vector3(-388, -130, 0),
    new Vector3(-304, -130, 0),
    new Vector3(-470, -210, 0),
    new Vector3(-388, -210, 0),
    new Vector3(-304, -210, 0),
  };

  public Vector3 coinPosition = new Vector3(-425, -260, 0);
  public List<Vector3> seedBoxPosition = new List<Vector3> { new Vector3(-63, 57, 0) };
  public CoinDisplay coinDisplay;

  void Update()
  {
    if (needRefreshBag && BagPanel.activeSelf)
    {
      RefreshBagUI();
      needRefreshBag = false; // 刷新一次后重置
    }

    if (needRefreshSeedBox && SeedBoxPanel.activeSelf)
    {
      RefreshSeedBoxUI();
      needRefreshSeedBox = false; // 刷新一次后重置
    }
  }

  private void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  // 添加物品到背包,自动叠加
  public void AddItemsInBag(Dictionary<ItemData, int> ItemDict)
  {
    foreach (var item in ItemDict)
    {
      if (playerBag.ContainsKey(item.Key))
        playerBag[item.Key] += item.Value;
      else
        playerBag[item.Key] = item.Value;
    }
    needRefreshBag = true;
    needRefreshSeedBox = true;
  }

  // 生成并刷新背包UI
  public void RefreshBagUI()
  {
    int i = 0;
    foreach (var kv in playerBag)
    {
      if (kv.Key.isSeed == Seed.Yes) continue;

      Vector3 pos = kv.Key.name == "Coin" ? coinPosition : itemPositions[i % itemPositions.Count];

      GameObject gridGo, go;
      ItemDisplay display;

      if (kv.Key.name == "Coin") // 硬币单独处理，不占用 i
      {
        coinDisplay.SetCoin(kv.Value, kv.Key);
        continue;
      }
      else // 普通物品
      {
        gridGo = Instantiate(gridPrefab, bagPanel);
        gridGo.transform.localPosition = pos;

        go = Instantiate(itemDisplayPrefab, bagPanel);
        go.transform.localPosition = pos;
        i++; // 普通物品才 i++
      }

      display = go.GetComponent<ItemDisplay>();
      Sprite icon = kv.Key.prefab ? kv.Key.prefab.GetComponent<SpriteRenderer>()?.sprite : null;
      display.SetItem(icon, kv.Value, kv.Key);
    }
    // 隐藏多余的格子
    for (; i < bagSlots.Count; i++)
    {
      bagSlots[i].gameObject.SetActive(false);
      gridObjs[i].SetActive(false);
    }
  }

  public void RefreshSeedBoxUI()
  {
    int i = 0;
    foreach (var kv in playerBag)
    {
      if (kv.Key.isSeed == Seed.No)
        continue; // 跳过普通物品

      ItemDisplay display;
      GameObject gridGo;
      if (i < seedSlots.Count)
      {
        display = seedSlots[i];
        gridGo = seedGridObjs[i];
      }
      else
      {
        gridGo = Instantiate(seedGridPrefab, seedBoxPanel);
        gridGo.transform.localPosition = seedBoxPosition[i % seedBoxPosition.Count];
        seedGridObjs.Add(gridGo);

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
      gridGo.SetActive(true);

      i++;
    }
    // 隐藏多余的格子
    for (; i < seedSlots.Count; i++)
    {
      seedSlots[i].gameObject.SetActive(false);
      seedGridObjs[i].SetActive(false);
    }
  }

  public Vector3 GetNextBagPosition()
  {
    if (currentBagPositionIndex >= itemPositions.Count)
      currentBagPositionIndex = 0;
    Debug.Log("position index:" + currentBagPositionIndex);
    return itemPositions[currentBagPositionIndex++];
  }

  // 清空背包UI
  public void ClearBagUI()
  {
    foreach (var grid in gridObjs)
    {
      Destroy(grid);
    }
    gridObjs.Clear();
    bagSlots.Clear();
    currentBagPositionIndex = 0;
  }

  // 检查是否有物品
  public bool HasItem(ItemData data)
  {
    return playerBag.ContainsKey(data) && playerBag[data] > 0;
  }

  // 消耗物品
  public void ConsumeItem(ItemData data, int amount)
  {
    if (playerBag.ContainsKey(data))
    {
      playerBag[data] -= amount;
      if (playerBag[data] <= 0)
        playerBag.Remove(data);
    }
  }

  // 添加单个物品(一般给农作物用)
  public void AddOneToBag(ItemData data, int count = 1)
  {
    if (playerBag.ContainsKey(data))
      playerBag[data] += count;
    else
      playerBag[data] = count;

    needRefreshBag = true;
    needRefreshSeedBox = true;
  }
}
