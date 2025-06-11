using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
using UnityEngine.AI;

// create new item asset
public enum Rare { Common, Rare, UltraRare }

[CreateAssetMenu(fileName = "ItemData", menuName = "Item System/Item Data")]
public class ItemData : ScriptableObject
{
    public GameObject prefab;
    // set each item prob range from 0 to 1
    [Range(0f, 1f)]
    public float probability;
    //稀有程度(tag)
    public Rare rareLevel;
}
