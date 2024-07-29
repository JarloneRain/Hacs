namespace Hacs;

using System;
using System.ComponentModel;
using System.Diagnostics;

public partial class HomeForm : Form
{
    private readonly Icon hacsIcon;

    public HomeForm()
    {
        InitializeComponent();
        hacsIcon = (Icon)(new ComponentResourceManager(typeof(HomeForm))).GetObject("hacs")!;
        Icon = hacsIcon;
        Width = 800;
        Height = 500;

        InitNotifyIcon();
        InitContainer();
        InitButtons();
        InitDgv();
        InitTriButtons();

#if !DEBUG
        WindowState = FormWindowState.Minimized;
        ShowInTaskbar = false;
#endif

        FormClosing += (_, e) =>
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
            }
        };
    }

    #region notify icon
    /* In the InitNotifyIcon function,
    ** I initialize the following two objects and call the function in the constructor,
    ** However, fxxking Visual Studio fails to recognize this initialization.
    ** To suppress the CS1618 warning in the constructor, performing a dummy construction here.
    */
    private NotifyIcon notifyIcon = new();
    private ContextMenuStrip iconMenuStrip = new();
    void InitNotifyIcon()
    {
        var @switch = new ToolStripMenuItem("Stop", null);
        @switch.Click += (_, _) =>
        {
            if(Program.IsActive)
            {
                Program.IsActive = false;
                @switch.Text = "Open";
                Logger.Log("Stop");
            }
            else
            {
                Program.IsActive = true;
                @switch.Text = "Stop";
                Logger.Log("Open");
            }
        };

        iconMenuStrip = new ContextMenuStrip(components);
        iconMenuStrip.Items.Add(@switch);
        iconMenuStrip.Items.Add(new ToolStripSeparator());
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Home", null, (_, _) => ShowHome()));
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Logs", null, (_, _) => new LogForm().Show()));
        iconMenuStrip.Items.Add(new ToolStripSeparator());
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, (_, _) => Application.Exit()));
        // Set the fxxking font to monospaced
        foreach(ToolStripItem item in iconMenuStrip.Items)
            item.Font = new Font("Consolas", 9, FontStyle.Regular);

        notifyIcon = new NotifyIcon(components)
        {
            ContextMenuStrip = iconMenuStrip,
            Icon = hacsIcon,
            Text = "Hacs",
            Visible = true
        };
        notifyIcon.MouseClick += (_, e) =>
        {
            if(e.Button == MouseButtons.Left)
            {
                ShowHome();
            }
        };
    }
    #endregion

    #region container
    readonly SplitContainer vsc = new()
    {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Horizontal,
        FixedPanel = FixedPanel.Panel1
    }, hsc = new()
    {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Vertical,
        FixedPanel = FixedPanel.Panel1
    };
    void InitContainer()
    {
        vsc.Panel1.Controls.Add(hsc);
        Controls.Add(vsc);

        hsc.SplitterDistance = 2 * HexWhellRadius;
        vsc.SplitterDistance = 2 * HexWhellRadius;
    }
    #endregion

    //HexTriangleEnum curTri = HexTriangleEnum.UR;

    HexTriangleEnum CurTri
    {
        set
        {
            hsc.Panel1.BackColor = HexForm.TriangleColor[value];
            dgv.DataSource = Program.KeysConfig.TriangleConfigs[(int)value].KeyConfigs;
        }
    }
    #region triangle buttons
    const int HexWhellRadius = 100;
    const int HexWhellTriRadius = (int)(HexWhellRadius * 0.577/*sqrt(3)/2*/);
    //const int HexWhellCenterX = 100;
    //const int HexWhellCenterY = 100;
    readonly Button[] triButtons = new Button[6];
    void InitTriButtons()
    {
        foreach(var tri in Enum.GetValues<HexTriangleEnum>())
        {
            triButtons[(int)tri] = new QuirkyButtons.RegularPolygonButton(3)
            {
                Circumradius = HexWhellTriRadius,
                Center = new Point
                {
                    X = HexWhellRadius + (int)(HexWhellTriRadius * Math.Cos((int)tri * Math.PI / 3 + Math.PI / 6)),
                    Y = HexWhellRadius + (int)(HexWhellTriRadius * Math.Sin((int)tri * Math.PI / 3 + Math.PI / 6))
                },
                BackColor = HexForm.TriangleColor[tri],
                ForeColor = Color.White,
                Rotate = Math.PI / 2 + (int)tri * Math.PI / 3,
            };
            triButtons[(int)tri].Click += (_, _) =>
            {
                CurTri = tri;
            };
            hsc.Panel1.Controls.Add(triButtons[(int)tri]);
        }
        CurTri = HexTriangleEnum.DL;
    }
    #endregion
    #region table
    readonly DataGridView dgv = new()
    {
        RowHeadersVisible = false,
        Dock = DockStyle.Fill,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };
    void InitDgv()
    {
        vsc.Panel2.Controls.Add(dgv);
        dgv.DataSource = Program.KeysConfig.TriangleConfigs[(int)HexTriangleEnum.DL].KeyConfigs;

        foreach(var colName in new[] { "Modify", "Delete" })
        {
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = colName,
                UseColumnTextForButtonValue = true
            });
        }

        dgv.CellContentClick += (_, e) =>
        {
            /* Copilot told me that "Modify" and "Delete" should be the last two columns，
            ** but in actual testing, they are the 0th and 1st columns.
            */

            // Modify
            if(e.ColumnIndex == 0)
            {
                MessageBox.Show($"{e.ColumnIndex} {e.RowIndex}");
            }
            // Delete
            if(e.ColumnIndex == 1)
            {
                MessageBox.Show($"{e.ColumnIndex} {e.RowIndex}");
            }
        };
    }
    #endregion

    #region buttons
    readonly Dictionary<string, Button> buttons = [];
    readonly string[] buttonsName = ["Help", "Logs"];
    void InitButtons()
    {
        for(int i = 0; i < buttonsName.Length; i++)
        {
            var curName = buttonsName[i];
            var btn = new Button
            {
                Location = new(200 * i + 20, 50),
                Text = curName,
                Width = 160,
                Height = 100
            };
            buttons[curName] = btn;
            hsc.Panel2.Controls.Add(btn);
        }

        buttons["Help"].Click += (_, _) => Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/JarloneRain/Hacs",
            UseShellExecute = true
        });
        buttons["Logs"].Click += (_, _) => new LogForm().Show();
    }

    #endregion
    private void ShowHome()
    {
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
    }
    public void HomeForm_Load(object? s, EventArgs e) { }
}