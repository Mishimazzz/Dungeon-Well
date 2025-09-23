using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
using UnityEngine.AI;

// create new item asset
/*
 物品等级制度（从低到高）： E,D,C,B,A,S
 同意物品稀有度分类：
 - LowLevel (100% - 51%)
 - MidLevel (50% - 21%)
 - HighLevel (20% - 6%)
 - UltraRare (5% - 1%)
*/
public enum Rare { LowLevel, MidLevel, HighLevel, UltraRare }
public enum Level { E,D,C,B,A,S }

[CreateAssetMenu(fileName = "ItemData", menuName = "Item System/Item Data")]
public class ItemData : ScriptableObject
{
    public GameObject prefab;
    // set each item prob range from 0 to 1
    [Range(0f, 1f)]
    public float probability;
    //稀有程度(tag)
    public Rare rareLevel;
    public Level itemLevel;
}
