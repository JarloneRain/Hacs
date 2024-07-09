namespace Hacs;

using System.ComponentModel;

public partial class HomeForm : Form
{
    private readonly Icon hacsIcon;
    public HomeForm()
    {
        InitializeComponent();
        hacsIcon = (Icon)(new ComponentResourceManager(typeof(HomeForm))).GetObject("hacs")!;
        InitNotifyIcon();
#if !DEBUG
        WindowState = FormWindowState.Minimized;
        ShowInTaskbar = false;
#endif
        Icon = hacsIcon;

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
    private NotifyIcon notifyIcon;
    private ContextMenuStrip iconMenuStrip;
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
    private void ShowHome()
    {
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
    }
    public void HomeForm_Load(object? s, EventArgs e) { }
}
