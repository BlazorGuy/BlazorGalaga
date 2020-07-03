using System;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Bug : IAnimatable
    {
        public PointF Location { get; set; }
        public PointF PevLocation { get; set; }
        public PointF NextLocation { get; set; }
        public BezierCurve Path { get; set; }
        public bool DrawPath { get; set; }
        public Sprite Sprite { get; set; }
        public bool PathIsLine { get; set; }
        public bool RotateAlongPath { get; set; }

        public Bug()
        {
            Sprite = new Sprite(Sprite.SpriteTypes.BlueBug);
        }
    }
}
