using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge2
{
    public class Challenge4 : IntroBase
    {
        public Challenge4()
        {
            IsChallenge = true;
        }

        public override List<BezierCurve> GetPaths()
        {
            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(1466.896F,865.9338F),
                ControlPoint1 = new PointF(1466.896F,865.9338F),
                ControlPoint2 = new PointF(666.8956F,865.9338F),
                EndPoint = new PointF(666.8956F,865.9338F)},

                new BezierCurve() {StartPoint = new PointF(666.8956F,865.9338F),
                ControlPoint1 = new PointF(459.1159F,889.9542F),
                ControlPoint2 = new PointF(183.4156F,726.3358F),
                EndPoint = new PointF(172.7688F,565.4536F)},

                new BezierCurve() {StartPoint = new PointF(172.7688F,559.5388F),
                ControlPoint1 = new PointF(170.4029F,398.6566F),
                ControlPoint2 = new PointF(147.9265F,312.3008F),
                EndPoint = new PointF(224.8195F,330.0451F)},

                new BezierCurve() {StartPoint = new PointF(228.3684F,330.0451F),
                ControlPoint1 = new PointF(285.1508F,321.7644F),
                ControlPoint2 = new PointF(272.1382F,486.1955F),
                EndPoint = new PointF(274.5041F,563.0877F)},

                new BezierCurve() {StartPoint = new PointF(274.5041F,563.0877F),
                ControlPoint1 = new PointF(279.236F,677.8346F),
                ControlPoint2 = new PointF(78.13137F,605.6742F),
                EndPoint = new PointF(-50,250)},

            };

            return paths;
        }
    }
}
