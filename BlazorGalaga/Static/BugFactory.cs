using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;

namespace BlazorGalaga.Static
{
    public static class BugFactory
    {
        public static List<Animatable> CreateAnimation_BugIntro1()
        {
            List<Animatable> animatables = new List<Animatable>();

            for (int i = 0; i <= 4; i+=5)
            {
                animatables.Add(CreateAnimatable_BugIntro1(i));
            }

            return animatables;
        }

        public static Animatable CreateAnimatable_BugIntro1(int startdelay)
        {
            var w = Constants.CanvasSize.Width;
            var h = Constants.CanvasSize.Height;

            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(w / 2 - 10, -16),
                EndPoint = new PointF(w - (w / 4), h / 2),
                ControlPoint1 = new PointF(w / 2 - 10, h / 4),
                ControlPoint2 = new PointF(w - (w / 4), h / 4),
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(w - (w / 4), h / 2),
                EndPoint = new PointF(w - (w / 4), h / 5),
                ControlPoint1 = new PointF(w - (w / 4), h - 100),
                ControlPoint2 = new PointF(100, h - 100),
            });

            var bug = new Bug()
            {
                Paths = paths,
                DrawPath = false,
                RotateAlongPath = true,
                Speed=10,
                LoopBack = true
            };

            return bug;

        }
    }
}
