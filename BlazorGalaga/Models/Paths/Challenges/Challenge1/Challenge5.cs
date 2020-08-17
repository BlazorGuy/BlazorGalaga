using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge1
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

                new BezierCurve() {StartPoint = new PointF(386.8861F,4.73183F),
                ControlPoint1 = new PointF(395.1669F,144.3208F),
                ControlPoint2 = new PointF(246.1129F,733.4336F),
                EndPoint = new PointF(173.9518F,764.1905F)},

                new BezierCurve() {StartPoint = new PointF(172.7688F,765.3734F),
                ControlPoint1 = new PointF(41.45935F,832.802F),
                ControlPoint2 = new PointF(3.604357F,618.6867F),
                EndPoint = new PointF(82.86324F,542.9774F)},

                new BezierCurve() {StartPoint = new PointF(86.41215F,539.4286F),
                ControlPoint1 = new PointF(132.5479F,496.8421F),
                ControlPoint2 = new PointF(367.9586F,449.5238F),
                EndPoint = new PointF(750,400)},

            };

            return paths;
        }
    }
}
