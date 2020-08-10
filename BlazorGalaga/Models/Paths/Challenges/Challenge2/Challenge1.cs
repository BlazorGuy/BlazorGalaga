using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge2
{
    public class Challenge1 : IntroBase
    {
        public Challenge1()
        {
            IsChallenge = true;
            IntroLocation = IntroLocation.Top;
        }

        public override List<BezierCurve> GetPaths()
        {
            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(286.6868F,0F),
                ControlPoint1 = new PointF(285.7817F,159.2943F),
                ControlPoint2 = new PointF(281.2563F,409.0968F),
                EndPoint = new PointF(324.7005F,497.7948F)},

                new BezierCurve() {StartPoint = new PointF(323.7954F,495.9846F),
                ControlPoint1 = new PointF(321.0802F,388.28F),
                ControlPoint2 = new PointF(249.5782F,38.01342F),
                EndPoint = new PointF(244.1476F,-1000F)},

            };

            return paths;
        }
    }
}
