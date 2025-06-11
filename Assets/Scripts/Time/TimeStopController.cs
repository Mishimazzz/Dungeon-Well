using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeStopController : MonoBehaviour
{
    public GameObject timeStopPanel;
    public GameObject timeCountDownPanel;
    public TimeController timeController;
    public TimeCountDownController timeCountDownController;
    public TextMeshProUGUI exploreButtonText;

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
        ComputeExecuteTime();

        //whole system action
        timeCountDownPanel.SetActive(false);
        timeStopPanel.SetActive(false);
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
        int executeHour;
        if (timeCountDownController.tempHour > 0) executeHour = Mathf.Abs(timeCountDownController.tempHour - 1 - hourLeft);
        else executeHour = Mathf.Abs(timeCountDownController.tempHour - hourLeft);
        
        if (timeCountDownController.tempMin == 0) timeCountDownController.tempMin = 60;
        if (timeCountDownController.tempSec == 0) timeCountDownController.tempSec = 60;

        int executeMin = Mathf.Abs(timeCountDownController.tempMin -1 - minLeft);
        int executeSec = Mathf.Abs(timeCountDownController.tempSec - secLeft);
        Debug.Log("executeHour:" + executeHour);
        Debug.Log("executeMin:" + executeMin);
        Debug.Log("executeSec:" + executeSec);
        TotalExecuteTime = executeHour*60 + executeMin + executeSec / 60f;
        Debug.Log("TotalExecuteTime:" + TotalExecuteTime);
        return TotalExecuteTime;
    }

}
