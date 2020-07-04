using System;
using System.Drawing;

namespace BlazorGalaga.Static
{
    public static class Constants
    {
        public static Rectangle CanvasSize = new Rectangle(0,0,672, 944);
        public static Rectangle SpriteDestSize = new Rectangle(0, 0, 45, 45);
        public static int ShipMoveSpeed = 1;
        public static string ArrowLeft = "ArrowLeft";
        public static string ArrowRight = "ArrowRight";
    }
}
