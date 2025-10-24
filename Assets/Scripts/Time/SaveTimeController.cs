using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SaveTimeController : MonoBehaviour
{
  public static SaveTimeController Instance;
  [Header("时间icon")]
  public TextMeshProUGUI hourText;
  public TextMeshProUGUI minuteText;

  [Header("存档列表")]
  public TextMeshProUGUI[] saveSlots = new TextMeshProUGUI[4];
  public List<TimeSaveData> saveDataList = new List<TimeSaveData>();//系统储存时间槽

  [Header("按钮")]
  public Button[] setButtons = new Button[4];
  public Button[] deleteButtons = new Button[4];

  void Awake()
  {
    Instance = this;

    // 注册删除按钮事件和set timer事件
    for (int i = 0; i < deleteButtons.Length; i++)
    {
      int index = i;
      deleteButtons[i].onClick.AddListener(() => DeleteTime(index));
      setButtons[i].onClick.AddListener(() => SetTimer(index));
    }
  }
  private void Start()
  {
    TimeManager.Instance.OnTimeChanged += OnTimeChanged;
  }

  void OnDestroy()
  {
    if (TimeManager.Instance != null)
      TimeManager.Instance.OnTimeChanged -= OnTimeChanged;
  }

  void OnTimeChanged(TimeData data)
  {
    hourText.text = data.hour.ToString("D2");
    minuteText.text = data.min.ToString("D2");
  }

  public void SaveCurrentTime()
  {
    string timeString = $"{TimeManager.Instance.currentTime.hour:D2}:{TimeManager.Instance.currentTime.min:D2}";

    //找第一个空位
    for (int i = 0; i < saveSlots.Length; i++)
    {
      if (string.IsNullOrEmpty(saveSlots[i].text) || saveSlots[i].text == "00:00")
      {
        saveSlots[i].text = timeString;
        // Debug.Log($"存入时间 {timeString} 到槽位 {i}");
        //系统存入
        saveDataList.Add(new TimeSaveData { index = i, timeString = timeString });
        return;
      }
    }
    Debug.LogWarning("没有空位可以保存！");
  }

  //刷新（删除以后）
  public void DeleteTime(int index)
  {
    if (index < 0 || index >= saveSlots.Length) return;
    saveSlots[index].text = "00:00";
    saveDataList.RemoveAll(x => x.index == index);
  }

  //restore the time function
  public void TimeRestore()
  {
    foreach (var timeData in saveDataList)
    {
      int temp_index = timeData.index;
      saveSlots[temp_index].text = timeData.timeString;
    }
  }
  
  //一键放置时间槽的时间到timer里
  public void SetTimer(int index)
  {
    if (index < 0 || index >= saveSlots.Length) return;
    if (string.IsNullOrEmpty(saveSlots[index].text)) return;

    string[] parts = saveSlots[index].text.Split(':');
    int hour = int.Parse(parts[0]);
    int min = int.Parse(parts[1]);

    TimeManager.Instance.SetTime(hour, min, 0);
  }
}
