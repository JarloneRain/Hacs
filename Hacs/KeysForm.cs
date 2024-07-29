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
using System.Drawing.Drawing2D;

using static Logger;

public partial class KeysForm : Form
{
    public const int Length = 1000;

    readonly HexTriangleEnum hexTriangle;
    readonly Button closeButton;
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

        closeButton = new QuirkyButtons.RegularPolygonButton(4)
        {
            Circumradius = 50,
            BorderWidth = 10,
            BackColor = HexForm.TriangleColor[hexTriangle],
            Location = new Point(Length / 2 - 50, Length / 2 - 50)
        };
        closeButton.Click += (_, _) => Close();
        Controls.Add(closeButton);

        InitButtons();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        /* Due to a fxxking bizarre issue, launching KeysForm by listening to fxxking right-click actions
        ** causes the fxxking mouse button to temporarily become unresponsive.
        ** You'll need to fxxking manually press the right mouse button again to restore its functionality.
        ** Fxxking only Microsoft might know why this fxxking thing happens.
        */
        InputMessage.SendInputs([
            new INPUT
            {
                type = InputMessage.INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dwFlags =InputMessage. MOUSEEVENTF_RIGHTDOWN
                    }
                }
            },
            new INPUT
            {
                type = InputMessage.INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dwFlags =InputMessage. MOUSEEVENTF_RIGHTUP
                    }
                }
            }
        ]);
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
            buttons[i] = new QuirkyButtons.HexButton()
            {
                Text = key.Keys,
                Font = new Font("Consolas", 20, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexForm.TriangleColor[hexTriangle],
                Location = key.Location
            };
            buttons[i].Click += (_, _) =>
            {
                new Thread(new ThreadStart(() =>
                {
                    // Wait for the window to close and return focus.
                    Thread.Sleep(250);
                    InputMessage.SendInputs(key.Inputs);
                    Log($"Have sent keys:{triangleConfig.ModifierKeysText}+{key.Keys}");
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
