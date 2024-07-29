namespace QuirkyButtons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

class RegularPolygonButton : Button
{
    public const int DefaultCircumradius = 100;
    int circumradius = DefaultCircumradius;
    public int Circumradius
    {
        get => circumradius;
        set
        {
            if(value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Circumradius must be postive.");
            }
            circumradius = value;
            Width = 2 * value;
            Height = 2 * value;
            UpdateRegion();
        }
    }
    public int SidesCount { get; private init; }
    double rotate = 0;
    public double Rotate
    {
        get => rotate;
        set
        {
            rotate = value;
            UpdateRegion();
        }
    }

    public Point Center
    {
        get => new(Location.X + Circumradius, Location.Y + Circumradius);
        set => Location = new(value.X - Circumradius, value.Y - Circumradius);
    }
    public float BorderWidth { get; set; } = 0;
    public RegularPolygonButton(int sidesCount) : base()
    {
        SidesCount = sidesCount;
        Width = Circumradius;
        Height = Circumradius;

        UpdateRegion();
    }

    Point[] points = [];

    void UpdateRegion()
    {
        points = Enumerable.Range(0, SidesCount).Select(i =>
        {
            double angle = 2 * Math.PI / SidesCount * i + Rotate;
            return new Point
            {
                X = (int)(Circumradius * Math.Cos(angle) + Circumradius),
                Y = (int)(Circumradius * Math.Sin(angle) + Circumradius),
            };
        }).ToArray();

        GraphicsPath path = new();
        path.AddPolygon(points);
        Region = new Region(path);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.FillPolygon(new SolidBrush(BackColor), points);
        g.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle, new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        });
        if(BorderWidth > 1) g.DrawPolygon(new Pen(ForeColor, BorderWidth), points);
    }
}

class HexButton : Button
{
    public const int DefaultWidth = 200;
    public const int DefaultHeight = 100;
    readonly Point[] hexPoints = [];
    public HexButton() : base()
    {
        Width = DefaultWidth;
        Height = DefaultHeight;
        hexPoints = [
            new (Width/4, 0),
            new (Width * 3/4, 0),
            new (Width, Height/2),
            new (Width * 3/4, Height),
            new (Width/4 , Height),
            new (0, Height/2),
        ];

        GraphicsPath hexPath = new();
        hexPath.AddPolygon(hexPoints);
        Region = new Region(hexPath);
    }
    protected override void OnPaint(PaintEventArgs pevent)
    {
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
