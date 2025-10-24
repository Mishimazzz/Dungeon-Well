using System;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
  [Header("时间倒计时的Text")]
  public TextMeshProUGUI hourText;
  public TextMeshProUGUI minuteText;
  public TextMeshProUGUI secondText;
  [Header("显示运行时间的Text")]
  public TextMeshProUGUI executedHour;
  public TextMeshProUGUI executedMin;
  public TextMeshProUGUI executedSec;

  void Start()
  {
    TimeManager.Instance.OnTimeChanged += UpdateCountdownUI;
    TimeManager.Instance.OnTimeUp += OnTimeUp;
    UpdateCountdownUI(TimeManager.Instance.currentTime);
  }

  void OnDestroy()
  {
    if (TimeManager.Instance != null)
    {
      TimeManager.Instance.OnTimeChanged -= UpdateCountdownUI;
      TimeManager.Instance.OnTimeChanged -= UpdateExecutedUI;
      TimeManager.Instance.OnTimeUp -= OnTimeUp;
    }
  }

  // 用于更新倒计时 Text
  void UpdateCountdownUI(TimeData timeData)
  {
    UpdateUIText(hourText, minuteText, secondText, timeData);
  }

  // 用于更新执行时间 Text
  public void UpdateExecutedUI(TimeData executed)
  {
    UpdateUIText(executedHour, executedMin, executedSec, executed);
  }

  // 通用函数：给任意三组 Text 和 TimeData 更新
  void UpdateUIText(TextMeshProUGUI h, TextMeshProUGUI m, TextMeshProUGUI s, TimeData time)
  {
    h.text = time.hour.ToString("D2");
    m.text = time.min.ToString("D2");
    s.text = time.sec.ToString("D2");
  }

  void OnTimeUp()
  {
    TimeManager.Instance.CalculatedExecuteTime();  // 计算执行时间
    UpdateExecutedUI(TimeManager.Instance.executedTimeData); // 更新执行时间 Text
  }
}