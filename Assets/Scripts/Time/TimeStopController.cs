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
        // 停止计时并获取剩余时间
        hourLeft = timeCountDownController.hourLeft;
        minLeft = timeCountDownController.minLeft;
        secLeft = timeCountDownController.secLeft;

        Debug.Log($"hourLeft:{hourLeft}, minLeft:{minLeft}, secLeft:{secLeft}");

        TotalExecuteTime = ComputeExecuteTime();

        // 切 UI
        timeCountDownPanel.SetActive(false);
        timeStopPanel.SetActive(false);
        timeUpPanel.SetActive(true);

        // === 正确计算实际执行时间 ===
        int startSeconds = timeCountDownController.tempHour * 3600
                         + timeCountDownController.tempMin * 60
                         + timeCountDownController.tempSec;

        int leftSeconds = hourLeft * 3600 + minLeft * 60 + secLeft;
        int actualSeconds = Mathf.Max(0, startSeconds - leftSeconds);

        int actualHour = actualSeconds / 3600;
        int actualMin = (actualSeconds % 3600) / 60;
        int actualSec = actualSeconds % 60;

        // 显示
        executeHour.text = actualHour.ToString("D2");
        executeMin.text = actualMin.ToString("D2");
        executeSec.text = actualSec.ToString("D2");

        // 其他逻辑
        itemManager.SpawItem();
        exploreButtonText.text = "Explore";
        timeController.hour = 0;
        timeController.min = 0;
        timeController.sec = 0;
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
        // 起始时间（秒）
        int totalStartSec = timeCountDownController.tempHour * 3600
                          + timeCountDownController.tempMin * 60
                          + timeCountDownController.tempSec;

        // 剩余时间（秒）
        int totalLeftSec = hourLeft * 3600
                         + minLeft * 60
                         + secLeft;

        // 已用时间（秒）
        int usedSec = Mathf.Max(0, totalStartSec - totalLeftSec);

        // 拆分成 h:m:s
        buttonManage.executeHour = usedSec / 3600;
        buttonManage.executeMin = (usedSec % 3600) / 60;
        buttonManage.executeSec = usedSec % 60;

        // 总时间(分钟为单位)
        buttonManage.TotalFullExecuteTime = usedSec / 60f;
        return buttonManage.TotalFullExecuteTime;
    }

}
