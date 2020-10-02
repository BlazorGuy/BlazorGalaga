using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge2
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
                new BezierCurve() {StartPoint = new PointF(Constants.CanvasSize.Width+100,797.3768F),
                ControlPoint1 = new PointF(92.99786F,797.3768F),
                ControlPoint2 = new PointF(13.35006F,882.4545F),
                EndPoint = new PointF(51.36378F,249.8025F)},

                new BezierCurve() {StartPoint = new PointF(52.26887F,244.372F),
                ControlPoint1 = new PointF(70.37064F,98.65388F),
                ControlPoint2 = new PointF(646.912F,16.29147F),
                EndPoint = new PointF(619.7594F,453.4458F)},

                new BezierCurve() {StartPoint = new PointF(620F,456.1611F),
                ControlPoint1 = new PointF(615.2339F,563.8658F),
                ControlPoint2 = new PointF(667.7291F,799.187F),
                EndPoint = new PointF(-100F,798.2819F)},
            };

            return paths;
        }
    }
}
