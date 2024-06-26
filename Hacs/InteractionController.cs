namespace Hacs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hacs.HexForm;
using static Program;

/// <summary>
/// HexTriangleEnum represents the order of triangles in a hexagon,
/// arranged in a clockwise direction starting from the positive x-axis.
/// </summary>
public enum HexTriangleEnum { DR, D_, DL, UL, U_, UR, }

public class InteractionController
{
    HexForm? hexForm;
    KeysForm? keysForm;
    public bool IsRHolding { get; set; } = false;
    Point rDownPos = new(0, 0);

    public void OnMouseMove(int x, int y)
    {
        if(!IsRHolding) return;

        var radius = Math.Sqrt(Math.Pow(x - rDownPos.X, 2) + Math.Pow(y - rDownPos.Y, 2));
        if(radius < 5) return;

        if(hexForm == null)
        {
            hexForm = new HexForm();
            /// I attempted to set the Location when creating the hexForm,
            /// but the window's location did not match my expectations.
            /// I performed many actions similar to those described below, but all attempts failed.
            /// I suspected that the "Show" method might adjust the location,
            /// but found nothing about this in the documentation.
            /// Currently, the hexForm can only be displayed correctly on the main screen.
            //{ Location = new Point(rDownPos.X - 300, rDownPos.Y - 300) };
            //Log($"Create HexForm at {hexForm.Location}");
            //Graphics g = hexForm.CreateGraphics();
            //Log($"DPI:{g.DpiX},{g.DpiY}");
            //hexForm.Location = new Point(rDownPos.X - 300, rDownPos.Y - 300);
            //Log($"Location:{hexForm.Location}");
            //Log($"new X=X*d.DpiX/96={hexForm.Location.X}*{g.DpiX}/96={hexForm.Location.X * g.DpiX / 96}");
            //Log($"new Y=Y*d.DpiY/96={hexForm.Location.Y}*{g.DpiY}/96={hexForm.Location.Y * g.DpiY / 96}");
            //var p = new Point {
            //    X = (int)(hexForm.Location.X * g.DpiX / 96),
            //    Y = (int)(hexForm.Location.Y * g.DpiY / 96),
            //};
            //Log($"Calculated by DPI {p}");
            //hexForm.Location = p;
            //Log($"Adjust HexForm to {hexForm.Location}");

            hexForm.Show();
            hexForm.Location = new Point(rDownPos.X - 300, rDownPos.Y - 300);
            Log($"Showed HexForm at {hexForm.Location}");
        }

        var angle = Math.Atan2(y - rDownPos.Y, x - rDownPos.X) * (180 / Math.PI);
        angle = (angle + 360) % 360;
        hexForm.CurrentDrawingTriangle = (HexTriangleEnum)(int)(angle / 60);
        Log($"CurrentDrawingTriangle: {hexForm.CurrentDrawingTriangle}");
    }

    public void OnRDown(int x, int y)
    {
        IsRHolding = true;
        rDownPos = new Point(x, y);
        Log($"Record R down:{rDownPos}");
    }

    public void OnRUp(int x, int y)
    {
        IsRHolding = false;
        if(hexForm == null) return;

        hexForm.Close();
        hexForm = null;

        var angle = Math.Atan2(y - rDownPos.Y, x - rDownPos.X) * (180 / Math.PI);
        angle = (angle + 360) % 360;
        keysForm = new KeysForm((HexTriangleEnum)(int)(angle / 60));
        keysForm.Show();
        keysForm.Location = new(x - keysForm.Width / 2, y - keysForm.Height / 2);
    }
}
