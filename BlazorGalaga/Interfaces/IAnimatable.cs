using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Models;
using static BlazorGalaga.Models.Sprite;

namespace BlazorGalaga.Interfaces
{
    public interface IAnimatable 
    {
        public PointF Location { get; set; }
        public PointF PevLocation { get; set; }
        public PointF NextLocation { get; set; }
        public List<BezierCurve> Paths { get; set; }
        public bool RotateAlongPath { get; set; }
        public bool DrawPath { get; set; }
        public Sprite Sprite { get; set; }
        public bool PathIsLine { get; set; }
        public List<PointF> PathPoints { get; set; }
        public int CurPathPointIndex { get; set; }
        public float Rotation { get; set; }
        public int Speed { get; set; }
        public bool LoopBack { get; set; }
        public bool DrawControlLines { get; set; }
        public int StartDelay { get; set; }

    }
}
