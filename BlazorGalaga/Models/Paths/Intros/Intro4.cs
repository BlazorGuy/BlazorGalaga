using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths.Intros
{
    public class Intro4 : IntroBase
    {
        public Intro4()
        {
            IntroLocation = IntroLocation.LowerRight;
        }

        public override List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(666.8956F,665.9338F),
                ControlPoint1 = new PointF(461.7071F,813.9725F),
                ControlPoint2 = new PointF(367.2202F,513.1754F),
                EndPoint = new PointF(368.0871F,474.1671F)},

                new BezierCurve() {StartPoint = new PointF(368.0871F,463.7649F),
                ControlPoint1 = new PointF(369.8208F,358.876F),
                ControlPoint2 = new PointF(524.9873F,354.5418F),
                EndPoint = new PointF(526.721F,450.7622F)},

                new BezierCurve() {StartPoint = new PointF(527.5878F,459.4307F),
                ControlPoint1 = new PointF(537.1232F,565.1864F),
                ControlPoint2 = new PointF(369.8208F,595.5262F),
                EndPoint = new PointF(360.2854F,464.6318F)},
            };

            return paths;
        }
    }
}
