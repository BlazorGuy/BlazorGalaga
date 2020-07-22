using System;
using System.Drawing;

namespace BlazorGalaga.Static
{
    public static class Constants
    {
        public static Rectangle CanvasSize = new Rectangle(0,0,672, 944);
        public static Rectangle SpriteDestSize = new Rectangle(0, 0, 45, 45);
        public static int SpriteBufferCount = 25;
        public static int SpriteSourceSize = 16;
        public static int ShipMoveSpeed = 5;
        public static int BugIntroSpeed = 10;
        public static int BugIntroSpacing = 70;
        public static int BugRotateIntoPlaceSpeed = 2;
        public static string ArrowLeft = "ArrowLeft";
        public static string ArrowRight = "ArrowRight";
        public static string Space = "Space";
        public static int ShipMissleDelaySpeed = 200;
        public static int WingFlapInterval = 500;
    }
}
