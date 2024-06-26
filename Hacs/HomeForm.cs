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
        iconMenuStrip = new ContextMenuStrip(components);
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Home", null, (_, _) => ShowHome()));
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Logs", null, (_, _) => new LogForm().Show()));
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, (_, _) => Application.Exit()));

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
