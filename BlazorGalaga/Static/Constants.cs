using System;
using System.Drawing;

namespace BlazorGalaga.Static
{
    public static class Constants
    {
        public static Rectangle CanvasSize = new Rectangle(0,0,672, 944);
        public static Rectangle SpriteDestSize = new Rectangle(0, 0, 46, 46);
        public static Rectangle BigSpriteDestSize = new Rectangle(0, 0, 90, 90);
        public static Rectangle BiggerSpriteDestSize = new Rectangle(0, 0, 135, 280);
        public static Rectangle BiggerSpriteSourceSize = new Rectangle(0, 0, 48, 80);
        public static int SpriteBufferCount = 25;
        public static int BigSpriteBufferCount = 10;
        public static int BiggerSpriteBufferCount = 3;
        public static int SpriteSourceSize = 16;
        public static int BigSpriteSourceSize = 32;
        public static int ShipMoveSpeed = 5;
        public static int BugDiveSpeed = 5;
        public static int BugIntroSpeed = 10;
        public static int BugIntroSpacing = 70;
        public static int BugRotateIntoPlaceSpeed = 2;
        public static string ArrowLeft = "ArrowLeft";
        public static string ArrowRight = "ArrowRight";
        public static string Space = "Space";
        public static int ShipMissleDelaySpeed = 50;
        public static int ShipMissleSpeed = 14;
        public static int WingFlapInterval = 500;
        public static int EnemyGridLeft = 250;
        public static int EnemyGridHSpacing = 45;
        public static int EnemyMissileSpeed = 6 ;

        public static int Score_BlueBug = 50;
        public static int Score_BlueBugDiving = 100;
        public static int Score_RedBug = 80;
        public static int Score_RedBugDiving = 160;
        public static int Score_GreenBug = 150;
        public static int Score_GreenBugDiving = 400;
        public static int Score_GreenBugDiving1 = 800;
        public static int Score_GreenBugDiving2 = 1600;
        public static int Score_CapturedFighter = 1000;
        public static int Score_ChallengingStageGroup1 = 1000;
        public static int Score_ChallengingStageGroup2 = 1500;
        public static int Score_ChallengingStageGroup3 = 2000;
        public static int Score_TransformBug = 160;
    }
}
