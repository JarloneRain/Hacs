namespace Hacs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



static class KeyboardMessage
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public uint type;
        public InputUnion U;
        public static int Size => Marshal.SizeOf(typeof(INPUT));
    }

    [StructLayout(LayoutKind.Explicit)]
    struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    private const uint INPUT_KEYBOARD = 1;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    public static void SendCombinationKeys(uint modifier, List<Keys> keys)
    {
        var modifierKey = Enum.GetValues<ModifierKeyEnum>()
            .Where(mkey => (modifier & (uint)mkey) != 0)
            .Select(mkey => mkey switch
            {
                ModifierKeyEnum.Shift => Keys.ShiftKey,
                ModifierKeyEnum.Ctrl => Keys.ControlKey,
                ModifierKeyEnum.Alt => Keys.Menu,
                _ => throw new Exception("No such modifier key.")
            });

        INPUT[] inputs = [
            ..modifierKey.Select(mkey => new INPUT
            {
                type = INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)mkey
                    }
                }
            }),
            ..keys.SelectMany(key => new INPUT[]
            {
                new ()
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = (ushort)key
                        }
                    }
                },
                new ()
                {
                        type = INPUT_KEYBOARD,
                        U = new InputUnion
                        {
                            ki = new KEYBDINPUT
                            {
                                wVk = (ushort)key,
                                dwFlags = KEYEVENTF_KEYUP
                            }
                        }
                }
            }),
            ..modifierKey.Select(mkey => new INPUT
            {
                type = INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)mkey,
                        dwFlags = KEYEVENTF_KEYUP
                    }
                }
            })
        ];
        _ = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
    }
}

