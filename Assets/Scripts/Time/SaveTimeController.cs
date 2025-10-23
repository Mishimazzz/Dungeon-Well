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

  public void SaveCurrentTime()
  {
    int hour = TimeController.Instance.hour;
    int min = TimeController.Instance.min;
    hourText.text = hour.ToString("D2");
    minuteText.text = min.ToString("D2");

    string timeString = $"{hour:D2}:{min:D2}";
    Debug.Log("当前时间: " + timeString);

    //找第一个空位
    for (int i = 0; i < saveSlots.Length; i++)
    {
      string currentText = saveSlots[i].text.Trim();

      if (string.IsNullOrEmpty(currentText) || currentText == "00:00")
      {
        saveSlots[i].text = timeString;
        Debug.Log($"存入时间 {timeString} 到槽位 {i}");
        //系统存入
        TimeSaveData timeSaveData = new TimeSaveData();
        timeSaveData.index = i;
        timeSaveData.timeString = timeString;
        saveDataList.Add(timeSaveData);
        return;
      }
    }

    Debug.LogWarning("没有空位可以保存！");

  }

  //刷新（删除以后）
  public void DeleteTime(int index)
  {
    if (index < 0 || index >= saveSlots.Length)
    {
      Debug.LogWarning("无效的索引：" + index);
      return;
    }

    if (saveSlots[index] != null) saveSlots[index].text = "00:00"; // 清空该槽

    // 删除系统存储的数据
    if (saveDataList != null && saveDataList.Count > 0)
    {
      var itemToRemove = saveDataList.Find(x => x.index == index);
      if (itemToRemove != null)
      {
        saveDataList.Remove(itemToRemove);
        Debug.Log($"已从 saveDataList 中删除 index={index} 的存档。");
      }
    }
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
    if (index < 0 || index >= saveSlots.Length)
    {
      Debug.LogWarning($"⚠️ SetTimer：索引 {index} 超出范围 (0~{saveSlots.Length - 1})");
      return;
    }
    string timeString = saveSlots[index].text;

    if(!string.IsNullOrEmpty(timeString))
    {
      string[] parts = timeString.Split(":");
      string hour = parts[0];
      string min = parts[1];

      TimeController.Instance.hourText.text = hour;
      TimeController.Instance.minuteText.text = min;
      TimeController.Instance.hour = int.Parse(hour);
      TimeController.Instance.min = int.Parse(min);

      Debug.Log($"✅ 已从槽 {index} 载入时间 {hour}:{min}");
    }
  }
}
