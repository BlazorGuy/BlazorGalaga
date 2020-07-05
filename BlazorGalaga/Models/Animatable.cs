using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Animatable : IAnimatable
    {
        public PointF Location { get; set; }
        public PointF PevLocation { get; set; }
        public PointF NextLocation { get; set; }
        public List<BezierCurve> Paths { get; set; }
        public bool DrawPath { get; set; }
        public bool DrawPathPoints { get; set; }
        public Sprite Sprite { get; set; }
        public bool PathIsLine { get; set; }
        public bool RotateAlongPath { get; set; }
        public List<PointF> PathPoints { get; set; }
        public int CurPathPointIndex { get; set; }
        public float Rotation { get; set; }
        public int Speed { get; set; }
        public bool LoopBack { get; set; }

        public Animatable()
        {
            PathPoints = new List<PointF>();
        }

    }
}
