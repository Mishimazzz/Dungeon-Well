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
        // Debug.Log("you touch two times TimeUpButton()");
        // TotalFullExecuteTime = ComputeFullExecuteTime();
        itemManager.ClearAllItemDisplays();
        timeUpPanel.SetActive(false);
    }

    public float ComputeFullExecuteTime()
    {
        executeHour = timeCountDownController.tempHour;
        executeMin = timeCountDownController.tempMin;
        executeSec = timeCountDownController.tempSec;
        Debug.Log("executeHour:" + executeHour);
        Debug.Log("executeMin:" + executeMin);
        Debug.Log("executeSec:" + executeSec);
        float tempTotalFullExecuteTime = executeHour * 60 + executeMin + executeSec / 60f;
        Debug.Log("TotalExecuteTime:" + tempTotalFullExecuteTime);
        return tempTotalFullExecuteTime;
    }
}
