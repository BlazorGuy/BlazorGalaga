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
        }

        [JSInvokable("OnMouseDown")]
        public static void OnMouseDown()
        {
            MouseIsDown = true;
        }

        [JSInvokable("OnMouseUp")]
        public static void OnMouseUp()
        {
            MouseIsDown = false;
        }
    }
}
