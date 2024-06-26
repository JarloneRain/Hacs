namespace Hacs;

using System.ComponentModel;

public partial class MainForm : Form {
    private readonly Icon hacsIcon;
    //private readonly TextBox tb = new() {
    //    Dock = DockStyle.Fill,
    //    ReadOnly = true
    //};
    public MainForm() {
        InitializeComponent();
        hacsIcon = (Icon)(new ComponentResourceManager(typeof(MainForm))).GetObject("hacs")!;
        InitNotifyIcon();
#if !DEBUG
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
#endif
        Icon = hacsIcon;

        FormClosing += (_, e) => {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
            }
        };

        //Controls.Add(tb);

    }


    #region notify icon
    private NotifyIcon notifyIcon;
    private ContextMenuStrip iconMenuStrip;
    void InitNotifyIcon() {
        iconMenuStrip = new ContextMenuStrip(components);
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Main", null, (_, _) => ShowMain()));
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Logs", null, (_, _) => new LogForm().Show()));
        iconMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, (_, _) => Application.Exit()));

        notifyIcon = new NotifyIcon(components) {
            ContextMenuStrip = iconMenuStrip,
            Icon = hacsIcon,
            Text = "Hacs",
            Visible = true
        };
        notifyIcon.MouseClick += (_, e) => {
            if (e.Button == MouseButtons.Left) {
                ShowMain();
            }
        };
    }
    #endregion
    private void ShowMain() {
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
    }


    //readonly System.Windows.Forms.Timer timer = new() {
    //    Interval = 50
    //};
    public void MainForm_Load(object? s, EventArgs e) {
        //timer.Tick += (_, _) => {
        //    Location = Location with { X = Location.X + 10 };
        //    tb.Text = Location.ToString();
        //};
        //timer.Start();
    }
}
