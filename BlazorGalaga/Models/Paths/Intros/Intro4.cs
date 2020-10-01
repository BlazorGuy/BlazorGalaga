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
                new BezierCurve()
                {
                    StartPoint = new PointF(666.8956F, 665.9338F),
                    ControlPoint1 = new PointF(459.1159F, 889.9542F),
                    ControlPoint2 = new PointF(307.785F, 497.2214F),
                    EndPoint = new PointF(337.8109F, 458.7888F)
                },

                new BezierCurve()
                {
                    StartPoint = new PointF(337.8109F, 453.9847F),
                    ControlPoint1 = new PointF(333.0068F, 339.888F),
                    ControlPoint2 = new PointF(550.3948F, 290.6463F),
                    EndPoint = new PointF(555.199F, 455.1858F)
                },

                new BezierCurve()
                {
                    StartPoint = new PointF(555.199F, 456.3868F),
                    ControlPoint1 = new PointF(563.6063F, 576.4885F),
                    ControlPoint2 = new PointF(336.6099F, 617.3232F),
                    EndPoint = new PointF(336.6099F, 467.1959F)
                }
            };

            return paths;
        }
    }
}
