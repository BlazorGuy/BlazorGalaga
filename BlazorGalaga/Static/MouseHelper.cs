using System;
using System.Drawing;
using BlazorGalaga.Models;
using Microsoft.JSInterop;

namespace BlazorGalaga.Static
{
    public class Pos {
        public float x { get; set; }
        public float y { get; set; }
    }

    public static class MouseHelper
    {
        public static bool MouseIsDown { get; set; }
        public static PointF Position { get; set; }

        [JSInvokable("OnMouseMove")]
        public static void OnMouseMove(Pos position)
        {
            Position = new PointF(position.x, position.y);
            Utils.dOut("MousePos", (int)Position.X + "," + (int)Position.Y);
        }

        [JSInvokable("OnMouseDown")]
        public static void OnMouseDown()
        {
            MouseIsDown = true;
            Utils.dOut("MouseDown", MouseIsDown);
        }

        [JSInvokable("OnMouseUp")]
        public static void OnMouseUp()
        {
            MouseIsDown = false;
            Utils.dOut("MouseDown", MouseIsDown);
        }
    }
}
