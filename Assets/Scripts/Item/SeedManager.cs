using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
  public static ItemData seedInstance;
  public static SeedManager Instance;
  public float TotalTime;
  public float currentTime = 0f;
  private GameObject currentStageObj;
  public GameObject stage1Prefab;
  public GameObject stage2Prefab;
  public GameObject stage3Prefab;
  public ItemData harvestItem;
  private bool initialized = false;
  private bool hasChanged = false;
  public bool isHavestItem = false;
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
    currentTime = TotalTime;
    stage1Prefab = seed.firstPhasePrefab;
    stage2Prefab = seed.secondPhasePrefab;
    stage3Prefab = seed.thirdPhasePrefab;
    harvestItem = seed.harvestItem;

    initialized = true;
    Debug.Log("SeedManager 初始化完成");
  }

  void Update()
  {
    if (!initialized) return;

    if (currentTime > 0)
    {
      currentTime -= Time.deltaTime;
      UpdateStage();
    }
    else
    {
      //更新为第三阶段收获（在土里的图例）
      if (seedInstance != null)
      {
        SwitchPrefab(stage3Prefab);
        // OnMouseDown();
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

  public void UpdateStage()
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
      // 如果已经是同一个Prefab，不重复切换
      if (currentStageObj.name.Contains(newPrefab.name))
        return;

      Destroy(currentStageObj);
    }

    //放在世界坐标
    currentStageObj = Instantiate(newPrefab, transform.position, Quaternion.identity);
  }

  //点击收获农作物，会变成收获物到背包里
  // 点击收获农作物，会变成收获物到背包里
  void OnMouseDown()
  {
    if (hasChanged) return;
    if (currentStageObj != null && currentStageObj.name.Contains(stage3Prefab.name))
    {
      isHavestItem = true;
      Destroy(gameObject);
      Destroy(currentStageObj);   // 删除当前阶段实例
      currentStageObj = null;

      // 获取收获物数据
      ItemData harvestData = seedInstance.harvestItem;

      // 更新 playerBag 数据
      Dictionary<ItemData, int> newItems = new Dictionary<ItemData, int>();
      newItems[harvestData] = 1;
      HarvestItem.Instance.AddItemsInBag(newItems);

      // 刷新 UI（交给 RefreshBagUI 处理）
      HarvestItem.Instance.RefreshBagUI();

      Debug.Log("收获物品放入背包");
      hasChanged = true;
    }
  }

}
