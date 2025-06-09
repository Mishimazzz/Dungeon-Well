using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCountDownController : MonoBehaviour
{
    public TimeController timeController;
    public GameObject timeCountDownPanel;
    public TextMeshProUGUI exploreButtonText;
    public TextMeshProUGUI hourText;
    public TextMeshProUGUI minuteText;
    public TextMeshProUGUI secondText;

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
        timeController.RefreshTimeUI();
        int hour = timeController.hour;
        int min = timeController.min;
        int sec = timeController.sec;

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

            hourText.text = hour.ToString("D2");
            minuteText.text = min.ToString("D2");
            secondText.text = sec.ToString("D2");
        }

        Debug.Log(" Times Up!!!");
        timeCountDownPanel.SetActive(false);
        exploreButtonText.text = "Explore";
        timeController.hour = 0; timeController.min = 0; timeController.sec = 0;
        RefreshTimeUI();
    }

    public void RefreshTimeUI()
    {
        timeController.hourText.text = timeController.hour.ToString("D2");
        timeController.minuteText.text = timeController.min.ToString("D2");
        timeController.secondText.text = timeController.sec.ToString("D2");
    }
}
