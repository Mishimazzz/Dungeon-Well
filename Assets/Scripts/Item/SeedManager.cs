using System.Collections.Generic;
using UnityEngine;
using System;

public class SeedManager : MonoBehaviour
{
  public ItemData seedInstance;
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
  public FarmGridCell ownerCell;

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

  public void SwitchPrefab(GameObject newPrefab)
  {
    if (currentStageObj != null)
    {
      if (currentStageObj.name.StartsWith(newPrefab.name))
      {
        //Debug.Log("已经是目标Prefab: " + newPrefab.name);
        return;
      }

      DestroyImmediate(currentStageObj); // 立刻销毁
    }

    //Debug.Log("切换Prefab为: " + newPrefab.name);
    currentStageObj = Instantiate(newPrefab, transform.position, Quaternion.identity, transform);

    var sr = currentStageObj.GetComponentInChildren<SpriteRenderer>();
    if (sr == null)
    {
      //Debug.LogWarning("新Prefab没有SpriteRenderer: " + newPrefab.name);
    }
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

      // 释放土地
      if (ownerCell != null)
      {
        ownerCell.occupied = false;

        if (FarmManager.Instance != null)
        {
          bool removed = FarmManager.Instance.plantedCells.Remove(ownerCell.cellId);
        }
      }

      Dictionary<ItemData, int> newItems = new Dictionary<ItemData, int>();
      newItems[harvestData] = 1;
      HarvestItem.Instance.AddItemsInBag(newItems);
      HarvestItem.Instance.RefreshBagUI();

      //点击收货后，从保存数据list里移出去
      if (FarmManager.Instance != null)
      {
          int beforeCount = FarmManager.Instance.plantedSeeds.Count;
          // 执行移除
          int removedCount = FarmManager.Instance.plantedSeeds.RemoveAll(s =>
          {
              bool sameId = s.seedId == seedInstance.name;
              bool sameX = Mathf.Abs(s.posX - transform.position.x) < 0.01f;
              bool sameY = Mathf.Abs(s.posY - transform.position.y) < 0.01f;
              // 不比较 z
              if (sameId && sameX && sameY)
              {
                  return true;
              }
              else
              {
                  return false;
              }
          });
        int afterCount = FarmManager.Instance.plantedSeeds.Count;
      }
    }
  }

  public void Restore(ItemData seed, DateTime plantedDate, float growDuration)
  {
    if (seed == null || seed.isSeed == Seed.No) return;

    seedInstance = seed;
    TotalTime = growDuration;

    stage1Prefab = seed.firstPhasePrefab;
    stage2Prefab = seed.secondPhasePrefab;
    stage3Prefab = seed.thirdPhasePrefab;
    harvestItem = seed.harvestItem;

    double elapsed = (DateTime.Now - plantedDate).TotalSeconds;    // 真实已过去的秒数
    plantedTime = Time.time - Mathf.Max(0f, (float)elapsed);       // 回填

    initialized = true;
    hasChanged = false;

    float currentTime = TotalTime - (Time.time - plantedTime);
    if (currentTime <= 0f) SwitchPrefab(stage3Prefab);
    else UpdateStage(currentTime);
  }


}
