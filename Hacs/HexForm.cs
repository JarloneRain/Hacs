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

using static Logger;


public partial class HexForm : Form
{

    public const int Length = 600;

    public static readonly Dictionary<HexTriangleEnum, Color> TriangleColor = new()
    {
        [HexTriangleEnum.DR] = Color.Green,
        [HexTriangleEnum.D_] = Color.Cyan,
        [HexTriangleEnum.DL] = Color.Blue,
        [HexTriangleEnum.UL] = Color.Magenta,
        [HexTriangleEnum.U_] = Color.Red,
        [HexTriangleEnum.UR] = Color.Yellow,
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
            var curTriAngleS = Math.PI * (int)curDrawTri / 3;
            var curTriAngleE = curTriAngleS + Math.PI / 3;
            var radius = Length / 2;
            g.FillPolygon(new SolidBrush(TriangleColor[curDrawTri]), new PointF[]
            {
                new (radius,radius),
                new (radius+radius*(float)Math.Cos(curTriAngleS),radius+radius*(float)Math.Sin(curTriAngleS)),
                new (radius+radius*(float)Math.Cos(curTriAngleE),radius + radius *(float) Math.Sin(curTriAngleE))
            });
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
