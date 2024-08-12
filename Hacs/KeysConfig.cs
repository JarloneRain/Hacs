namespace Hacs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.ComponentModel;

enum ModifierKeyEnum
{
    None = 0,
    Shift = 1,
    Ctrl = 2,
    Alt = 4
}

static partial class InputsGenerator
{
    [GeneratedRegex(@"[a-zA-Z]|\{\w+\}|\d")]
    private static partial Regex KeysRegex();

    public static INPUT[] ToInputs(this string keys, uint modifiers)
    {
        var modifierKey = Enum.GetValues<ModifierKeyEnum>()
            .Where(mkey => (modifiers & (uint)mkey) != 0)
            .Select(mkey => mkey switch
            {
                ModifierKeyEnum.Shift => Keys.ShiftKey,
                ModifierKeyEnum.Ctrl => Keys.ControlKey,
                ModifierKeyEnum.Alt => Keys.Menu,
                _ => throw new ArgumentException("No such modifier key.")
            });

        var matches = KeysRegex().Matches(keys);
        List<Keys> keySeq = [];
        foreach(Match match in matches)
        {
            keySeq.Add(match.Value switch
            {
                #region letter
                "A" => Keys.A,
                "B" => Keys.B,
                "C" => Keys.C,
                "D" => Keys.D,
                "E" => Keys.E,
                "F" => Keys.F,
                "G" => Keys.G,
                "H" => Keys.H,
                "I" => Keys.I,
                "J" => Keys.J,
                "K" => Keys.K,
                "L" => Keys.L,
                "M" => Keys.M,
                "N" => Keys.N,
                "O" => Keys.O,
                "P" => Keys.P,
                "Q" => Keys.Q,
                "R" => Keys.R,
                "S" => Keys.S,
                "T" => Keys.T,
                "U" => Keys.U,
                "V" => Keys.V,
                "W" => Keys.W,
                "X" => Keys.X,
                "Y" => Keys.Y,
                "Z" => Keys.Z,
                #endregion
                #region number
                "0" => Keys.D0,
                "1" => Keys.D1,
                "2" => Keys.D2,
                "3" => Keys.D3,
                "4" => Keys.D4,
                "5" => Keys.D5,
                "6" => Keys.D6,
                "7" => Keys.D7,
                "8" => Keys.D8,
                "9" => Keys.D9,
                #endregion
                #region numpad
                "{num0}" or "{Num0}" or "{NUM0}" => Keys.NumPad0,
                "{num1}" or "{Num1}" or "{NUM1}" => Keys.NumPad1,
                "{num2}" or "{Num2}" or "{NUM2}" => Keys.NumPad2,
                "{num3}" or "{Num3}" or "{NUM3}" => Keys.NumPad3,
                "{num4}" or "{Num4}" or "{NUM4}" => Keys.NumPad4,
                "{num5}" or "{Num5}" or "{NUM5}" => Keys.NumPad5,
                "{num6}" or "{Num6}" or "{NUM6}" => Keys.NumPad6,
                "{num7}" or "{Num7}" or "{NUM7}" => Keys.NumPad7,
                "{num8}" or "{Num8}" or "{NUM8}" => Keys.NumPad8,
                "{num9}" or "{Num9}" or "{NUM9}" => Keys.NumPad9,
                #endregion
                #region sign
                "[" => Keys.OemOpenBrackets,
                "]" => Keys.OemCloseBrackets,
                "\\" => Keys.OemBackslash,
                ";" => Keys.OemSemicolon,
                "'" => Keys.OemQuotes,
                "," => Keys.Oemcomma,
                "." => Keys.OemPeriod,
                "/" => Keys.OemQuestion,
                "-" => Keys.OemMinus,
                "=" => Keys.Oemplus,
                #endregion
                #region function
                "{F1}" or "{f1}" => Keys.F1,
                "{F2}" or "{f2}" => Keys.F2,
                "{F3}" or "{f3}" => Keys.F3,
                "{F4}" or "{f4}" => Keys.F4,
                "{F5}" or "{f5}" => Keys.F5,
                "{F6}" or "{f6}" => Keys.F6,
                "{F7}" or "{f7}" => Keys.F7,
                "{F8}" or "{f8}" => Keys.F8,
                "{F9}" or "{f9}" => Keys.F9,
                "{F10}" or "{f10}" => Keys.F10,
                "{F11}" or "{f11}" => Keys.F11,
                "{F12}" or "{f12}" => Keys.F12,
                #endregion
                "e" or "{ENTER}" or "{enter}" or "{Enter}" => Keys.Enter,
                "t" or "{TAB}" or "{tab}" or "{Tab}" => Keys.Tab,
                "b" or "{BACKSPACE}" or "{backspace}" or "{Backspace}" or "{BACK}" or "{back}" or "{Back}" => Keys.Back,
                "s" or "{SPACE}" or "{space}" or "{Space}" => Keys.Space,
                "d" or "{DELETE}" or "{delete}" or "{Delete}" => Keys.Delete,
                "i" or "{INSERT}" or "{insert}" or "{Insert}" => Keys.Insert,
                "h" or "{HOME}" or "{home}" or "{Home}" => Keys.Home,
                "{End}" or "{end}" or "{END}" => Keys.End,
                "{PAGEUP}" or "{pageup}" or "{Pageup}" or "{PgUp}" or "{pgup}" or "{Pgup}" => Keys.PageUp,
                "{PAGEDOWN}" or "{pagedown}" or "{Pagedown}" or "{PgDn}" or "{pgdn}" or "{Pgdn}" => Keys.PageDown,
                "{Esc}" or "{ESC}" or "{esc}" => Keys.Escape,
                _ => throw new ArgumentException($"Invalid key:{match.Value}")
            });
        }

        return
        [
            ..modifierKey.Select(mkey => new INPUT
            {
                type =InputMessage. INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)mkey
                    }
                }
            }),
            ..keySeq.SelectMany(key => new INPUT[]
            {
                new ()
                {
                    type = InputMessage.INPUT_KEYBOARD,
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
                    type = InputMessage.INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = (ushort)key,
                            dwFlags = InputMessage.KEYEVENTF_KEYUP
                        }
                    }
                }
            }),
            ..modifierKey.Select(mkey => new INPUT
            {
                type = InputMessage.INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)mkey,
                        dwFlags = InputMessage.KEYEVENTF_KEYUP
                    }
                }
            })
        ];
    }
}

/// <summary>
/// The offset X or Y is relative to the center of the hexagon.
/// </summary>
class KeyConfig
{
    public INPUT[] Inputs = [];
    public void GenInputs(uint modifiers)
    {
        Inputs = Keys.ToInputs(modifiers);
    }
    public string Keys { get; set; } = "";
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }
    public string Description { get; set; } = "";
    [JsonIgnore]
    [Browsable(false)]
    public Point Location => new()
    {
        X = OffsetX + KeysForm.Length / 2 - QuirkyButtons.HexButton.DefaultWidth / 2,
        Y = OffsetY + KeysForm.Length / 2 - QuirkyButtons.HexButton.DefaultHeight / 2
    };
}

class TriangleConfig
{
    public uint ModifierKeys { get; set; }
    public KeyConfig[] KeyConfigs { get; set; } = [];
    [JsonIgnore]
    public string ModifierKeysText => ModifierKeys == 0 ? "None" : string.Join("+",
        Enum.GetValues<ModifierKeyEnum>().Where(mkey => (ModifierKeys & (uint)mkey) != 0));
}

class KeysConfig
{
    public TriangleConfig[] TriangleConfigs = [];
    public TriangleConfig this[HexTriangleEnum hexTriangle] => TriangleConfigs[(int)hexTriangle];
    public KeysConfig()
    {
        if(!File.Exists(KeysConfigPath)) File.WriteAllText(KeysConfigPath, DefaultKeysConfigJson);
        try
        {
            TriangleConfigs = JsonSerializer.Deserialize<TriangleConfig[]>(File.ReadAllText(KeysConfigPath))!;
        }
        catch(Exception)
        {
            MessageBox.Show("Failed to load KeysConfig.json, please check the program or contact the developer.");
        }

        // Generate inputs for each key config.
        foreach(var triangleConfig in TriangleConfigs)
        {
            foreach(var keyConfig in triangleConfig.KeyConfigs)
            {
                keyConfig.GenInputs(triangleConfig.ModifierKeys);
            }
        }
    }

    public void Save()
    {
        File.WriteAllText(KeysConfigPath, JsonSerializer.Serialize(TriangleConfigs));
    }

    const string KeysConfigPath = "KeysConfig.json";
    const string DefaultKeysConfigJson = """
[
    {"ModifierKeys":4,"KeyConfigs":[]},
    {"ModifierKeys":6,"KeyConfigs":[
        {"Keys": "A", "OffsetX": -200, "OffsetY": 50, "Description": "Screenshot by Tencent QQ"}
    ]},
    {"ModifierKeys":2,"KeyConfigs":[
        {"Keys":"A", "OffsetX":100,"OffsetY":250,"Description":"Select all."},
        {"Keys":"KF", "OffsetX":-200, "OffsetY":50, "Description":"Formatting in Visual Studio."},
        {"Keys":"C", "OffsetX":100, "OffsetY":-250,"Description":"Copy."},
        {"Keys":"V", "OffsetX":100, "OffsetY":-150,"Description":"Paste."},
        {"Keys":"X", "OffsetX":100, "OffsetY":-50,"Description":"Cut."},
        {"Keys":"Y", "OffsetX":250, "OffsetY":-200,"Description":"Redo."},
        {"Keys":"Z", "OffsetX":250, "OffsetY":-100,"Description":"Undo."}
    ]},
    {"ModifierKeys":3,"KeyConfigs":[]},
    {"ModifierKeys":1,"KeyConfigs":[]},
    {"ModifierKeys":5,"KeyConfigs":[]}
]
""";
}