namespace Hacs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hacs.HexForm;
using static Logger;

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
            hexForm.Show();
            hexForm.Location = new Point(rDownPos.X - Length / 2, rDownPos.Y - Length / 2);
            Log($"Showed HexForm at {hexForm.Location}");
        }
        else
        {
            var angle = Math.Atan2(y - rDownPos.Y, x - rDownPos.X) * (180 / Math.PI);
            angle = (angle + 360) % 360;
            hexForm.CurrentDrawingTriangle = (HexTriangleEnum)(int)(angle / 60);
        }
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
        keysForm = new KeysForm(hexForm.CurrentDrawingTriangle);
        hexForm.Close();
        hexForm = null;
        keysForm.Show();
        keysForm.Location = new(x - keysForm.Width / 2, y - keysForm.Height / 2);
        Log($"Showed KeysForm at {keysForm.Location}");
        //keysForm.Activate();
    }
}
