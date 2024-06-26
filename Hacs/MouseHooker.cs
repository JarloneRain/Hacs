namespace Hacs;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

internal delegate void MouseEventHandler(MSLLHOOKSTRUCT mouseInfo);

[StructLayout(LayoutKind.Sequential)]
struct MSLLHOOKSTRUCT {
    public POINT pt;
    public int mouseData;
    public int flags;
    public int time;
    public IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
struct POINT {
    public int x;
    public int y;
}
static partial class MouseHooker {

    #region outer
    public static event MouseEventHandler? MouseMove;
    public static event MouseEventHandler? LeftButtonDown;
    public static event MouseEventHandler? LeftButtonUp;
    public static event MouseEventHandler? LeftButtonDoubleClick;
    public static event MouseEventHandler? RightButtonDown;
    public static event MouseEventHandler? RightButtonUp;
    public static event MouseEventHandler? RightButtonDoubleClick;
    public static event MouseEventHandler? MiddleButtonDown;
    public static event MouseEventHandler? MiddleButtonUp;
    public static event MouseEventHandler? MiddleButtonDoubleClick;
    public static event MouseEventHandler? MouseWheel;
    public static event MouseEventHandler? MouseHWheel;
    public static event MouseEventHandler? Unknown;
    #endregion

    private const int WH_MOUSE_LL = 14;
    private static IntPtr hookId = IntPtr.Zero;
    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static LowLevelMouseProc proc = HookCallback;

    static bool isHooking = false;

    public static void Start() {
        if (isHooking || hookId != IntPtr.Zero) return;

        proc = new LowLevelMouseProc(HookCallback);
        hookId = SetWindowsHookEx(WH_MOUSE_LL, proc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
        if (hookId == IntPtr.Zero) {
            Program.Log("Failed to set hook");
            return;
        }
        isHooking = true;

    }

    public static void Stop() {
        if (!isHooking || hookId == IntPtr.Zero) return;

        if (!UnhookWindowsHookEx(hookId)) {
            Program.Log("Failed to unhook");
            return;
        }
        hookId = IntPtr.Zero;
        isHooking = false;
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode >= 0) {
            MSLLHOOKSTRUCT mouseInfo = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
            switch ((MouseMessages)wParam) {
                case MouseMessages.WM_MOUSEMOVE:
                    MouseMove?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_LBUTTONDOWN:
                    LeftButtonDown?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_LBUTTONUP:
                    LeftButtonUp?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_LBUTTONDBLCLK:
                    LeftButtonDoubleClick?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_RBUTTONDOWN:
                    RightButtonDown?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_RBUTTONUP:
                    RightButtonUp?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_RBUTTONDBLCLK:
                    RightButtonDoubleClick?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_MBUTTONDOWN:
                    MiddleButtonDown?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_MBUTTONUP:
                    MiddleButtonUp?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_MBUTTONDBLCLK:
                    MiddleButtonDoubleClick?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_MOUSEWHEEL:
                    MouseWheel?.Invoke(mouseInfo);
                    break;
                case MouseMessages.WM_MOUSEHWHEEL:
                    MouseHWheel?.Invoke(mouseInfo);
                    break;
                default:
                    Program.Log($"Unknown mouse event: wParam={wParam}, lParam={lParam}");
                    break;
            }
        }
        return CallNextHookEx(hookId, nCode, wParam, lParam);
    }


    public enum MouseMessages {
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_MOUSEWHEEL = 0x020A,
        WM_MOUSEHWHEEL = 0x020E,
    }



    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
}

