using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;

namespace BlazorGalaga.Static
{
    public static class BugFactory
    {
        public static List<IAnimatable> CreateAnimation_BugIntro1()
        {
            List<IAnimatable> animatables = new List<IAnimatable>();

            for (int i = 0; i < 1; i++)
            {
                animatables.Add(CreateAnimatable_BugIntro1(i*60));
            }

            return animatables;
        }

        public static IAnimatable CreateAnimatable_BugIntro1(int startdelay)
        {
            var w = Constants.CanvasSize.Width;
            var h = Constants.CanvasSize.Height;

            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(10, 10),
                EndPoint = new PointF(w - (w / 4), h / 2),
                ControlPoint1 = new PointF(w / 2 - 10, h / 4),
                ControlPoint2 = new PointF(w - (w / 4), h / 4),
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(w - (w / 4), h / 2),
                EndPoint = new PointF(502,188),
                ControlPoint1 = new PointF(w - (w / 4), h - 100),
                ControlPoint2 = new PointF(100, h - 100),
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(w - (w / 4), h / 2),
                EndPoint = new PointF(100, 188),
                ControlPoint1 = new PointF(w - (w / 4), h - 100),
                ControlPoint2 = new PointF(100, h - 100),
            });

            var bug = new Bug()
            {
                Paths = paths,
                //DrawPath = true,
                //DrawControlLines = true,
                RotateAlongPath = true,
                Speed=10,
                LoopBack = true,
                StartDelay = startdelay
            };

            return bug;

        }
    }
}
