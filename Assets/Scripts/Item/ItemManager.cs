using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDatabase itemCommonList;
    public ItemDatabase itemRareList;
    public ItemDatabase itemUltraRareList;
    public ButtonManage buttonManage;
    public TimeStopController timeStopController;
    private float executeTime;

    public void SpawItem()
    {
        /*
            -> if the setting time <= 1 minutes, only got coins.
            -> setting time 1 - 5 minutes, 1 item + coins
            -> setting time 6 - 15 minutes, 3 item + coins
            -> setting time 16 - 30 minutes, 6 item + coins + 1 rare item
            -> setting time 31 - 45  minutes, 10 item + coins + 2 rare item
            -> setting time 45 - 60  minutes, 16 item + coins + 2 rare item + 1 ultra Rare
            -> after 1 hour, 16 items + coins + 3 rare + 1 ultra rare x per hour
        */

        //get player execute time
        executeTime = GetTime();




    }

    public void SpawnItemsByRarity(Rare rareLevel, int count)
    {
        if (rareLevel == Rare.Common)
        {

        }
        
    }

    public void SpawnItem(ItemDatabase itemDatabase)
    {
        float total = 0f;
        foreach (var item in itemDatabase.Iteams)
            total += item.probability;

        float rand = Random.Range(0f, total);
        float current = 0f;

        foreach (var item in itemDatabase.Iteams)
        {
            current += item.probability;
            if (rand <= current)
            {
                Instantiate(item.prefab, transform.position, Quaternion.identity);
                break;
            }
        }
    }

    public float GetTime()
    {
        float returnTime = 0f;
        if (buttonManage.TotalFullExecuteTime != 0f)
        {
            returnTime = buttonManage.TotalFullExecuteTime;
        }
        else if (timeStopController.TotalExecuteTime != 0f)
        {
            returnTime = timeStopController.TotalExecuteTime;
        }

        return returnTime;
    }
}
