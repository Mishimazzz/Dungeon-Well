using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class RareGroup
{
  public List<ItemData> low = new List<ItemData>();
  public List<ItemData> mid = new List<ItemData>();
  public List<ItemData> high = new List<ItemData>();
  public List<ItemData> ultra = new List<ItemData>();
}

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item System/Item Database")]
public class ItemDatabase : ScriptableObject
{
  public static ItemDatabase Instance;
  public RareGroup E;
  public RareGroup D;
  public RareGroup C;
  public RareGroup B;
  public RareGroup A;
  public RareGroup S;

  public List<ItemData> GetItems(Level level, Rare rare)
  {
    RareGroup group = null;

    switch (level)
    {
      case Level.E: group = E; break;
      case Level.D: group = D; break;
      case Level.C: group = C; break;
      case Level.B: group = B; break;
      case Level.A: group = A; break;
      case Level.S: group = S; break;
    }

    if (group == null) return null;

    switch (rare)
    {
      case Rare.LowLevel: return group.low;
      case Rare.MidLevel: return group.mid;
      case Rare.HighLevel: return group.high;
      case Rare.UltraRare: return group.ultra;
    }

    return null;
  }

  public ItemData GetItemByName(string name)
  {
    // 遍历所有稀有度分组
    List<ItemData>[] allGroups =
    {
        E.low, E.mid, E.high, E.ultra,
        D.low, D.mid, D.high, D.ultra,
        C.low, C.mid, C.high, C.ultra,
        B.low, B.mid, B.high, B.ultra,
        A.low, A.mid, A.high, A.ultra,
        S.low, S.mid, S.high, S.ultra
    };

    foreach (var group in allGroups)
    {
      foreach (var item in group)
      {
        if (item != null && item.name == name)
          return item;
      }
    }

    Debug.LogWarning("未找到物品：" + name);
    return null;
  }

}