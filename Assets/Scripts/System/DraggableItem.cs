using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 originalPosition;
    private Transform originalParent;
    private bool canDrag = false;
    public bool isInSeedBox;
    [SerializeField] private AudioClip plantSound;
    private AudioSource audioSource;

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
        audioSource.clip = plantSound;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        Transform seedBoxPanelTransform = GameObject.Find("SeedBoxPanel")?.transform;

        if (seedBoxPanelTransform != null)
        {
            // 正常情况，检查是否在 SeedBoxPanel 之下
            isInSeedBox = transform.IsChildOf(seedBoxPanelTransform);
        }
        else
        {
            // 如果没找到 SeedBoxPanel，就认为不在里面
            isInSeedBox = false;
        }

        //只有种子物品&在种子仓库里的物品才允许拖动
        canDrag = isInSeedBox;
    }

    void Update()
    {
        //切换场景用
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentSceneName.Equals("FarmScene"))
        {
            // 自动收集所有带 "FarmGrid" 标签的格子
            GameObject[] cells = GameObject.FindGameObjectsWithTag("FarmGrid");
            foreach (var cell in cells)
            {
                RectTransform rt = cell.GetComponent<RectTransform>();
                if (rt != null) farmGridAreas.Add(rt);
            }
        }
    }

  public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag()) return;
        canvasGroup.blocksRaycasts = false;
        originalPosition = transform.localPosition;
        originalParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag()) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanDrag()) return;
        canvasGroup.blocksRaycasts = true;

        bool insideAnyGrid = false;
        foreach (var farmGrid in FarmManager.Instance.farmGridAreas)
        {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log("World MousePos: " + worldMousePos);
            // Debug.Log("FarmGrid WorldPos: " + farmGrid.position);
            // Debug.Log("FarmGrid WorldPos: " + farmGrid.localPosition);

            if (IsInsideFarmGrid(farmGrid.position, worldMousePos))
            {
                // 找到这个物品 + 土地
                ItemDisplay display = GetComponent<ItemDisplay>();
                FarmGridCell cell = farmGrid.GetComponent<FarmGridCell>();

                // 如果土地已经被占用，就会回到原位
                if (cell != null && cell.occupied)
                {
                    Debug.Log("土地已被占用");
                    rectTransform.localPosition = originalPosition;
                    insideAnyGrid = true;
                    break;
                }

                if (display != null && display.itemData != null)
                {
                    ItemData data = display.itemData;

                    // 在 playerBag 里减少数量
                    if (HarvestItem.Instance.HasItem(data))
                    {
                        HarvestItem.Instance.ConsumeItem(data, 1); // 消耗1个
                        GameObject plantedSeed = Instantiate(data.emptyPrefab, farmGrid.position, Quaternion.identity);

                        SeedManager manager = plantedSeed.AddComponent<SeedManager>();
                        manager.Init(data);

                        if (cell != null)
                        {
                            cell.occupied = true;
                            manager.ownerCell = cell;

                            if (FarmManager.Instance != null)
                            {
                                FarmManager.Instance.plantedCells.Add(cell.cellId);
                                audioSource.clip = plantSound;
                                audioSource.Play();
                            }
                        }


                        //功能： 让第三行的植物图层永远在第一第二行上，其他的也一样
                        float[] rowY = { 4.1125f, 3.15f, 2.1875f };//植物种植的y轴
                        float[] rowZ = { 3f, 2f, 1f };  // 第一行=3f，以此类推

                        float y = plantedSeed.transform.position.y;
                        int closestIndex = 0;
                        float minDist = Mathf.Abs(y - rowY[0]);
                        for (int i = 1; i < rowY.Length; i++)
                        {
                            float dist = Mathf.Abs(y - rowY[i]);
                            if (dist < minDist)
                            {
                                minDist = dist;
                                closestIndex = i;
                            }
                        }

                        //设置对应 Z 层
                        Vector3 pos = plantedSeed.transform.position;
                        pos.z = rowZ[closestIndex];
                        plantedSeed.transform.position = pos;

                        // Debug
                        // Debug.Log($"植物Y={y}，最近行Y={rowY[closestIndex]}，设置Z={rowZ[closestIndex]}");

                        // 初始化
                        // SeedManager manager = plantedSeed.AddComponent<SeedManager>();
                        // manager.Init(data);
                        SeedBoxManager.seedBoxManager.needRefreshSeedBox = true;

                        //添加到FarmManager.Instance.plantedSeeds里，需要保存数据用
                        Debug.Log("添加到FarmManager.Instance.plantedSeeds");
                        SeedSaveData saveData = new SeedSaveData();
                        saveData.seedId = data.name;
                        saveData.plantedDate = System.DateTime.Now.ToBinary().ToString();
                        saveData.growDuration = data.thirdPhase;
                        saveData.posX = farmGrid.position.x;
                        saveData.posY = farmGrid.position.y;
                        saveData.posZ = pos.z;
                        Debug.Log("posZ:" + saveData.posZ);
                        FarmManager.Instance.plantedSeeds.Add(saveData);
                    }
                }

                rectTransform.localPosition = originalPosition;
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

    private bool CanDrag()
    {
        return isInSeedBox;
    }
}
