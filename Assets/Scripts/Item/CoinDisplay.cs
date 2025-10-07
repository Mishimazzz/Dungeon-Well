using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
  public TextMeshProUGUI itemText;
  public ItemData itemData;

  public void SetCoin(int count, ItemData itemdata)
  {
    // Debug.Log($"更新金币数量: {count}");
    if (itemText != null)
      itemText.text = count.ToString();
    itemData = itemdata;
  }
}