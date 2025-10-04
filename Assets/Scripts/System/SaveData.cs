using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
  public List<ItemSaveData> bagItems = new List<ItemSaveData>();
  public List<SeedSaveData> plantedSeeds = new List<SeedSaveData>();
}

[System.Serializable]
public class SeedSaveData
{
  public string seedId;         // 种子名字
  public string plantedDate;    // 种下时间 (DateTime.ToBinary().ToString())
  public float growDuration;    // 总生长时间 (秒)
  public float posX, posY, posZ; //位置
}

[System.Serializable]
public class ItemSaveData
{
  public string itemName;
  public int count;
}
