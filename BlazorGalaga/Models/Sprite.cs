using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System;
using System.Drawing;

namespace BlazorGalaga.Models
{
    public class Sprite
    {
        public enum SpriteTypes
        {
            Ship,
            BlueBug,
            BlueBug_DownFlap,
            RedBug,
            RedBug_DownFlap,
            GreenBug,
            GreenBug_Blue,
            GreenBug_DownFlap,
            GreenBug_Blue_DownFlap,
            ShipMissle,
            BugMissle,
            EnemyExplosion1,
            EnemyExplosion2,
            EnemyExplosion3,
            EnemyExplosion4,
            EnemyExplosion5,
            TractorBeam,
            TractorBeam2,
            TractorBeam3,
            CapturedShip,
            DoubleShip
        }

        public Rectangle? SourceRect { get; set; }
        public Rectangle? DestRect { get; set; }
        public RectangleF SpriteDestRect { get; set; }
        public SpriteTypes SpriteType { get; set; }
        public bool IsInitialized { get; set; }
        public float InitialRotationOffset { get; set; }
        public Canvas2DContext BufferCanvas { get; set; }
        public Canvas2DContext DynamicCanvas { get; set; }
        public Sprite(SpriteTypes spritetype)
        {
            SpriteType = spritetype;
        }
    }
}
