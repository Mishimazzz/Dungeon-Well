using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManage : MonoBehaviour
{
    public GameObject timePanel;
    public GameObject timeStopPanel;
    public GameObject timeUpPanel;
    public GameObject BagPanel;
    public GameObject SeedPanel;
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
    public void TogglePlayerBag()
    {
        bool isActive = BagPanel.activeSelf;
        BagPanel.SetActive(!isActive);

        if (isActive) // 如果是刚打开
        {
            HarvestItem.Instance.RefreshBagUI();
        }
    }

    public void ToggleSeedBox()
    {
        bool isActive = SeedPanel.activeSelf;
        SeedPanel.SetActive(!isActive);

        if (isActive) // 如果是刚打开
        {
            HarvestItem.Instance.RefreshSeedBoxUI();
        }
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
        //unity play 退出
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


}
