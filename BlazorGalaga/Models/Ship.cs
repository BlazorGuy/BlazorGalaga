using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Ship : IAnimatable
    {
        public PointF Location { get; set; }
        public PointF PevLocation { get; set; }
        public PointF NextLocation { get; set; }
        public List<BezierCurve> Paths { get; set; }
        public bool DrawPath { get; set; }
        public Sprite Sprite { get; set; }
        public bool PathIsLine { get; set; }
        public bool RotateAlongPath { get; set; }

        public Ship() 
        {
            Sprite = new Sprite(Sprite.SpriteTypes.Ship);
        }
    }
}
