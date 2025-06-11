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

    //test
    public GameObject coinObject;

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

        //basic setting
        int itemCount = 0;
        int rareCount = 0;
        int ultraRareCount = 0;

        if (executeTime <= 1)
        {
            Debug.Log("you reach here");
            GiveCoin(1);
        }
        else if (executeTime <= 5)
        {
            itemCount = 1;
            GiveCoin(2);
        }
        else if (executeTime <= 15)
        {
            itemCount = 3;
            GiveCoin(3);
        }
        else if (executeTime <= 30)
        {
            itemCount = 6;
            rareCount = 1;
            GiveCoin(4);
        }
        else if (executeTime <= 45)
        {
            itemCount = 10;
            rareCount = 2;
            GiveCoin(5);
        }
        else if (executeTime <= 60)
        {
            itemCount = 16;
            rareCount = 2;
            ultraRareCount = 1;
            GiveCoin(5);
        }
        else
        {
            int hourCount = Mathf.FloorToInt(executeTime / 60f);
            itemCount = 16;
            rareCount = 3;
            ultraRareCount = hourCount;
            GiveCoin(5* hourCount);
        }

        SpawnItemsByRarity(Rare.Common, itemCount);
        SpawnItemsByRarity(Rare.Rare, rareCount);
        SpawnItemsByRarity(Rare.UltraRare, ultraRareCount);
    }

    public void SpawnItemsByRarity(Rare rareLevel, int count)
    {
        if (rareLevel == Rare.Common && count > 0)
        {
            while (count > 0)
            {
                SpawnItem(itemCommonList);
                count--;
            }
        }
        else if (rareLevel == Rare.Rare && count > 0)
        {
            while (count > 0)
            {
                SpawnItem(itemRareList);
                count--;
            }
        }
        else if (rareLevel == Rare.UltraRare && count > 0)
        {
            while (count > 0)
            {
                SpawnItem(itemUltraRareList);
                count--;
            }
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

    public void GiveCoin(int amount)
    {
        /*
            -> setting time 1 - 5 minutes, 1 coins
            -> setting time 6 - 15 minutes, 2 coins
            -> setting time 16 - 30 minutes, 3 coins
            -> setting time 31 - 45  minutes, 4 coins
            -> setting time 45 - 60  minutes, 5 coins
            -> after 1 hour, 600 coins x per hour
            *后续可以根据分钟，秒数的变化去改，这里只是写了逻辑,看看生成能不能work
        */
        while (amount >= 0)
        {
            Instantiate(coinObject, transform.position, Quaternion.identity);
            amount--;
        }
    }
}
