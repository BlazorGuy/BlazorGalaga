using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge1
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
                new BezierCurve() {StartPoint = new PointF(281.6019F,1.182957F),
                ControlPoint1 = new PointF(281.6019F,152.6015F),
                ControlPoint2 = new PointF(302.8954F,558.3559F),
                EndPoint = new PointF(459.0472F,715.6892F)},

                new BezierCurve() {StartPoint = new PointF(461.4131F,719.2381F),
                ControlPoint1 = new PointF(627.0287F,871.8396F),
                ControlPoint2 = new PointF(664.8837F,553.6241F),
                EndPoint = new PointF(579.71F,520.5013F)},

                new BezierCurve() {StartPoint = new PointF(578.527F,519.3183F),
                ControlPoint1 = new PointF(521.7445F,438.8772F),
                ControlPoint2 = new PointF(94.69292F,424.6817F),
                EndPoint = new PointF(-1000F,398.6566F)},
            };

            return paths;
        }
    }
}
