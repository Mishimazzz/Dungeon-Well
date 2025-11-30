using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;


public class FoldButtonBehavior : MonoBehaviour
{
    public GameObject animationPanel;   // 动画板
    public GameObject iconPanel;        // Icon 动画

    public GameObject mainButton;      // 主按钮
    public GameObject state3Button;    // state3 专用按钮
    // 主板
    public GameObject mainCanvas;
    public GameObject foldButton;
    public GameObject CountTimePanel;
    public GameObject bagPanel;
    public GameObject toDoListPanel;
    public GameObject buttons;
    public GameObject UIbuttons;
    public GameObject Cat;
    public GameObject DragParent;
    //fold button 皮肤
    public Sprite normalSkin;
    public Sprite transparentSkin;

    public Animator animationAnimator;  // 动画板 Animator
    public Animator iconAnimator;       // Icon Animator

    private int state = 0;              // 当前状态
    private bool timerStarted = false;
    public DragPanel dragPanel;
    

    void Update()
    {
        CheckTimeUpState();
    }

  // 在计时开始时调用
  public void StartFoldFlow()
    {
        timerStarted = true;
        state = 1;  // 显示动画板（状态1）
        dragPanel.canDrag = false;

        animationPanel.SetActive(true);
        CountTimePanel.SetActive(true);
        state3Button.SetActive(false);
        RectTransform rt = animationPanel.GetComponent<RectTransform>();
        RectTransform ct = CountTimePanel.GetComponent<RectTransform>();
        RectTransform ft = foldButton.GetComponent<RectTransform>();
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        ct.anchoredPosition = new Vector2(200f, -185f);
        rt.anchoredPosition = new Vector2(200f, 210f);
        ft.anchoredPosition = new Vector2(530f, -65f);
        image.sprite = normalSkin;
        iconPanel.SetActive(false);
        if (mainCanvas != null) mainCanvas.SetActive(true);
        if (buttons != null) buttons.SetActive(true);
        if (UIbuttons != null) UIbuttons.SetActive(true);
        if (Cat != null) Cat.SetActive(false);

        if (animationAnimator != null)
            animationAnimator.SetTrigger("Show");
    }

    // 主按钮点击
    public void OnMainButtonClick()
    {
        Debug.Log("now: " + state);
        if (state == 1) OnFoldButtonClick(2);
        else if (state == 2) OnFoldButtonClick(3);
        else if (state == 3) OnFoldButtonClick(2);
    }

    // state3 按钮点击
    public void OnState3ButtonClick()
    {
        state = 3;
        timerStarted = true;
        OnFoldButtonClick(4);
    }

    public void OnFoldButtonClick(int stateNum)
    {

        if (!timerStarted) return; // 没开始计时时不执行

        state = stateNum;
        Debug.Log(state);
        RectTransform rt = animationPanel.GetComponent<RectTransform>();
        RectTransform ct = CountTimePanel.GetComponent<RectTransform>();
        RectTransform ft = foldButton.GetComponent<RectTransform>();
        Transform dt = DragParent.GetComponent<Transform>();
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();

        switch (state)
        {
            case 2:
                // 第 1 次按 → 只显示动画板
                dragPanel.canDrag = true;
                // 读取玩家拖拽后的记录位置
                dt.transform.position = dragPanel.savedPos;
                animationPanel.SetActive(true);
                CountTimePanel.SetActive(true);
                mainButton.SetActive(true);
                state3Button.SetActive(false);
                rt.anchoredPosition = new Vector2(200f, -228f);
                ct.anchoredPosition = new Vector2(200f, -320);
                ft.anchoredPosition = new Vector2(510f, -170);
                image.sprite = transparentSkin;
                iconPanel.SetActive(false);
                if (mainCanvas != null) mainCanvas.SetActive(false);
                if (buttons != null) buttons.SetActive(false);
                if (bagPanel != null) bagPanel.SetActive(false);
                if (toDoListPanel != null) toDoListPanel.SetActive(false);
                if (UIbuttons != null) UIbuttons.SetActive(false);
                if (Cat != null) Cat.SetActive(false);
                break;

            case 3:
                // 第 2 次按 → 只显示 Icon 动画
                state3Button.SetActive(true);
                mainButton.SetActive(false);
                dragPanel.canDrag = false;
                animationPanel.SetActive(false);
                CountTimePanel.SetActive(false);
                iconPanel.SetActive(true);
                image.sprite = transparentSkin;
                if (iconAnimator != null)
                    iconAnimator.SetTrigger("PlayIcon");
                if (mainCanvas != null) mainCanvas.SetActive(false);
                if (buttons != null) buttons.SetActive(false);
                if (UIbuttons != null) UIbuttons.SetActive(false);
                if (Cat != null) Cat.SetActive(false);
                break;

            case 4:
                // 第 3 次按 → 主板 + 动画板
                dragPanel.canDrag = false;
                animationPanel.SetActive(true);
                CountTimePanel.SetActive(true);
                mainButton.SetActive(true);
                state3Button.SetActive(false);
                rt.anchoredPosition = new Vector2(200f, 210f);
                ct.anchoredPosition = new Vector2(200f, -185f);
                ft.anchoredPosition = new Vector2(530f, -65f);
                dt.position = Vector3.zero;

                image.sprite = normalSkin;
                iconPanel.SetActive(false);
                if (mainCanvas != null) mainCanvas.SetActive(true);
                if (buttons != null) buttons.SetActive(true);
                if (UIbuttons != null) UIbuttons.SetActive(true);
                if (Cat != null) Cat.SetActive(false);

                // 循环：回到状态2（下一次按显示动画板）
                state = 1;
                break;
        }
    }

    private void CheckTimeUpState()
    {
        RectTransform ft = foldButton.GetComponent<RectTransform>();
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (!TimeManager.Instance.isCounting && currentSceneName.Equals("WellScene"))
        {
            animationPanel.SetActive(false);
            iconPanel.SetActive(false);
            ft.anchoredPosition = new Vector2(530f, -65f);
            image.sprite = normalSkin;
            if (mainCanvas != null) mainCanvas.SetActive(true);
            if (buttons != null) buttons.SetActive(true);
            if (UIbuttons != null) UIbuttons.SetActive(true);
            if (Cat != null) Cat.SetActive(true);
            timerStarted = false;
        }
    }
}
