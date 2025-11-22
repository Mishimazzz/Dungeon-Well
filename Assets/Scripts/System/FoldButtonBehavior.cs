using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldButtonBehavior : MonoBehaviour
{
    public GameObject animationPanel;   // 动画板
    public GameObject iconPanel;        // Icon 动画
    // 主板
    public GameObject mainCanvas;
    public GameObject buttons;
    public GameObject UIbuttons;
    public GameObject Cat;


    public Animator animationAnimator;  // 动画板 Animator
    public Animator iconAnimator;       // Icon Animator

    private int state = 0;              // 当前状态
    private bool timerStarted = false;

    // 在计时开始时调用
    public void StartFoldFlow()
    {
        timerStarted = true;
        state = 1;  // 显示动画板（状态1）

        animationPanel.SetActive(true);
        iconPanel.SetActive(false);
        if (mainCanvas != null) mainCanvas.SetActive(true);
        if (buttons != null) buttons.SetActive(true);
        if (UIbuttons != null) UIbuttons.SetActive(true);
        if (Cat != null) Cat.SetActive(false);

        if (animationAnimator != null)
            animationAnimator.SetTrigger("Show");
    }

    public void OnFoldButtonClick()
    {

        if (!timerStarted) return; // 没开始计时时不执行

        state++;
        Debug.Log(state);

        switch (state)
        {
            case 2:
                // 第 1 次按 → 只显示动画板
                animationPanel.SetActive(true);
                iconPanel.SetActive(false);
                if (mainCanvas != null) mainCanvas.SetActive(false);
                if (buttons != null) buttons.SetActive(false);
                if (UIbuttons != null) UIbuttons.SetActive(false);
                if (Cat != null) Cat.SetActive(false);
                break;

            case 3:
                // 第 2 次按 → 只显示 Icon 动画
                animationPanel.SetActive(false);
                iconPanel.SetActive(true);
                if (iconAnimator != null)
                    iconAnimator.SetTrigger("PlayIcon");
                if (mainCanvas != null) mainCanvas.SetActive(false);
                if (buttons != null) buttons.SetActive(false);
                if (UIbuttons != null) UIbuttons.SetActive(false);
                if (Cat != null) Cat.SetActive(false);
                break;

            case 4:
                // 第 3 次按 → 主板 + 动画板
                animationPanel.SetActive(true);
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
}
