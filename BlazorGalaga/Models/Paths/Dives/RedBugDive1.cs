using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models.Paths
{
    public class RedBugDive1 : IDive
    {
        public List<BezierCurve> GetPaths(IAnimatable animatable, Ship ship)
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            var cx = Constants.CanvasSize.Width / 2;

            var rotateclockwise = new BezierCurve()
            {
                StartPoint = animatable.Location,
                EndPoint = new PointF(animatable.Location.X + 100, animatable.Location.Y),
                ControlPoint1 = new PointF(animatable.Location.X, animatable.Location.Y - 100),
                ControlPoint2 = new PointF(animatable.Location.X + 100, animatable.Location.Y - 100)
            };
            var dive = new BezierCurve()
            {
                StartPoint = new PointF(animatable.Location.X + 100, animatable.Location.Y+1),
                EndPoint = new PointF(ship.Location.X + 90, Constants.CanvasSize.Height + 20),
                ControlPoint1 = new PointF(animatable.Location.X + 100, Constants.CanvasSize.Height / 2),
                ControlPoint2 = new PointF(0, Constants.CanvasSize.Height / 2),
            };
            var gohome = new BezierCurve()
            {
                BreakPath = true,
                StartPoint = new PointF(cx + 100, -50),
                EndPoint = new PointF(cx, 150),
                ControlPoint1 = new PointF(cx + 150, 150),
                ControlPoint2 = new PointF(cx, 0),
            };

            paths.Add(rotateclockwise);
            paths.Add(dive);
            paths.Add(gohome);

            return paths;
        }
    }
}
