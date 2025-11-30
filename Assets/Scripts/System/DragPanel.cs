using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPanel : MonoBehaviour
{
    Vector3 lastPos;
    public bool canDrag = true;
    public Vector2 savedPos;    // 保存位置

    void Update()
    {
        if (!canDrag) return;

        if (Input.GetMouseButtonDown(1))
            lastPos = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastPos;
            lastPos = Input.mousePosition;

            transform.position += new Vector3(delta.x, delta.y, 0) * 0.01f;
        }

        // 松开右键时记录拖拽后的最终位置
        if (Input.GetMouseButtonUp(1))
            savedPos = transform.position;
    }
}
