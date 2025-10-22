using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCountDownController : MonoBehaviour
{
    public GameObject timeCountDownPanel;
    public GameObject timeUpPanel;
    public TextMeshProUGUI exploreButtonText;
    public TextMeshProUGUI hourText;
    public TextMeshProUGUI minuteText;
    public TextMeshProUGUI secondText;
    public TextMeshProUGUI executeHour; public TextMeshProUGUI executeMin; public TextMeshProUGUI executeSec;
    public ButtonManage buttonManage;
    public ItemManager itemManager;

    //keep original set time
    public int tempHour; public int tempMin; public int tempSec;
    //time left
    public int hourLeft; public int minLeft; public int secLeft;
    public bool isCounting;

    void Update()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        if (!isCounting)
        {
            StartCoroutine(CountDown());
            isCounting = true;
        }
    }

    private IEnumerator CountDown()
    {
        TimeController.Instance.RefreshTimeUI();
        int hour = TimeController.Instance.hour;
        int min = TimeController.Instance.min;
        int sec = TimeController.Instance.sec;
        // copy setting time
        tempHour = hour;
        tempMin = min;
        tempSec = sec;

        // show time at first
        hourText.text = hour.ToString("D2");
        minuteText.text = min.ToString("D2");
        secondText.text = sec.ToString("D2");

        while (hour > 0 || min > 0 || sec > 0)
        {
            //wait for one second
            yield return new WaitForSeconds(1f);

            sec--;
            if (sec < 0 && (min > 0 || hour > 0))
            {
                sec = 59;
                min--;
                if (min < 0 && hour > 0)
                {
                    min = 59;
                    hour--;
                }
            }
            hourLeft = hour;minLeft = min;secLeft = sec;

            hourText.text = hour.ToString("D2");
            minuteText.text = min.ToString("D2");
            secondText.text = sec.ToString("D2");
        }

        //when time is finish
        timeCountDownPanel.SetActive(false);
        timeUpPanel.SetActive(true);
        exploreButtonText.text = "Explore";
        TimeController.Instance.hour = 0; TimeController.Instance.min = 0; TimeController.Instance.sec = 0;
        RefreshTimeUI();
        // WindowsToastNotifier.ShowToast("探索完成", "探索时间结束！可以领取奖励了。");

        buttonManage.TotalFullExecuteTime = buttonManage.ComputeFullExecuteTime();
        executeHour.text = buttonManage.executeHour.ToString("D2");
        executeMin.text = buttonManage.executeMin.ToString("D2");
        executeSec.text = buttonManage.executeSec.ToString("D2");
        itemManager.SpawItem();
    }

    public void RefreshTimeUI()
    {
        TimeController.Instance.hourText.text = TimeController.Instance.hour.ToString("D2");
        TimeController.Instance.minuteText.text = TimeController.Instance.min.ToString("D2");
        TimeController.Instance.secondText.text = TimeController.Instance.sec.ToString("D2");
    }
}
