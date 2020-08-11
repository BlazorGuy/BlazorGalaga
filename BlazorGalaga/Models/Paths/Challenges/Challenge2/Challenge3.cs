using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models.Paths.Intros;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models.Paths.Challenges.Challenge2
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
                new BezierCurve() {StartPoint = new PointF(-100,785.6107F),
                ControlPoint1 = new PointF(235.0968F,781.0853F),
                ControlPoint2 = new PointF(665.9189F,881.5494F),
                EndPoint = new PointF(628.8103F,235.3212F)},

                new BezierCurve() {StartPoint = new PointF(627.0001F,234.4161F),
                ControlPoint1 = new PointF(629.7154F,92.31831F),
                ControlPoint2 = new PointF(10.63479F,18.10163F),
                EndPoint = new PointF(60.41467F,436.2493F)},

                new BezierCurve() {StartPoint = new PointF(61.31975F,440.7747F),
                ControlPoint1 = new PointF(76.70626F,557.5302F),
                ControlPoint2 = new PointF(21.49585F,890.6002F),
                EndPoint = new PointF(Constants.CanvasSize.Width+100F,753.9329F)},
            };

            return paths;
        }
    }
}
