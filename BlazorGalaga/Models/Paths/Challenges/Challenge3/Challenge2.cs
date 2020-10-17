using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge3
{
    public class Challenge2 : IntroBase
    {
        public Challenge2()
        {
            IsChallenge = true;
            IntroLocation = IntroLocation.Top;
        }

        public override List<BezierCurve> GetPaths()
        {
            List<BezierCurve> paths = new List<BezierCurve>
            {

                new BezierCurve() {StartPoint = new PointF(281.0104F,1F),
                ControlPoint1 = new PointF(294.0231F,431.7794F),
                ControlPoint2 = new PointF(224.228F,702.6767F),
                EndPoint = new PointF(628.8032F,664.8221F)},

                new BezierCurve() {StartPoint = new PointF(627.6202F,664.8221F),
                ControlPoint1 = new PointF(225.4109F,695.5789F),
                ControlPoint2 = new PointF(310.5847F,467.2682F),
                EndPoint = new PointF(282.1934F,-50F)},


            };

            return paths;
        }
    }
}
