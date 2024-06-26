namespace Hacs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

enum ModifierKeyEnum
{
    None = 0,
    Shift = 1,
    Ctrl = 2,
    Alt = 4
}

class KeyConfig
{
    public string Keys { get; set; } = "";
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }
    public string Description { get; set; } = "";
    [JsonIgnore]
    public Point Location => new(OffsetX + KeysForm.Length / 2, OffsetY + KeysForm.Length / 2);
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
        {"Keys": "A", "OffsetX": 0, "OffsetY": 0, "Description": "Screenshot by Tencent QQ"}
    ]},
    {"ModifierKeys":2,"KeyConfigs":[
        {"Keys":"A","OffsetX":0,"OffsetY":200,"Description":"Select all."},
        {"Keys":"KF","OffsetX":-300,"OffsetY":0,"Description":"Formatting in Visual Studio."},
        {"Keys":"C","OffsetX":0,"OffsetY":-300,"Description":"Copy."},
        {"Keys":"V","OffsetX":0,"OffsetY":-200,"Description":"Paste."},
        {"Keys":"X","OffsetX":0,"OffsetY":-100,"Description":"Cut."},
        {"Keys":"Y","OffsetX":150,"OffsetY":-250,"Description":"Redo."},
        {"Keys":"Z","OffsetX":150,"OffsetY":-150,"Description":"Undo."}
    ]},
    {"ModifierKeys":3,"KeyConfigs":[]},
    {"ModifierKeys":1,"KeyConfigs":[]},
    {"ModifierKeys":5,"KeyConfigs":[]}
]
""";
}