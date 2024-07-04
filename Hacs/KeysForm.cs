namespace Hacs;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using static Program;
using System.Drawing.Drawing2D;

class HexButton : Button
{
    protected override void OnPaint(PaintEventArgs pevent)
    {
        Point[] hexPoints = [
            new (Width/4, 0),
            new ( Width*3/4, 0),
            new ( Width , Height/2),
            new ( Width*3/4, Height ),
            new (Width/4 , Height),
            new (0,  Height/2),
        ];

        GraphicsPath hexPath = new();
        hexPath.AddPolygon(hexPoints);
        Region = new Region(hexPath);

        Graphics g = pevent.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.FillPolygon(new SolidBrush(BackColor), hexPoints);
        g.DrawPolygon(new Pen(ForeColor, 10), hexPoints);

        g.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle, new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        });
    }
}

public partial class KeysForm : Form
{
    public const int Length = 1000;

    readonly HexTriangleEnum hexTriangle;

    uint modifierKeys;
    List<Keys>? keySeq;

    Button[] buttons = [];
    readonly ToolTip toolTip = new();
    public KeysForm(HexTriangleEnum hexTriangle)
    {
        InitializeComponent();

        FormBorderStyle = FormBorderStyle.None;
        BackColor = Color.White;
        TransparencyKey = Color.White;
        TopMost = true;
        Opacity = 0.75;

        this.hexTriangle = hexTriangle;

        InitButtons();
    }

    void InitButtons()
    {
        TriangleConfig triangleConfig = Program.KeysConfig[hexTriangle];
        KeyConfig[] keys = triangleConfig.KeyConfigs;
        Log($"KeysConfig: {JsonSerializer.Serialize(keys)}");
        buttons = new Button[keys.Length];
        for(int i = 0; i < keys.Length; i++)
        {
            var key = keys[i];
            buttons[i] = new HexButton()
            {
                Width = 200,
                Height = 100,
                Text = key.Keys,
                Font = new Font("Arial", 25),
                ForeColor = Color.White,
                BackColor = HexForm.TriangleColor[hexTriangle],
                Location = key.Location
            };
            buttons[i].Click += (_, _) =>
            {
                modifierKeys = triangleConfig.ModifierKeys;
                keySeq = key.KeySeq;

                new Thread(new ThreadStart(() =>
                {
                    // Wait for the window to close and return focus.
                    Thread.Sleep(250);
                    KeyboardMessage.SendCombinationKeys(modifierKeys, keySeq);
                    Log($"Have sent keys: {string.Join(", ", keySeq)}");
                })).Start();

                Close();
                Log($"Close KeysForm");
            };
            toolTip.SetToolTip(buttons[i], key.Description);
            Controls.Add(buttons[i]);
        }
    }

    private void KeysForm_Load(object sender, EventArgs e) { }

}
