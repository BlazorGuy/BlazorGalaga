using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Static;

namespace BlazorGalaganimatable.Models.Paths
{
    public class BlueBugDive1 : IDive
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
                StartPoint = new PointF(animatable.Location.X + 100, animatable.Location.Y),
                EndPoint = new PointF(cx, Constants.CanvasSize.Height-50),
                ControlPoint1 = new PointF(animatable.Location.X + 100, Constants.CanvasSize.Height / 2),
                ControlPoint2 = new PointF(0, Constants.CanvasSize.Height / 2),
            };
            var swoopcounterclockwise = new BezierCurve()
            {
                StartPoint = new PointF(cx, Constants.CanvasSize.Height - 50),
                EndPoint = new PointF(cx + 250, Constants.CanvasSize.Height - 200),
                ControlPoint1 = new PointF(cx + 50, Constants.CanvasSize.Height + 25),
                ControlPoint2 = new PointF(cx + 250, Constants.CanvasSize.Height-20)
            };
            var gohome = new BezierCurve()
            {
                StartPoint = new PointF(cx + 250, Constants.CanvasSize.Height - 200),
                EndPoint = (animatable as Bug).HomePoint,
                ControlPoint1 = new PointF(cx + 250, Constants.CanvasSize.Height - 300),
                ControlPoint2 = new PointF(Constants.CanvasSize.Width, Constants.CanvasSize.Height / 2)
            };

            paths.Add(rotateclockwise);
            paths.Add(dive);
            paths.Add(swoopcounterclockwise);
            paths.Add(gohome);

            return paths;
        }
    }
}
