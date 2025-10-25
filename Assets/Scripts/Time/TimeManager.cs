using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class TimeManager : MonoBehaviour 
{
  public static TimeManager Instance { get; private set; }
  public event Action<TimeData> OnTimeChanged; // 时间变化就通知
  public event Action OnTimeUp; // 时间到了通知
  public bool isCounting { get; set; }
  private int startTotal;
  private int total;
  public int executedTime;
  public TimeData currentTime = new TimeData();
  public TimeData executedTimeData = new TimeData();

  private void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Destroy(gameObject);
  }

  /// <summary>
  /// 更改时间，并通知所有观察者，时间被改了
  /// </summary>
  /// <param name="hour"></param>
  /// <param name="min"></param>
  /// <param name="sec"></param>
  public void SetTime(int hour, int min, int sec)
  {
    currentTime.hour = hour;
    currentTime.min = min;
    currentTime.sec = sec;
    OnTimeChanged?.Invoke(currentTime);
  }

  /// <summary>
  /// 开始倒计时，并检查是否:
  /// 1.正在倒计时
  /// 2.设定的总计时是否是小于等于0的
  /// </summary>
  public void StartCountDown()
  {
    if (isCounting || currentTime.totalSeconds <= 0) return;
    startTotal = currentTime.totalSeconds;
    StartCoroutine(CountdownRoutine());
  }

  /// <summary>
  /// 只有倒计时的逻辑。会提醒更改UI + timeUp的观察者们
  /// </summary>
  public IEnumerator CountdownRoutine()
  {
    isCounting = true;
    total = startTotal;

    while (total > 0)
    {
      yield return new WaitForSeconds(1f);
      total--;
      currentTime.ChangeToSeconds(total);
      OnTimeChanged?.Invoke(currentTime);
    }

    ButtonManage.Instance.TimePanelYes();
  }

  /// <summary>
  /// 停止计时
  /// </summary>
  public void StopCountDown()
  {
    if (!isCounting) return;

    StopAllCoroutines();
    isCounting = false;
  }

  /// <summary>
  /// 给hourup调整时间的button用的function
  /// </summary>
  public void IncreaseHour()
  {
    currentTime.hour++;
    if (currentTime.hour == 24) currentTime.hour = 0;
    OnTimeChanged?.Invoke(currentTime);
  }

  /// <summary>
  /// 给hourDown调整时间的button用的function
  /// </summary>
  public void DecreaseHour()
  {
    currentTime.hour--;
    if (currentTime.hour < 0) currentTime.hour = 23;
    OnTimeChanged?.Invoke(currentTime);
  }

  /// <summary>
  /// 给Minup调整时间的button用的function
  /// </summary>
  public void IncreaseMinute()
  {
    currentTime.min++;
    if (currentTime.min == 60) currentTime.min = 0;
    OnTimeChanged?.Invoke(currentTime);
  }

  /// <summary>
  /// 给MinDown调整时间的button用的function
  /// </summary>
  public void DecreaseMinute()
  {
    currentTime.min--;
    if (currentTime.min == -1) currentTime.min = 59;
    OnTimeChanged?.Invoke(currentTime);
  }

  /// <summary>
  /// 算总共运行了多少时间(秒)
  /// </summary>
  public void CalculatedExecuteTime()
  {
    executedTime = startTotal - total;
    executedTimeData.hour = executedTime / 3600;
    executedTimeData.min = (executedTime % 3600) / 60;
    executedTimeData.sec = executedTime % 60;
  }
}