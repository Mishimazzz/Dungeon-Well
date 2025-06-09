using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeStopController : MonoBehaviour
{
    public GameObject timeStopPanel;
    public GameObject timeCountDownPanel;
    public TimeController timeController;
    public TextMeshProUGUI exploreButtonText;
    public void SelectYes()
    {
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

}
