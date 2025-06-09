using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    public TextMeshProUGUI hourText;
    public TextMeshProUGUI minuteText;
    public TextMeshProUGUI secondText;
    private int hour; int min; int sec;

    public void IncreaseHour()
    {
        hour++;
        if (hour == 25) hour = 0;
        hourText.text = hour.ToString("D2");
    }

    public void DecreaseHour()
    {
        hour--;
        if (hour == -1) hour = 24;
        hourText.text = hour.ToString("D2");
    }

    public void IncreaseMinute()
    {
        min++;
        if (min == 61) min = 0;
        minuteText.text = min.ToString("D2");
    }

    public void DecreaseMinute()
    {
        min--;
        if (min == -1) min = 60;
        minuteText.text = min.ToString("D2");
    }

    public void IncreaseSecond()
    {
        sec++;
        if (sec == 61) sec = 0;
        secondText.text = sec.ToString("D2");
    }

    public void DecreaseSecond()
    {
        sec--;
        if (sec == -1) sec = 60;
        secondText.text = sec.ToString("D2");
    }
}
