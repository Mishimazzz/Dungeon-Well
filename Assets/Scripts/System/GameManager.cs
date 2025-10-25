using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;
  public ItemDatabase itemDatabase; // 在 Inspector 里拖你的 ItemDatabase.asset

  void Awake()
  {
    Application.runInBackground = true;
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);

    // 自动生成 SaveSystem
    if (SaveSystem.Instance == null)
    {
      GameObject go = new GameObject("SaveSystem");
      go.AddComponent<SaveSystem>();
      DontDestroyOnLoad(go);
    }
  }

  void Start()
  {
    LoadGame();
    StartCoroutine(AutoSaveLoop());
  }

  IEnumerator AutoSaveLoop()
  {
    var wait = new WaitForSecondsRealtime(300f); // 5分钟
    while (true)
    {
      yield return wait;
      SaveGame();
    }
  }

  void OnApplicationPause(bool pause)
  {
    if (pause) SaveGame();
  }

  void OnApplicationQuit()
  {
    SaveGame();
  }

  public void SaveGame()
  {
    SaveData data = new SaveData();

    // 背包
    foreach (var kv in HarvestItem.Instance.GetPlayerBag())
    {
      // Debug.Log("kv name: "+kv.Key.name);
      data.bagItems.Add(new ItemSaveData { itemName = kv.Key.name, count = kv.Value });
    }

    // 农田
    data.plantedSeeds = FarmManager.Instance.plantedSeeds;
    foreach (var seedData in data.plantedSeeds)
    {
      Debug.Log("seedData: " + seedData.seedId);
    }

    //时间存储槽
    SaveTimeController.Instance.SaveInDataList();
    data.timeSaveList = SaveTimeController.Instance.saveDataList;
    SaveSystem.Instance.SaveGame(data);
  }

  public void LoadGame()
  {
    SaveData data = SaveSystem.Instance.LoadGame();
    //Debug.Log("Loaded bag count: " + data.bagItems.Count);

    // 背包
    HarvestItem.Instance.ClearBag();
    foreach (var it in data.bagItems)
    {
      var itemData = itemDatabase.GetItemByName(it.itemName);
      // Debug.Log(it.itemName);
      if (itemData != null)
        HarvestItem.Instance.AddOneToBag(itemData, it.count);
    }
    HarvestItem.Instance.needRefreshBag = true;

    // 农田
    FarmManager.Instance.plantedSeeds = data.plantedSeeds;
    FarmManager.Instance.RestoreSeeds();

    //时间
    // Debug.Log(SaveTimeController.Instance == null);
    SaveTimeController.Instance.saveDataList = data.timeSaveList;
    // foreach (var timeData in SaveTimeController.Instance.saveDataList)
    // {
    //   Debug.Log("timeData: " + timeData.timeString);
    // }
    SaveTimeController.Instance.TimeRestore();
  }
}
