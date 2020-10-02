using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge2
{
    public class Challenge5 : IntroBase
    {
        public Challenge5()
        {
            IsChallenge = true;
            IntroLocation = IntroLocation.Top;
        }

        public override List<BezierCurve> GetPaths()
        {
            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(286.6868F,-100),
                ControlPoint1 = new PointF(285.7817F,159.2943F),
                ControlPoint2 = new PointF(281.2563F,409.0968F),
                EndPoint = new PointF(324.7005F,650F)},

                new BezierCurve() {StartPoint =  new PointF(325F,651F),
                ControlPoint1 = new PointF(321.0802F,388.28F),
                ControlPoint2 = new PointF(249.5782F,38.01342F),
                EndPoint = new PointF(244.1476F,-100F)},

            };

            return paths;
        }
    }
}
