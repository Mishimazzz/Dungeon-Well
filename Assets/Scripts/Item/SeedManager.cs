using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
  public static ItemData seedInstance;
  public float TotalTime;
  public float currentTime = 0f;
  private GameObject currentStageObj;
  public GameObject stage1Prefab;
  public GameObject stage2Prefab;
  public GameObject stage3Prefab;
  private bool initialized = false;
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
        SwitchPrefab(stage3Prefab);
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

    // 不要传 parent，直接放在世界坐标
    currentStageObj = Instantiate(newPrefab, transform.position, Quaternion.identity);
  }
}
