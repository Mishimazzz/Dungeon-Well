using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManage : MonoBehaviour
{
    public static ButtonManage Instance;
    public GameObject timePanel;
    public GameObject timeStopPanel;
    public GameObject timeUpPanel;
    public GameObject timeCountDownPanel;
    public GameObject BagPanel;
    public HarvestItem harvestItem;
    public ItemManager itemManager;
    public Button exploreButton;
    public TextMeshProUGUI exploreButtonText;
    public float TotalFullExecuteTime;
    public int executeHour; public int executeMin; public int executeSec;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartButton()
    {
        timePanel.SetActive(false);
        timeCountDownPanel.SetActive(true);
        exploreButtonText.text = "Cancel";
        TimeManager.Instance.isCounting = false;
        TimeManager.Instance.StartCountDown();
    }
    public void ExploreButton()
    {
        string text = exploreButtonText.text;

        // Explore / Cancel 都可以控制面板开关
        if (text.Equals("Explore"))
        {
            bool isActive = timePanel.activeSelf;
            timePanel.SetActive(!isActive);
        }
        else
        {
            bool isActive = timeStopPanel.activeSelf;
            timeStopPanel.SetActive(!isActive);
        }
    }

    public void TimePanelYes()
    {
        TimeManager.Instance.StopCountDown();
        timeCountDownPanel.SetActive(false);
        timeStopPanel.SetActive(false);
        timeUpPanel.SetActive(true);

        TimeManager.Instance.CalculatedExecuteTime();
        FindObjectOfType<TimeUI>().UpdateExecutedUI(TimeManager.Instance.executedTimeData);

        itemManager.SpawItem();
        exploreButtonText.text = "Explore";
        TimeManager.Instance.SetTime(0,0,0);
    }

    public void TimePanelNo()
    {
        timeStopPanel.SetActive(false);
    }

    public void TimeUpButtom()
    {
        //clean all the items in time up panel and save in player's bag
        harvestItem.AddItemsInBag(itemManager.itemDict);
        itemManager.ClearAllItemDisplays();
        timeUpPanel.SetActive(false);
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

    public void SaveTimeButton()
    {
        SaveTimeController.Instance.SaveCurrentTime();
    }
}
