using System;
using System.Drawing;
using BlazorGalaga.Models;
using static BlazorGalaga.Models.Sprite;

namespace BlazorGalaga.Interfaces
{
    public interface IAnimatable 
    {
        public PointF Location { get; set; }
        public BezierCurve Path { get; set; }
        public bool DrawPath { get; set; }
        public Sprite Sprite { get; set; }
        public bool PathIsLine { get; set; }
    }
}
