// using UnityEngine;
// using System;
// using System.Collections;
// using System.Runtime.InteropServices;

// public class TransparentDock : MonoBehaviour
// {
//     [DllImport("user32.dll")] static extern IntPtr GetActiveWindow();
//     [DllImport("user32.dll")]
//     static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
//         int X, int Y, int cx, int cy, uint uFlags);
//     [DllImport("user32.dll")] static extern int GetWindowLong(IntPtr hWnd, int nIndex);
//     [DllImport("user32.dll")] static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
//     [DllImport("dwmapi.dll")] static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS m);

//     static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
//     const int GWL_STYLE = -16;
//     const int WS_CAPTION = 0x00C00000, WS_THICKFRAME = 0x00040000, WS_MINIMIZEBOX = 0x00020000,
//               WS_MAXIMIZEBOX = 0x00010000, WS_SYSMENU = 0x00080000;
//     const uint SWP_SHOWWINDOW = 0x0040;

//     [StructLayout(LayoutKind.Sequential)]
//     struct MARGINS { public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight; }

//     public int windowWidth = 1250;
//     public int windowHeight = 800;
//     public int margin = 12;

//     IEnumerator Start()
//     {
//         // 切到可控尺寸的无边框窗口
//         Screen.fullScreenMode = FullScreenMode.Windowed;
//         Screen.SetResolution(windowWidth, windowHeight, false);
//         yield return null; yield return null;

//         ApplyWindowSettings();
//     }

//     void ApplyWindowSettings()
//     {
//         IntPtr h = GetActiveWindow();

//         // 去边框（无标题/无边框）
//         int style = GetWindowLong(h, GWL_STYLE);
//         style &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
//         SetWindowLong(h, GWL_STYLE, style);

//         // 背景透明（DWM）
//         var m = new MARGINS { cxLeftWidth = -1, cxRightWidth = 0, cyTopHeight = 0, cyBottomHeight = 0 };
//         DwmExtendFrameIntoClientArea(h, ref m);

//         // 右下角定位 + 置顶
//         int sw = Screen.currentResolution.width;
//         int sh = Screen.currentResolution.height;
//         int x = sw - windowWidth - margin;
//         int y = sh - windowHeight - margin;
//         SetWindowPos(h, HWND_TOPMOST, x, y, windowWidth, windowHeight, SWP_SHOWWINDOW);

//         // 相机背景设透明
//         var cam = Camera.main;
//         if (cam)
//         {
//             cam.clearFlags = CameraClearFlags.SolidColor;
//             var c = cam.backgroundColor; c.a = 0f;
//             cam.backgroundColor = c;
//         }
//     }

//     // 失去焦点时，强制保持顶层
//     void OnApplicationFocus(bool hasFocus)
//     {
//         if (!hasFocus)
//         {
//             ApplyWindowSettings();
//         }
//     }
// }
