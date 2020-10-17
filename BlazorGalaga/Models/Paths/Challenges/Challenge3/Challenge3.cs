using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge3
{
    public class Challenge3 : IntroBase
    {
        public Challenge3()
        {
            IsChallenge = true;
            IntroLocation = IntroLocation.LowerLeft;
        }

        public override List<BezierCurve> GetPaths()
        {
            List<BezierCurve> paths = new List<BezierCurve>
            {

                new BezierCurve() {StartPoint = new PointF(7.744746F,776.0201F),
                ControlPoint1 = new PointF(90.55254F,778.386F),
                ControlPoint2 = new PointF(152.0669F,777.203F),
                EndPoint = new PointF(231.3258F,778.386F)},

                new BezierCurve() {StartPoint = new PointF(231.3258F,771.2882F),
                ControlPoint1 = new PointF(26.67224F,733.4336F),
                ControlPoint2 = new PointF(55.06348F,488.5614F),
                EndPoint = new PointF(51.51458F,308.7519F)},

                new BezierCurve() {StartPoint = new PointF(52.69755F,304.0201F),
                ControlPoint1 = new PointF(53.88052F,49.68421F),
                ControlPoint2 = new PointF(288.1083F,114.7469F),
                EndPoint = new PointF(286.9253F,199.9198F)},

                new BezierCurve() {StartPoint =new PointF(286.9253F,209.3835F),
                ControlPoint1 = new PointF(286.9253F,370.2657F),
                ControlPoint2 = new PointF(286.9253F,346.6065F),
                EndPoint = new PointF(288.1083F,489.7444F)},

                new BezierCurve() {StartPoint = new PointF(295.7975F,489.7444F),
                ControlPoint1 = new PointF(469.6939F,492.1103F),
                ControlPoint2 = new PointF(502.817F,492.1103F),
                EndPoint = new PointF(710F,490.9273F)},

            };

            return paths;
        }
    }
}
