using UnityEngine;

//create item list in Asset
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item System/Item Database")]

public class ItemDatabase : ScriptableObject
{
  public ItemData[] Iteams;
}

