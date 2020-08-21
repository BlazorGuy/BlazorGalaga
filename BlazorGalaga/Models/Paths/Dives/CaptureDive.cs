using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models.Paths
{
    public class CaptureDive : IDive
    {
        public List<BezierCurve> GetPaths(IAnimatable animatable, Ship ship)
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            var rotateclockwise = new BezierCurve()
            {
                StartPoint = animatable.Location,
                EndPoint = new PointF(animatable.Location.X + 100, animatable.Location.Y),
                ControlPoint1 = new PointF(animatable.Location.X, animatable.Location.Y - 100),
                ControlPoint2 = new PointF(animatable.Location.X + 100, animatable.Location.Y - 100)
            };
            var dive = new BezierCurve()
            {
                StartPoint = new PointF(animatable.Location.X + 100, animatable.Location.Y),
                EndPoint = new PointF(ship.Location.X, Constants.CanvasSize.Height/2 + 130),
                ControlPoint1 = new PointF(animatable.Location.X + 100, animatable.Location.Y),
                ControlPoint2 = new PointF(ship.Location.X + 90, Constants.CanvasSize.Height / 2),
            };

            paths.Add(rotateclockwise);
            paths.Add(dive);

            return paths;
        }
    }
}
