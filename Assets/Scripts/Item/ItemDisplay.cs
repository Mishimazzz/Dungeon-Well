using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemText;
    public ItemData itemData;

    void Awake()
    {
        if (itemIcon == null)
            itemIcon = transform.Find("ItemIcon").GetComponent<Image>();

        if (itemText == null)
            itemText = transform.Find("ItemCountText").GetComponent<TextMeshProUGUI>();
    }

    public void SetItem(Sprite icon, int count, ItemData itemdata)
    {
        itemIcon.sprite = icon;
        itemText.text = count.ToString();
        itemData = itemdata;
    }
}

public class CoinDisplay
{
    public TextMeshProUGUI itemText;
    public ItemData itemData;
    public void SetCoin(int count, ItemData itemdata)
    {
        itemText.text = count.ToString();
        itemData = itemdata;
    }
}