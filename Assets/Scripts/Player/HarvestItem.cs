using System.Collections;
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
  

  public Transform seedBoxPanel; // 拖你的种子仓库Panel（父物体）
  public GameObject SeedBoxPanel;
  public GameObject seedGridPrefab;
  private List<GameObject> seedGridObjs = new List<GameObject>();
  private List<ItemDisplay> seedSlots = new List<ItemDisplay>();
  public bool needRefreshSeedBox = false;


  private List<GameObject> gridObjs = new List<GameObject>();
  private List<ItemDisplay> bagSlots = new List<ItemDisplay>();

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

  public List<Vector3> seedBoxPosition = new List<Vector3> { new Vector3(-95, -54, 0) };

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


  private int currentBagPositionIndex = 0;

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
    ClearBagUI();
    currentBagPositionIndex = 0;
    // int currentSeedPositionIndex = 0;

    int i = 0;
    foreach (var kv in playerBag)
    {
      // 只显示普通物品
      if (kv.Key.isSeed == Seed.Yes)
        continue;

      // 用普通物品位置
      Vector3 position;
      if (currentBagPositionIndex >= itemPositions.Count)
        currentBagPositionIndex = 0;
      position = itemPositions[currentBagPositionIndex++];

      // 生成Grid
      GameObject gridGo = Instantiate(gridPrefab, bagPanel);
      gridGo.transform.localPosition = position;
      gridObjs.Add(gridGo);

      // 生成ItemDisplay，作为Grid的子物体
      GameObject go = Instantiate(itemDisplayPrefab, gridGo.transform);
      go.transform.localPosition = Vector3.zero;
      ItemDisplay display = go.GetComponent<ItemDisplay>();
      bagSlots.Add(display);

      // 设置icon和数量
      Sprite icon = null;
      if (kv.Key.prefab != null)
      {
        var sr = kv.Key.prefab.GetComponent<SpriteRenderer>();
        if (sr != null) icon = sr.sprite;
      }
      display.SetItem(icon, kv.Value);
      display.gameObject.SetActive(true);
      gridGo.SetActive(true);

      i++;
    }
  }

  public void RefreshSeedBoxUI()
  {
    // 清空种子仓库UI
    foreach (var grid in seedGridObjs)
    {
      Destroy(grid);
    }
    seedGridObjs.Clear();
    seedSlots.Clear();
    int currentSeedPositionIndex = 0;

    foreach (var kv in playerBag)
    {
      if (kv.Key.isSeed == Seed.No)
        continue; // 跳过普通物品

      // 用种子专用位置
      Vector3 position;
      if (currentSeedPositionIndex >= seedBoxPosition.Count)
        currentSeedPositionIndex = 0;
      position = seedBoxPosition[currentSeedPositionIndex++];

      // 生成Grid
      GameObject gridGo = Instantiate(seedGridPrefab, seedBoxPanel);
      gridGo.transform.localPosition = position;
      seedGridObjs.Add(gridGo);

      // 生成ItemDisplay，作为Grid的子物体
      GameObject go = Instantiate(itemDisplayPrefab, gridGo.transform);
      go.transform.localPosition = Vector3.zero;
      ItemDisplay display = go.GetComponent<ItemDisplay>();
      seedSlots.Add(display);

      // 设置icon和数量
      Sprite icon = null;
      if (kv.Key.prefab != null)
      {
        var sr = kv.Key.prefab.GetComponent<SpriteRenderer>();
        if (sr != null) icon = sr.sprite;
      }
      display.SetItem(icon, kv.Value);
      display.gameObject.SetActive(true);
      gridGo.SetActive(true);
    }
  }

  private Vector3 GetNextBagPosition()
  {
    if (currentBagPositionIndex >= itemPositions.Count)
      currentBagPositionIndex = 0;
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




}
