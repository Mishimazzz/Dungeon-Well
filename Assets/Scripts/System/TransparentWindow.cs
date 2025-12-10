using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.AI;
using TMPro;
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

    public int windowWidth = 1920;
    public int windowHeight = 1080;
    public int margin = 0;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    [StructLayout(LayoutKind.Sequential)]
    struct MARGINS { public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight; }

    IntPtr hWnd;

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MONITORINFOEX
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szDevice;
    }

    private delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

    [DllImport("user32.dll")]
    private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

    public struct MonitorInfo
    {
        public Vector2Int origin;
        public Vector2Int size;
        public string name;
    }
    MonitorInfo[] monitors;
    int currentMonitor = 0;

    void Start()
    {
#if !UNITY_EDITOR
    hWnd = GetActiveWindow();

    int style = GetWindowLong(hWnd, GWL_STYLE);
        style &= ~0x00C00000; // WS_CAPTION
        SetWindowLong(hWnd, GWL_STYLE, style);

    var m = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref m);

    var nativeMonitors = GetAllMonitors();
    monitors = new MonitorInfo[nativeMonitors.Length];
    for (int i = 0; i < nativeMonitors.Length; i++)
    {
        monitors[i] = nativeMonitors[i];
        // Debug.Log($"显示器 {i + 1}: 分辨率=({monitors[i].size.x},{monitors[i].size.y}), 原点=({monitors[i].origin.x},{monitors[i].origin.y})");
    }

    // 自动检测主屏（原点 == 0,0）
    currentMonitor = 0;
    for (int i = 0; i < monitors.Length; i++)
    {
        if (monitors[i].origin.x == 0 && monitors[i].origin.y == 0)
        {
            currentMonitor = i;
            // Debug.Log($"检测到主显示器: index={i}");
            break;
        }
    }

    EnableTransparent(false);
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentMonitor--;
            if (currentMonitor < 0) currentMonitor = 0;
            MoveToMonitor(currentMonitor);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentMonitor++;
            if (currentMonitor >= monitors.Length) currentMonitor = monitors.Length - 1;
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
        // Debug.Log($"窗口已移动到显示器 {index + 1} 右下角 ({x},{y}) 分辨率=({sw},{sh})");
        StartCoroutine(RefreshPositionDelayed(x, y));
    }

    public static MonitorInfo[] GetAllMonitors()
    {
        var list = new System.Collections.Generic.List<MonitorInfo>();

        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
            (IntPtr hMonitor, IntPtr hdc, ref RECT rc, IntPtr data) =>
            {
                var info = new MONITORINFOEX();
                info.cbSize = Marshal.SizeOf(info);
                if (GetMonitorInfo(hMonitor, ref info))
                {
                    int width = info.rcMonitor.right - info.rcMonitor.left;
                    int height = info.rcMonitor.bottom - info.rcMonitor.top;

                    var monitor = new MonitorInfo
                    {
                        origin = new Vector2Int(info.rcMonitor.left, info.rcMonitor.top),
                        size = new Vector2Int(width, height),
                        name = info.szDevice
                    };
                    list.Add(monitor);
                }
                return true;
            },
            IntPtr.Zero);

        // ✅ 按 X 坐标从左到右排序
        var array = list.ToArray();
        Array.Sort(array, (a, b) => a.origin.x.CompareTo(b.origin.x));

        return array;
    }

    IEnumerator RefreshPositionDelayed(int x, int y)
    {
        yield return new WaitForSeconds(0.05f);
        SetWindowPos(hWnd, HWND_TOPMOST, x, y, windowWidth, windowHeight, SWP_SHOWWINDOW);
    }

}
