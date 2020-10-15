using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths.Intros
{
    public class Intro6 : IntroBase
    {
        public Intro6()
        {
            IntroLocation = IntroLocation.LowerRight;
        }

        public override List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(666.8956F,613.9338F),
                ControlPoint1 = new PointF(469.5088F,779.2985F),
                ControlPoint2 = new PointF(397.5601F,524.4445F),
                EndPoint = new PointF(401.0275F,461.1644F)},

                new BezierCurve() {StartPoint = new PointF(401.0275F,454.2296F),
                ControlPoint1 = new PointF(414.0302F,404.8191F),
                ControlPoint2 = new PointF(474.7099F,386.6152F),
                EndPoint = new PointF(489.4464F,449.8953F)},

                new BezierCurve() {StartPoint = new PointF(491.1801F,456.8301F),
                ControlPoint1 = new PointF(511.6063F,524.4885F),
                ControlPoint2 = new PointF(385.4241F,556.5179F),
                EndPoint = new PointF(397.5601F,422.1561F)},
            };

            return paths;
        }
    }
}
