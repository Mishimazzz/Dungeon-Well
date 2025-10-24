using System;

[Serializable]
public class TimeData
{
  public int hour;
  public int min;
  public int sec;
  public int totalSeconds => hour * 3600 + min * 60 + sec;

  /// <summary>
  /// Converts totalTime that executed to Seconds.
  /// if < 0, then let totalSec = 0;
  /// </summary>
  /// <param name="totalSec">executed time</param>
  public void ChangeToSeconds(int totalSec)
  {
    if (totalSec < 0) totalSec = 0;

    hour = totalSec / 3600;
    min = (totalSec % 3600) / 60;
    sec = totalSec % 60;
  }

  /// <summary>
  /// convert time to string.
  /// </summary>
  public override string ToString()
  {
    return $"{hour:D2}:{min:D2}:{sec:D2}";
  }
}