using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths
{
    public class Intro3 : IIntro
    {
        public int Offset { get; set; }
        public bool IsChallenge { get; set; }

        public List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>();

             paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(1.520065F, 865.9338F),
                ControlPoint1 = new PointF(206.8977F, 885.1501F),
                ControlPoint2 = new PointF(377.4452F, 484.0102F),
                EndPoint = new PointF(337.8109F, 458.7888F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(337.8109F, 453.9847F),
                ControlPoint1 = new PointF(343.8161F, 351.8982F),
                ControlPoint2 = new PointF(116.8198F, 302.6565F),
                EndPoint = new PointF(130.0312F, 480.4071F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(131.2323F, 487.6132F),
                ControlPoint1 = new PointF(133.6343F, 580.0916F),
                ControlPoint2 = new PointF(324.5995F, 610.1171F),
                EndPoint = new PointF(340.213F, 461.1908F)
            });

            return paths;
        }
    }
}
