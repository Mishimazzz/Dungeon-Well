using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveTimeController : MonoBehaviour
{
  public static SaveTimeController Instance;
  [Header("时间icon")]
  public TextMeshProUGUI hourText;
  public TextMeshProUGUI minuteText;

  [Header("存档列表")]
  public TextMeshProUGUI[] saveSlots = new TextMeshProUGUI[4];

  [Header("按钮")]
  public Button saveButton;

  void Awake()
  {
    Instance = this;
  }

  public void SaveCurrentTime()
  {

    if (TimeController.Instance == null)
    {
      Debug.LogError("❌ 没有找到 TimeController.Instance！");
      return;
    }

    int hour = TimeController.Instance.hour;
    int min = TimeController.Instance.min;
    hourText.text = hour.ToString("D2");
    minuteText.text = min.ToString("D2");

    string timeString = $"{hour:D2}:{min:D2}";
    Debug.Log("当前时间: " + timeString);

    //找第一个空位
    for (int i = 0; i < saveSlots.Length; i++)
    {
      if (saveSlots[i] == null)
      {
        Debug.LogWarning($"saveSlots[{i}] 没有拖入对象！");
        continue;
      }

      string currentText = saveSlots[i].text.Trim();

      if (string.IsNullOrEmpty(currentText) || currentText == "00:00")
      {
        saveSlots[i].text = timeString;
        Debug.Log($"✅ 存入时间 {timeString} 到槽位 {i}");
        return;
      }
    }

    Debug.LogWarning("⚠️ 没有空位可以保存！");

  }

  //刷新（删除以后）

}
