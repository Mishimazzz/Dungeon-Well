using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeStopController : MonoBehaviour
{
    public GameObject timeStopPanel;
    public GameObject timeCountDownPanel;
    public GameObject timeUpPanel;
    public TimeController timeController;
    public TimeCountDownController timeCountDownController;
    public TextMeshProUGUI exploreButtonText;
    public ButtonManage buttonManage;
    public ItemManager itemManager;
    public TextMeshProUGUI executeHour; public TextMeshProUGUI executeMin; public TextMeshProUGUI executeSec;

    // hour, min, sec that left
    public int hourLeft; public int minLeft; public int secLeft;
    public float TotalExecuteTime;
    public void SelectYes()
    {
        //stop the time, and find hourLeft,min,sec
        hourLeft = timeCountDownController.hourLeft; minLeft = timeCountDownController.minLeft; secLeft = timeCountDownController.secLeft;
        Debug.Log("hourLeft:" + hourLeft);
        Debug.Log("minLeft:" + minLeft);
        Debug.Log("secLeft:" + secLeft);
        TotalExecuteTime = ComputeExecuteTime();

        //whole system action
        timeCountDownPanel.SetActive(false);
        timeStopPanel.SetActive(false);
        timeUpPanel.SetActive(true);

        buttonManage.TotalFullExecuteTime = buttonManage.ComputeFullExecuteTime();
        int actualHour = buttonManage.executeHour - hourLeft;
        int actualMin = buttonManage.executeMin - minLeft;
        int actualSec = buttonManage.executeSec - secLeft;

        executeHour.text = actualHour.ToString("D2");
        executeMin.text = actualMin.ToString("D2");
        executeSec.text = actualSec.ToString("D2");
        itemManager.SpawItem();

        exploreButtonText.text = "Explore";
        timeController.hour = 0; timeController.min = 0; timeController.sec = 0;
        RefreshTimeUI();
    }

    public void SelectNo()
    {
        timeStopPanel.SetActive(false);
    }

    public void RefreshTimeUI()
    {
        timeController.hourText.text = timeController.hour.ToString("D2");
        timeController.minuteText.text = timeController.min.ToString("D2");
        timeController.secondText.text = timeController.sec.ToString("D2");
    }

    public float ComputeExecuteTime()
    {
        // 起始时间
        int startHour = timeCountDownController.tempHour;
        int startMin = timeCountDownController.tempMin;
        int startSec = timeCountDownController.tempSec;

        // 剩余时间
        int leftHour = hourLeft;
        int leftMin = minLeft;
        int leftSec = secLeft;

        // 转为总秒数
        int totalStartSec = startHour * 3600 + startMin * 60 + startSec;
        int totalLeftSec = leftHour * 3600 + leftMin * 60 + leftSec;

        // 实际消耗的秒数
        int usedSec = totalStartSec - totalLeftSec;
        if (usedSec < 0) usedSec = 0;

        // 转回时分秒
        int executeHour = usedSec / 3600;
        int executeMin = (usedSec % 3600) / 60;
        int executeSec = usedSec % 60;

        buttonManage.executeHour = executeHour;
        buttonManage.executeMin = executeMin;
        buttonManage.executeSec = executeSec;

        buttonManage.TotalFullExecuteTime = executeHour * 60 + executeMin + executeSec / 60f;
        return buttonManage.TotalFullExecuteTime;
    }

}
