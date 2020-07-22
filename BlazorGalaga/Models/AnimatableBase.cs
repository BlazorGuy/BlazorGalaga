using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public abstract class AnimatableBase : IAnimatable, IDisposable
    {
        public int Index { get; set; }
        public PointF Location { get; set; }
        public PointF PevLocation { get; set; }
        public PointF NextLocation { get; set; }
        public List<BezierCurve> Paths { get; set; }
        public bool DrawPath { get; set; }
        public Sprite Sprite { get; set; }
        public bool PathIsLine { get; set; }
        public bool RotateAlongPath { get; set; }
        public List<PointF> PathPoints { get; set; }
        public int CurPathPointIndex { get; set; }
        public float Rotation { get; set; }
        public int Speed { get; set; }
        public bool LoopBack { get; set; }
        public bool DrawControlLines { get; set; }
        public bool Started { get; set; }
        public bool StartDelayStarted { get; set; }
        public bool IsMoving { get; set; }
        public List<Sprite> SpriteBank { get; set; }
        public int? SpriteBankIndex { get; set; }
        public int StartDelay { get; set; }
        public PointF LineFromToLocation { get; set; }
        public PointF LineToLocation { get; set; }
        public int ZIndex { get; set; }
        public int RotatIntoPlaceSpeed { get; set; }
        public int LineToLocationPercent { get; set; }
        public AnimatableBase()
        {
            PathPoints = new List<PointF>();
            Location = new PointF(-5000, -5000);
        }

        public void Dispose()
        {
            PathPoints?.Clear();
            Paths?.Clear();
            SpriteBank?.Clear();

            Paths = null;
            PathPoints = null;
            Sprite = null;
            SpriteBank = null;
        }

    }
}
