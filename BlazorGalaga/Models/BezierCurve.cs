using System;
using System.Drawing;

namespace BlazorGalaga.Models
{
    public class BezierCurve
    {
        public PointF StartPoint { get; set; }
        public PointF ControlPoint1 { get; set; }
        public PointF ControlPoint2 { get; set; }
        public PointF EndPoint { get; set; }
    }

}
