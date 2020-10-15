using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths.Intros
{
    public class Intro5 : IntroBase
    {
        public Intro5()
        {
            IntroLocation = IntroLocation.LowerLeft;
        }

        public override List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(1.520065F,613.9338F),
                ControlPoint1 = new PointF(170.4448F,760.2277F),
                ControlPoint2 = new PointF(257.13F,535.7135F),
                EndPoint = new PointF(248.8109F,458.7888F)},

                new BezierCurve() {StartPoint = new PointF(248.8109F,453.9847F),
                ControlPoint1 = new PointF(247.5946F,411.7539F),
                ControlPoint2 = new PointF(171.3116F,396.1506F),
                EndPoint = new PointF(169.5779F,469.8329F)},

                new BezierCurve() {StartPoint = new PointF(169.5779F,478.5014F),
                ControlPoint1 = new PointF(147.0398F,514.9091F),
                ControlPoint2 = new PointF(309.1411F,575.5886F),
                EndPoint = new PointF(233.725F,405.6859F)},

            };

            return paths;
           
        }
    }
}
