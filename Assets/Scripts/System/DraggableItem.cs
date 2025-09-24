using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;          
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 originalPosition;
    private Transform originalParent;

    private List<RectTransform> farmGridAreas = new List<RectTransform>();

    private void Start()
    {
        // 自动收集所有带 "FarmGrid" 标签的格子
        GameObject[] cells = GameObject.FindGameObjectsWithTag("FarmGrid");
        foreach (var cell in cells)
        {
            RectTransform rt = cell.GetComponent<RectTransform>();
            if (rt != null) farmGridAreas.Add(rt);
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        originalPosition = transform.localPosition;
        originalParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        
        bool insideAnyGrid = false;
        foreach (var farmGrid in farmGridAreas)
        {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log("World MousePos: " + worldMousePos);
            // Debug.Log("FarmGrid WorldPos: " + farmGrid.position);
            Debug.Log("FarmGrid WorldPos: " + farmGrid.localPosition);

            if (IsInsideFarmGrid(farmGrid.position, worldMousePos))
            {
                Debug.Log($"放进去了 {farmGrid.name} ");
                rectTransform.localPosition = farmGrid.localPosition;
                insideAnyGrid = true;
                break;
            }
        }

        if (!insideAnyGrid)
        {
            Debug.Log("没放到地里, 回到原位");
            rectTransform.localPosition = originalPosition;
        }
    }

    private bool IsInsideFarmGrid(Vector3 farmGrid, Vector3 screenPos)
    {

        // 取格子宽高（UI单位）
        float gridWidth = 0.5f;
        float gridHeight = 0.5f;

        float rangeWidthRight = farmGrid.x + gridWidth;
        float rangeWidthLeft = farmGrid.x - gridWidth;
        float rangeHeightTop = farmGrid.y + gridHeight;
        float rangeHeightBottom = farmGrid.y - gridHeight;
        // Debug.Log("rangeWidthRight:" + rangeWidthRight);
        // Debug.Log("rangeWidthLeft:" + rangeWidthLeft);
        // Debug.Log("rangeHeightTop:" + rangeHeightTop);
        // Debug.Log("rangeHeightBottom:" + rangeHeightBottom);
        // Debug.Log("screenPos.x:" + screenPos.x);
        // Debug.Log("screenPos.y:" + screenPos.y);

        return rangeWidthLeft <= screenPos.x && screenPos.x <= rangeWidthRight &&
        rangeHeightTop >= screenPos.y && rangeHeightBottom <= screenPos.y;
    }
}
