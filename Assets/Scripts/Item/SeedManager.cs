using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
  public static ItemData seedInstance;
  public static SeedManager Instance;

  private GameObject currentStageObj;
  public GameObject stage1Prefab;
  public GameObject stage2Prefab;
  public GameObject stage3Prefab;
  public ItemData harvestItem;

  private bool initialized = false;
  private bool hasChanged = false;
  public bool isHavestItem = false;

  public float TotalTime;
  private float plantedTime; // 记录种下时间

  public void Init(ItemData seed)
  {
    if (seed == null || seed.isSeed == Seed.No)
    {
      Debug.LogWarning("传入的不是种子，SeedManager不会启动");
      return;
    }

    Debug.Log("进入了");
    seedInstance = seed;

    TotalTime = seed.thirdPhase;
    plantedTime = Time.time; // 种下的时间点

    stage1Prefab = seed.firstPhasePrefab;
    stage2Prefab = seed.secondPhasePrefab;
    stage3Prefab = seed.thirdPhasePrefab;
    harvestItem = seed.harvestItem;

    initialized = true;
    hasChanged = false;

    Debug.Log("SeedManager 初始化完成");
  }

  void Awake()
  {
    DontDestroyOnLoad(gameObject); // 跨场景不销毁
  }

  void Update()
  {
    if (!initialized) return;
    if (hasChanged) return;

    // 计算剩余时间
    float elapsed = Time.time - plantedTime;
    float currentTime = TotalTime - elapsed;

    if (currentTime > 0)
    {
      UpdateStage(currentTime);
    }
    else
    {
      // 长到第三阶段
      if (seedInstance != null)
      {
        SwitchPrefab(stage3Prefab);

        if (Input.GetMouseButtonDown(0))
        {
          Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

          if (hit.collider != null && hit.collider.gameObject == currentStageObj)
          {
            OnMouseDown();
          }
        }
      }
    }
  }

  public void UpdateStage(float currentTime)
  {
    float stage1Time = seedInstance.firstPhase;
    float stage2Time = seedInstance.secondPhase;

    if (currentTime > stage2Time) SwitchPrefab(stage1Prefab);
    else if (currentTime > stage1Time) SwitchPrefab(stage2Prefab);
  }

  void SwitchPrefab(GameObject newPrefab)
  {
    if (currentStageObj != null)
    {
      if (currentStageObj.name.Contains(newPrefab.name))
        return;

      Destroy(currentStageObj);
    }

    currentStageObj = Instantiate(newPrefab, transform.position, Quaternion.identity);
  }

  void OnMouseDown()
  {
    if (hasChanged) return;
    if (currentStageObj != null && currentStageObj.name.Contains(stage3Prefab.name))
    {
      isHavestItem = true;
      Destroy(gameObject);
      Destroy(currentStageObj);
      currentStageObj = null;

      ItemData harvestData = seedInstance.harvestItem;

      Dictionary<ItemData, int> newItems = new Dictionary<ItemData, int>();
      newItems[harvestData] = 1;
      HarvestItem.Instance.AddItemsInBag(newItems);

      HarvestItem.Instance.RefreshBagUI();

      Debug.Log("收获物品放入背包");
      hasChanged = true;
    }
  }
}
