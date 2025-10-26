using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.AI;
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

    struct MonitorInfo
    {
        public Vector2Int origin;   // 屏幕左上角虚拟坐标
        public Vector2Int size;     // 分辨率
    }

    MonitorInfo[] monitors = new MonitorInfo[]
    {
    new MonitorInfo { origin = new Vector2Int(-2560, 0), size = new Vector2Int(2560, 1440) }, // 左
    new MonitorInfo { origin = new Vector2Int(0, 0),     size = new Vector2Int(3440, 1440) }, // 主
    new MonitorInfo { origin = new Vector2Int(3440, 0),  size = new Vector2Int(1920, 1080) }  // 右
    };

    int currentMonitor = 1;

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
        MoveToMonitor(currentMonitor);
#endif
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            EnableTransparent(false); // UI上 → 可交互
        else
            EnableTransparent(true);  // 无UI → 穿透

        // 快捷键模拟 “Shift+Win+左右箭头”
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToMonitor(1);
            MoveToMonitor(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentMonitor--;
            if (currentMonitor < 0) currentMonitor = 0;
            MoveToMonitor(currentMonitor);
            MoveToMonitor(currentMonitor);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentMonitor++;
            if (currentMonitor > 2) currentMonitor = 2;
            MoveToMonitor(currentMonitor);
            MoveToMonitor(currentMonitor);
        }
            
    }

    void EnableTransparent(bool enable)
    {
        int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

        if (enable)
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | (int)(WS_EX_LAYERED | WS_EX_TRANSPARENT));
        else
            SetWindowLong(hWnd, GWL_EXSTYLE, (exStyle | (int)WS_EX_LAYERED) & ~(int)WS_EX_TRANSPARENT);

        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
    }

    void MoveToMonitor(int index)
    {
        if (index < 0 || index >= monitors.Length) index = 0;

        var m = monitors[index];
        int sw = m.size.x;
        int sh = m.size.y;

        // 根据每个显示器自己的分辨率计算右下角
        int x = m.origin.x + sw - windowWidth - margin;
        int y = m.origin.y + sh - windowHeight - margin;

        SetWindowPos(hWnd, HWND_TOPMOST, x, y, windowWidth, windowHeight, SWP_SHOWWINDOW);
        Debug.Log($"✅ 窗口已移动到显示器 {index + 1} 右下角 ({x},{y}) 分辨率=({sw},{sh})"); 
    }

}
