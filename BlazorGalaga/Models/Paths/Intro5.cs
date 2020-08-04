using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths
{
    public class Intro5 : IIntro
    {
        public int Offset { get; set; }

        public List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>();
            
            Offset = 52;

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(1.520065F, 865.9338F - Offset),
                ControlPoint1 = new PointF(206.8977F, 885.1501F - Offset),
                ControlPoint2 = new PointF(377.4452F - Offset, 484.0102F),
                EndPoint = new PointF(337.8109F - Offset, 458.7888F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(337.8109F - Offset, 453.9847F),
                ControlPoint1 = new PointF(343.8161F - Offset, 351.8982F + Offset),
                ControlPoint2 = new PointF(116.8198F + Offset, 302.6565F + Offset),
                EndPoint = new PointF(130.0312F + Offset, 480.4071F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(131.2323F + Offset, 487.6132F),
                ControlPoint1 = new PointF(133.6343F + Offset, 580.0916F - Offset),
                ControlPoint2 = new PointF(324.5995F, 610.1171F - Offset),
                EndPoint = new PointF(340.213F - Offset, 461.1908F - Offset)
            });

            return paths;
        }
    }
}
