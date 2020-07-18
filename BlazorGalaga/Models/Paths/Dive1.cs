using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Static;

namespace BlazorGalaganimatable.Models.Paths
{
    public class Dive1
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
                EndPoint = new PointF(ship.Location.X, ship.Location.Y + 50),
                ControlPoint1 = new PointF(animatable.Location.X + 100, Constants.CanvasSize.Height / 2),
                ControlPoint2 = new PointF(0, Constants.CanvasSize.Height / 2),
            };
            var swoopcounterclockwise = new BezierCurve()
            {
                StartPoint = new PointF(ship.Location.X, ship.Location.Y + 50),
                EndPoint = new PointF(ship.Location.X - 100, ship.Location.Y),
                ControlPoint1 = new PointF(ship.Location.X, ship.Location.Y + 100),
                ControlPoint2 = new PointF(ship.Location.X - 100, ship.Location.Y + 100)
            };
            var gohome = new BezierCurve()
            {
                StartPoint = new PointF(ship.Location.X - 100, ship.Location.Y),
                EndPoint = (animatable as Bug).HomePoint,
                ControlPoint1 = new PointF(ship.Location.X - 100, ship.Location.Y - 100),
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
