using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;

namespace BlazorGalaga.Static
{
    public static class BugFactory
    {
        public static List<Animation> CreateAnimation_BugIntro1()
        {
            List<Animation> animations = new List<Animation>();

            for (int i = 0; i <= 4; i+=5)
            {
                animations.Add(CreateAnimatable_BugIntro1(i));
            }

            return animations;
        }

        public static Animation CreateAnimatable_BugIntro1(int startdelay)
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
                StartPercent = 0,
                PercentageOfPath = 50
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(w - (w / 4), h / 2),
                EndPoint = new PointF(w - (w / 4), h / 5),
                ControlPoint1 = new PointF(w - (w / 4), h - 100),
                ControlPoint2 = new PointF(100, h - 100),
                StartPercent = 50,
                PercentageOfPath = 50
            });

            var bug = new Bug()
            {
                Paths = paths,
                DrawPath = true,
                RotateAlongPath = true,
            };

            var bugAnimation = new Animation() {
                Speed = 1,
                LoopBack = false,
                StartDelay = startdelay
            };

            bugAnimation.Animatables.Add(bug);
            return bugAnimation;

        }
    }
}
