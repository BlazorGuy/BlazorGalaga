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
    }
}
