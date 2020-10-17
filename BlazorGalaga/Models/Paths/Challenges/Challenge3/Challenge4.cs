using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge3
{
    public class Challenge4 : IntroBase
    {
        public Challenge4()
        {
            IsChallenge = true;
            IntroLocation = IntroLocation.LowerRight;
        }

        public override List<BezierCurve> GetPaths()
        {
            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(668.4326F,776.0201F),
                ControlPoint1 = new PointF(601.0034F,777.203F),
                ControlPoint2 = new PointF(526.4764F,777.203F),
                EndPoint = new PointF(421.1922F,776.0201F)},

                new BezierCurve() {StartPoint = new PointF(421.1922F,768.9223F),
                ControlPoint1 = new PointF(642.4073F,731.0677F),
                ControlPoint2 = new PointF(617.5649F,472F),
                EndPoint = new PointF(617.5649F,321.7644F)},

                new BezierCurve() {StartPoint = new PointF(616.382F,312.3008F),
                ControlPoint1 = new PointF(610.4672F,81.62406F),
                ControlPoint2 = new PointF(382.1543F,117.1128F),
                EndPoint = new PointF(384.5202F,205.8346F)},

                new BezierCurve() {StartPoint = new PointF(384.5202F,214.1153F),
                ControlPoint1 = new PointF(385.7032F,365.5338F),
                ControlPoint2 = new PointF(385.7032F,347.7895F),
                EndPoint = new PointF(386.8861F,488.5614F)},

                new BezierCurve() {StartPoint = new PointF(378.6053F,488.5614F),
                ControlPoint1 = new PointF(246.1129F,488.5614F),
                ControlPoint2 = new PointF(168.037F,488.5614F),
                EndPoint = new PointF(-50F,490.9273F)},

            };

            return paths;
        }
    }
}
