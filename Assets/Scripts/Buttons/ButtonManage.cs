using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonManage : MonoBehaviour
{
    public GameObject timePanel;
    public GameObject timeStopPanel;
    public GameObject timeUpPanel;
    public GameObject BagPanel;
    public HarvestItem harvestItem;
    public ItemManager itemManager;
    public Button exploreButton;
    public TextMeshProUGUI exploreButtonText;
    public GameObject timeCountDownPanel;
    public float TotalFullExecuteTime;
    public int executeHour; public int executeMin; public int executeSec;

    public TimeCountDownController timeCountDownController;

    public void StartButton()
    {
        // Debug.Log(" Debug: You click the start button");
        timePanel.SetActive(false);
        timeCountDownPanel.SetActive(true);
        exploreButtonText.text = "Cancel";
        timeCountDownController.isCounting = false;
    }
    public void ExploreButton()
    {
        string text = exploreButtonText.text;
        // Debug.Log(" Debug: You click the Explore button");

        if (text.Equals("Explore")) timePanel.SetActive(true);
        else
        {
            timeStopPanel.SetActive(true);
        }
    }

    public void TimeUpButtom()
    {
        //clean all the items in time up panel and save in player's bag
        harvestItem.AddItemsInBag(itemManager.itemDict);
        itemManager.ClearAllItemDisplays();
        timeUpPanel.SetActive(false);
    }

    public float ComputeFullExecuteTime()
    {
        executeHour = timeCountDownController.tempHour;
        executeMin = timeCountDownController.tempMin;
        executeSec = timeCountDownController.tempSec;
        float tempTotalFullExecuteTime = executeHour * 60 + executeMin + executeSec / 60f;
        return tempTotalFullExecuteTime;
    }

    // TODO:如果背包一开始就一直打开，获得的物品ui不会显示
    public void TogglePlayerBag()
    {
        bool isActive = BagPanel.activeSelf;
        BagPanel.SetActive(!isActive);

        if (isActive) // 如果是刚打开
        {
            HarvestItem.Instance.RefreshBagUI();
        }
    }
    
}
