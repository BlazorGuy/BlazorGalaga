using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge3
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
                new BezierCurve() {StartPoint = new PointF(385.7032F,1.182957F),
                ControlPoint1 = new PointF(386.8861F,154.9674F),
                ControlPoint2 = new PointF(424.7411F,559.5388F),
                EndPoint = new PointF(237.8321F,741.7143F)},

                new BezierCurve() {StartPoint = new PointF(231.9173F,744.0802F),
                ControlPoint1 = new PointF(67.48465F,876.5714F),
                ControlPoint2 = new PointF(22.53185F,623.4185F),
                EndPoint = new PointF(76.9484F,552.4411F)},

                new BezierCurve() {StartPoint = new PointF(79.31434F,550.0752F),
                ControlPoint1 = new PointF(146.7435F,453.0727F),
                ControlPoint2 = new PointF(570.2462F,421.1328F),
                EndPoint = new PointF(Constants.CanvasSize.Width + 50,398.6566F)},
            };

            return paths;
        }
    }
}
