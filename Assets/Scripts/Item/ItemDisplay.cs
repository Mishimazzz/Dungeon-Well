using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemCountText;

    public void Set(ItemData data, int count)
    {
        itemIcon.sprite = data.prefab.GetComponent<SpriteRenderer>()?.sprite;
        itemCountText.text = count.ToString();
    }
}