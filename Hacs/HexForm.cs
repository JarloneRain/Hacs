namespace Hacs;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Program;


public partial class HexForm : Form
{

    public const int Length = 600;

    public static readonly Dictionary<HexTriangleEnum, (SolidBrush Brush, PointF[] Points)> TriangleDict = new()
    {
        [HexTriangleEnum.UR] = (new(Color.Yellow), [new(300, 300), new(450, 40), new(600, 300)]),
        [HexTriangleEnum.U_] = (new(Color.Red), [new(300, 300), new(150, 40), new(450, 40)]),
        [HexTriangleEnum.UL] = (new(Color.Magenta), [new(300, 300), new(0, 300), new(150, 40)]),
        [HexTriangleEnum.DL] = (new(Color.Blue), [new(300, 300), new(150, 560), new(0, 300)]),
        [HexTriangleEnum.D_] = (new(Color.Cyan), [new(300, 300), new(450, 560), new(150, 560)]),
        [HexTriangleEnum.DR] = (new(Color.Green), [new(300, 300), new(600, 300), new(450, 560)]),

    };

    HexTriangleEnum curDrawTri = HexTriangleEnum.U_;
    public HexTriangleEnum CurrentDrawingTriangle
    {
        get => curDrawTri;
        set
        {
            if(value == curDrawTri) return;
            curDrawTri = value;
            Refresh();
            Log($"HexForm draw the {curDrawTri}");
        }
    }

    public HexForm()
    {
        InitializeComponent();

        FormBorderStyle = FormBorderStyle.None;
        BackColor = Color.Black;
        TransparencyKey = Color.Black;
        TopMost = true;
        Opacity = 0.25;

        Paint += (_, e) =>
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var curTri = TriangleDict[curDrawTri];
            g.FillPolygon(curTri.Brush, curTri.Points);
        };
    }


    //readonly Timer timer = new() {
    //    Interval = 1000
    //};
    private void HexForm_Load(object sender, EventArgs e)
    {
        //int i = 0;
        //timer.Tick += (_, _) => {
        //    Location = new Point(i * 1000, i * 1000);
        //    i++;
        //    Refresh();
        //};
        //timer.Start();
    }
}
