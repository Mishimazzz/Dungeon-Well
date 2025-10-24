using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    public GameObject blankUIPrefab; //鼠标浮在save time的板上的白色框框
    public GameObject blankUIInstance;

    private RectTransform targetRect;
    private RectTransform blankRect;

    private Camera cam;

    void Start()
    {
        //给savetimer board板块
        cam = Camera.main;
        blankUIPrefab.SetActive(false);

        //鼠标皮肤
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        targetRect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 如果白框不存在，就实例化一个
        if (blankUIInstance == null)
        {
            blankUIInstance = Instantiate(blankUIPrefab, transform.parent); // 放在同一层级
            blankRect = blankUIInstance.GetComponent<RectTransform>();

            Image img = blankUIInstance.GetComponent<Image>();
            if (img) img.raycastTarget = false;
        }

        // 对齐位置与大小
        blankRect.position = targetRect.position;
        blankRect.sizeDelta = targetRect.sizeDelta + new Vector2(20, 20); // 稍微比按钮大一点
        blankUIInstance.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("鼠标离开！");
        if (blankUIInstance != null) blankUIInstance.SetActive(false);
    }

}
