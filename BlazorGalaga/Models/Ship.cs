using System;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Ship : IAnimatable
    {
        public PointF Location { get; set; }
        public BezierCurve Path { get; set; }
        public bool DrawPath { get; set; }
        public Sprite Sprite { get; set; }
        public bool PathIsLine { get; set; }

        public Ship() 
        {
            Sprite = new Sprite(Sprite.SpriteTypes.Ship);
        }
    }
}
