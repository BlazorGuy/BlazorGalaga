using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Models;

namespace BlazorGalaga.Interfaces
{
    public interface IIntro
    {
        public List<BezierCurve> GetPaths();
        public List<PointF> PathPoints { get; set; }
    }
}
