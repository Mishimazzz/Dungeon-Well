using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HarvestItem : MonoBehaviour
{
  public Dictionary<ItemData, int> playerBag = new Dictionary<ItemData, int>();
  public static HarvestItem Instance;
  public Dictionary<ItemData, int> GetPlayerBag() => playerBag;

  public GameObject itemDisplayPrefab;    // 拖你的物品显示Prefab
  public Transform bagPanel;              // 拖你的背包Panel（父物体）
  public GameObject BagPanel;
  public bool needRefreshBag = false;
  private int currentBagPositionIndex = 0;
  public int currentBagPage = 0;
  public int totalPages = 20;//总共设置page 20页
  public bool pendingSeedBoxRefresh = false;
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
  public CoinDisplay coinDisplay;

  void Update()
  {
    if (needRefreshBag && BagPanel.activeSelf)
    {
      RefreshBagUI();
      needRefreshBag = false; // 刷新一次后重置
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
    SeedBoxManager.seedBoxManager.needRefreshSeedBox = true;
  }

  // 生成并刷新背包UI
  public void RefreshBagUI()
  {
    // 清空旧UI
    foreach (var slot in bagSlots)
      Destroy(slot.gameObject);
    bagSlots.Clear();
    currentBagPositionIndex = 0;

    // 过滤要显示的物品
    List<KeyValuePair<ItemData, int>> displayList = new List<KeyValuePair<ItemData, int>>();
    foreach (var kv in playerBag)
    {
      if (kv.Key.isSeed == Seed.No && kv.Value > 0)
        displayList.Add(kv);
    }

    // 计算分页
    int perPageCount = itemPositions.Count;  // 每页9个
    totalPages = Mathf.CeilToInt((float)displayList.Count / perPageCount);

    int start = currentBagPage * perPageCount;
    int end = Mathf.Min(start + perPageCount, displayList.Count);
    int shownCount = 0;
    int nonCoinCount = 0;
    int i = 0;

    // 跳过前几页的非 coin 物品
    while (i < displayList.Count && nonCoinCount < currentBagPage * itemPositions.Count)
    {
      if (displayList[i].Key.name != "Coin")
        nonCoinCount++;
      i++;
    }

    // 从这里开始显示本页内容
    shownCount = 0;
    while (i < displayList.Count && shownCount < itemPositions.Count)
    {
      var kv = displayList[i];

      if (kv.Key.name == "Coin")
      {
        coinDisplay.SetCoin(kv.Value, kv.Key);
        i++;
        continue;
      }

      // 非 coin：显示并计格
      Vector3 pos = itemPositions[shownCount];
      GameObject go = Instantiate(itemDisplayPrefab, bagPanel);
      go.transform.localPosition = pos;

      ItemDisplay display = go.GetComponent<ItemDisplay>();
      Sprite icon = kv.Key.prefab ? kv.Key.prefab.GetComponent<SpriteRenderer>()?.sprite : null;
      display.SetItem(icon, kv.Value, kv.Key);

      bagSlots.Add(display);

      shownCount++;
      i++;
    }

    // Debug.Log($"当前第 {currentBagPage + 1}/{totalPages} 页");

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
    if (SeedBoxManager.seedBoxManager != null)
    SeedBoxManager.seedBoxManager.needRefreshSeedBox = true;
  }

  // 清空背包（加载时用）
  public void ClearBag()
  {
    playerBag.Clear();
    bagSlots.Clear();
    currentBagPositionIndex = 0;
    needRefreshBag = true;

    if (SeedBoxManager.seedBoxManager != null)
      SeedBoxManager.seedBoxManager.needRefreshSeedBox = true;
    else
      pendingSeedBoxRefresh = true;
  }

  //背包翻页系统
  public void NextPage()
  {
    if (currentBagPage < totalPages - 1)
    {
      currentBagPage++;
      RefreshBagUI();
    }
  }

  public void PrevPage()
  {
    if (currentBagPage > 0)
    {
      currentBagPage--;
      RefreshBagUI();
    }
  }

}

