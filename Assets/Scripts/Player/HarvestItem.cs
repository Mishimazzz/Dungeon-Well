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

  private List<GameObject> gridObjs = new List<GameObject>();
  private List<ItemDisplay> bagSlots = new List<ItemDisplay>();

  public List<Vector3> itemPositions = new List<Vector3>
  {
    // Row 1 (Y = 100)
    new Vector3(-1100, 100, 0),
    new Vector3(-1000, 100, 0),
    new Vector3(-900, 100, 0),
    new Vector3(-800, 100, 0),
    new Vector3(-700, 100, 0),
    new Vector3(-600, 100, 0),
    new Vector3(-500, 100, 0),
    new Vector3(-400, 100, 0),
  };


  private int currentBagPositionIndex = 0;

  private void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  // 添加物品到背包（自动叠加）
  public void AddItemsInBag(Dictionary<ItemData, int> ItemDict)
  {
    foreach (var item in ItemDict)
    {
      if (playerBag.ContainsKey(item.Key))
        playerBag[item.Key] += item.Value;
      else
        playerBag[item.Key] = item.Value;
    }
    RefreshBagUI();
  }

  // 生成并刷新背包UI
  public void RefreshBagUI()
  {
    ClearBagUI();
    currentBagPositionIndex = 0;

    int i = 0;
    foreach (var kv in playerBag)
    {
      Vector3 position = GetNextBagPosition();

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
