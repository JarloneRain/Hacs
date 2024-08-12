namespace Hacs;

using System;
using System.ComponentModel;
using System.Diagnostics;

// This part implement the constructor and notyfy icon.
public partial class HomeForm : Form
{
    const int DefaultClientWidth = 1600;
    const int DefaultClientHeight = 1000;
    public HomeForm()
    {
        InitializeComponent();

        Icon = Resource.hacs;
        ClientSize = new(DefaultClientWidth, DefaultClientHeight);

        InitNotifyIcon();
        InitContainer();
        InitButtons();
        InitDgv();
        InitAddBtn();
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
            Icon = Resource.hacs,
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
    private void ShowHome()
    {
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
    }
    public void HomeForm_Load(object? s, EventArgs e) { }
}

// This part implement the main interface.
public partial class HomeForm : Form
{
    HexTriangleEnum curTri;
    HexTriangleEnum CurTri
    {
        get => curTri;
        set
        {
            curTri = value;
            hsc.Panel1.BackColor = HexForm.TriangleColor[value];
            dgv.DataSource = Program.KeysConfig.TriangleConfigs[(int)value].KeyConfigs;
        }
    }
    #region container
    readonly SplitContainer vsc0 = new()
    {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Horizontal,
        FixedPanel = FixedPanel.Panel1
    }, vsc1 = new()
    {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Horizontal,
        FixedPanel = FixedPanel.Panel2
    }, hsc = new()
    {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Vertical,
        FixedPanel = FixedPanel.Panel1
    };
    void InitContainer()
    {
        Controls.Add(vsc0);
        vsc0.Panel1.Controls.Add(hsc);
        vsc0.Panel2.Controls.Add(vsc1);

        hsc.SplitterDistance = 2 * HexWhellRadius;
        vsc0.SplitterDistance = 2 * HexWhellRadius;
        vsc1.SplitterDistance = ClientSize.Height - 2 * HexWhellRadius - DefaultButtonHeight;
    }

    Panel HexWhellPanel => hsc.Panel1;
    Panel FuncPanel => hsc.Panel2;
    Panel DgvPanel => vsc1.Panel1;
    Panel AddBtnPanel => vsc1.Panel2;
    #endregion

    #region hex whell
    const int HexWhellRadius = 100;
    const int HexWhellTriRadius = (int)(HexWhellRadius * 0.577/*sqrt(3)/2*/);
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
            triButtons[(int)tri].Click += (_, _) => CurTri = tri;
            HexWhellPanel.Controls.Add(triButtons[(int)tri]);
        }
        CurTri = HexTriangleEnum.DL;
    }
    #endregion

    #region buttons
    const int DefaultButtonWidth = 160;
    const int DefaultButtonHeight = HexWhellRadius;
    const int ButtonMargin = 20;
    readonly Dictionary<string, Button> buttons = [];
    readonly string[] buttonsName = ["Help", "Logs"];
    void InitButtons()
    {
        for(int i = 0; i < buttonsName.Length; i++)
        {
            var curName = buttonsName[i];
            var btn = new Button
            {
                Location = new(i * (DefaultButtonWidth + 2 * ButtonMargin) + ButtonMargin, DefaultButtonHeight / 2),
                Text = curName,
                Width = DefaultButtonWidth,
                Height = DefaultButtonHeight
            };
            buttons[curName] = btn;
            FuncPanel.Controls.Add(btn);
        }

        buttons["Help"].Click += (_, _) => Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/JarloneRain/Hacs",
            UseShellExecute = true
        });
        buttons["Logs"].Click += (_, _) => new LogForm().Show();
    }
    #endregion

    #region table
    readonly DataGridView dgv = new()
    {
        ReadOnly = true,
        RowHeadersVisible = false,
        Dock = DockStyle.Fill,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };
    void InitDgv()
    {
        DgvPanel.Controls.Add(dgv);
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
            /* GitHub Copilot told me that "Modify" and "Delete" should be the last two columns，
            ** but in actual testing, they are the 0th and 1st columns.
            */
            var tri = Program.KeysConfig.TriangleConfigs[(int)CurTri];
            // Modify
            if(e.ColumnIndex == 0)
            {
                new ModifyKeyConfigForm(tri.KeyConfigs[e.RowIndex], k =>
                {
                    k.GenInputs(tri.ModifierKeys);
                    tri.KeyConfigs[e.RowIndex] = k;
                    Program.KeysConfig.Save();
                }).ShowDialog();
                dgv.Refresh();
            }
            // Delete
            if(e.ColumnIndex == 1 && MessageBox.Show("Please confirm that you want to delete the shortcut key.", "Reassurance",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                tri.KeyConfigs = tri.KeyConfigs.Where((_, i) => i != e.RowIndex).ToArray();
                Program.KeysConfig.Save();
                // refresh
                CurTri = curTri;
            }
        };
    }
    #endregion


    #region the add button
    const int AddBtnMargin = 50;
    readonly Button addBtn = new()
    {
        Text = "Add",
        Location = new Point(AddBtnMargin, 0),
        Height = DefaultButtonHeight,
        Width = 1600 - 2 * AddBtnMargin
    };
    void InitAddBtn()
    {
        AddBtnPanel.Controls.Add(addBtn);
        addBtn.Click += (_, _) =>
        {
            new AddKeyConfigForm(k =>
            {
                var tri = Program.KeysConfig.TriangleConfigs[(int)CurTri];
                k.GenInputs(tri.ModifierKeys);

                tri.KeyConfigs = [.. tri.KeyConfigs, k];

                Program.KeysConfig.Save();
            }).ShowDialog();
            // refresh
            CurTri = curTri;
        };

        Resize += (_, _) => addBtn.Width = AddBtnPanel.Width - 2 * AddBtnMargin;
    }
    #endregion

}