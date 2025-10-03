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
public enum Level { E, D, C, B, A, S }
public enum Seed { No, Yes }

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
    public Seed isSeed;

    // 额外字段（只有 isSeed == Yes 才显示）
    public float firstPhase = 0f;//种土里第1阶段，秒数
    public float secondPhase;//种土里第2阶段，秒数
    public float thirdPhase;//种土里第3阶段（成熟），秒数
    public GameObject firstPhasePrefab;
    public GameObject secondPhasePrefab;
    public GameObject thirdPhasePrefab;
    public ItemData harvestItem;//成熟农作物的itemdata
    public GameObject emptyPrefab;//空白图例，用来调用Draggableitem.cs里头的manager line95
    public SeedManager seedManagerScript;//放seed的脚本的game obj
}
