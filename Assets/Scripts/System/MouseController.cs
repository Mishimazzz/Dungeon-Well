using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    public GameObject blankUI; //鼠标浮在save time的板上的白色框框
    private Camera cam;

    void Start()
    {
        //给savetimer board板块
        cam = Camera.main;
        blankUI.SetActive(false);

        //鼠标皮肤
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Selectable"))
            {
                blankUI.SetActive(true);
                blankUI.transform.position = Input.mousePosition;
            }
            else blankUI.SetActive(false);
        }
        else
        {
            Debug.Log("没有射到任何物体");
            blankUI.SetActive(false);
        }
    }

}
