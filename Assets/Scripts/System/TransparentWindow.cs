using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;

public class TransparentInteractiveWindow : MonoBehaviour
{
    [DllImport("user32.dll")] static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("dwmapi.dll")] static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

    const int GWL_EXSTYLE = -20;
    const int GWL_STYLE = -16;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_NOZORDER = 0x0004;
    const uint SWP_SHOWWINDOW = 0x0040;

    public int windowWidth = 1250;
    public int windowHeight = 800;
    public int margin = 12;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    [StructLayout(LayoutKind.Sequential)]
    struct MARGINS { public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight; }

    IntPtr hWnd;

    void Start()
    {
#if !UNITY_EDITOR
        hWnd = GetActiveWindow();

        // 去掉边框
        int style = GetWindowLong(hWnd, GWL_STYLE);
        style &= ~0x00C00000; // WS_CAPTION
        SetWindowLong(hWnd, GWL_STYLE, style);

        // 背景透明
        var m = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref m);

        // 初始状态：可交互但透明
        EnableTransparent(false);

        // 右下角 + 置顶
        int sw = Screen.currentResolution.width;
        int sh = Screen.currentResolution.height;
        int x = sw - windowWidth - margin;
        int y = sh - windowHeight - margin;
        SetWindowPos(hWnd, HWND_TOPMOST, x, y, windowWidth, windowHeight, SWP_SHOWWINDOW);
#endif
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            EnableTransparent(false); // UI上 → 可交互
        else
            EnableTransparent(true);  // 无UI → 穿透
    }

    void EnableTransparent(bool enable)
    {
        int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

        if (enable)
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | (int)(WS_EX_LAYERED | WS_EX_TRANSPARENT));
        else
            SetWindowLong(hWnd, GWL_EXSTYLE, (exStyle | (int)WS_EX_LAYERED) & ~(int)WS_EX_TRANSPARENT);

        // ✅ 关键：修改样式后重新置顶，防止失焦或掉层
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
    }
}
