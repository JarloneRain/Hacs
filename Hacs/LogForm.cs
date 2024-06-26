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


public partial class LogForm : Form {
    readonly TextBox logBox;
    readonly System.Windows.Forms.Timer timer;
    public LogForm() {
        InitializeComponent();

        logBox = new TextBox {
            Multiline = true,
            ScrollBars = ScrollBars.Both,
            Dock = DockStyle.Fill,
            ReadOnly = true
        };
        Controls.Add(logBox);

        timer = new() {
            Interval = 1000
        };
        timer.Tick += (_, _) => {
            logBox.Text = File.ReadAllText(Logger.LogPath);
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        };
    }

    private void LogForm_Load(object sender, EventArgs e) {
        timer.Start();
    }

}
