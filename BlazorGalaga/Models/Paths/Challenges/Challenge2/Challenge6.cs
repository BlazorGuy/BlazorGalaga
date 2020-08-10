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
        }

        public override List<BezierCurve> GetPaths()
        {
            List<BezierCurve> paths = new List<BezierCurve>
            {

                new BezierCurve() {StartPoint = new PointF(283.9679F,0F),
                ControlPoint1 = new PointF(270.9552F,138.406F),
                ControlPoint2 = new PointF(367.9586F,719.2381F),
                EndPoint = new PointF(454.3153F,754.7268F)},

                new BezierCurve() {StartPoint = new PointF(454.3153F,754.7268F),
                ControlPoint1 = new PointF(598.6375F,832.802F),
                ControlPoint2 = new PointF(669.6156F,629.3333F),
                EndPoint = new PointF(556.0506F,545.3434F)},

                new BezierCurve() {StartPoint = new PointF(552.5017F,541.7945F),
                ControlPoint1 = new PointF(501.6341F,512.2206F),
                ControlPoint2 = new PointF(197.6112F,414.0351F),
                EndPoint = new PointF(-50,400)},

            };

            return paths;
        }
    }
}
