using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestItem : MonoBehaviour
{
  private Dictionary<ItemData, int> playerBag = new Dictionary<ItemData, int>();
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

    // Row 2 (Y = 0)
    new Vector3(-1100, 0, 0),
    new Vector3(-1000, 0, 0),
    new Vector3(-900, 0, 0),
    new Vector3(-800, 0, 0),
    new Vector3(-700, 0, 0),
    new Vector3(-600, 0, 0),
    new Vector3(-500, 0, 0),
    new Vector3(-400, 0, 0),

    // Row 3 (Y = -100)
    new Vector3(-1100, -100, 0),
    new Vector3(-1000, -100, 0),
    new Vector3(-900, -100, 0),
    new Vector3(-800, -100, 0),
    new Vector3(-700, -100, 0),
    new Vector3(-600, -100, 0),
    new Vector3(-500, -100, 0),
    new Vector3(-400, -100, 0)
  };


  //add items in bag, if items it's already there, then change text
  public void AddItemsInBag(Dictionary<ItemData, int> ItemDict)
  {
    foreach (var item in ItemDict)
    {
      playerBag[item.Key] = item.Value;
      Debug.Log("item name in bag:" + item.Key);
      Debug.Log("item value in bag:" + item.Value);
    }
  }


}
