using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemText;

    void Awake()
    {
        if (itemIcon == null)
            itemIcon = transform.Find("ItemIcon").GetComponent<Image>();

        if (itemText == null)
            itemText = transform.Find("ItemCountText").GetComponent<TextMeshProUGUI>();
    }

    public void SetItem(Sprite icon, int count)
    {
        itemIcon.sprite = icon;
        itemText.text = count.ToString();
    }
}