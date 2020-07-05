using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Animation
    {

        public float Percent { get; set; }
        public float Speed { get; set; }
        public List<IAnimatable> Animatables { get; set; }
        public bool LoopBack { get; set; }
        public float StartDelay { get; set; }
        public List<PointF> PathPoints { get; set; }

        public Animation()
        {
            Speed = 1;
            Percent = 0;
            Animatables = new List<IAnimatable>();
            PathPoints = new List<PointF>();
        }
    }
}
