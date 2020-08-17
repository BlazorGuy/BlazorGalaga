using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths.Intros
{
    public class Intro2 : IntroBase
    {
        public Intro2()
        {
            IntroLocation = IntroLocation.Top;
        }

        public override List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve()
                {
                    StartPoint = new PointF(459.1159F, 0),
                    ControlPoint1 = new PointF(462.719F, 121.3028F),
                    ControlPoint2 = new PointF(166.0624F, 405.944F),
                    EndPoint = new PointF(112.0156F, 464.7939F)
                },

                new BezierCurve()
                {
                    StartPoint = new PointF(110.8146F, 467.1959F),
                    ControlPoint1 = new PointF(7.525259F, 559.6743F),
                    ControlPoint2 = new PointF(109.6136F, 682.1781F),
                    EndPoint = new PointF(191.2842F, 649.7506F)
                },

                new BezierCurve()
                {
                    StartPoint = new PointF(193.6863F, 649.7506F),
                    ControlPoint1 = new PointF(276.558F, 587.2977F),
                    ControlPoint2 = new PointF(288.5684F, 542.86F),
                    EndPoint = new PointF(295.7746F, 460)
                }
            };

            return paths;
        }
    }
}
