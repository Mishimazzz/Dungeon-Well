using UnityEngine;

public class DragController : MonoBehaviour
{
    public Transform[] targets;   // 需要一起移动的 Canvas / Root
    public float dragSpeed = 1f;

    Vector3 lastMousePos;
    bool dragging = false;

    void Update()
    {
        // 右键按下
        if (Input.GetMouseButtonDown(1))
        {
            dragging = true;
            lastMousePos = Input.mousePosition;
        }

        // 右键拖动
        if (dragging && Input.GetMouseButton(1))
        {
            Vector3 delta = (Input.mousePosition - lastMousePos) * dragSpeed;
            lastMousePos = Input.mousePosition;

            // 移动所有 Canvas
            foreach (Transform t in targets)
                t.Translate(delta, Space.World);
        }

        // 右键松开
        if (Input.GetMouseButtonUp(1))
        {
            dragging = false;
        }
    }
}
