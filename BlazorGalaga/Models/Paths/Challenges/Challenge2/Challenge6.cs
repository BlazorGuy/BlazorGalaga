using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge2
{
    public class Challenge6 : IntroBase
    {
        public Challenge6()
        {
            IsChallenge = true;
            IntroLocation = IntroLocation.Top;
        }

        public override List<BezierCurve> GetPaths()
        {
            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(385.3415F,-100),
                ControlPoint1 = new PointF(374.4804F,160.1994F),
                ControlPoint2 = new PointF(368.1448F,407.2867F),
                EndPoint = new PointF(324.7005F,650F)},

                new BezierCurve() {StartPoint = new PointF(323.7954F,650F),
                ControlPoint1 = new PointF(321.0802F,388.28F),
                ControlPoint2 = new PointF(421.545F,38.01342F),
                EndPoint = new PointF(430.5959F,-1000F)},
            };

            return paths;
        }
    }
}
