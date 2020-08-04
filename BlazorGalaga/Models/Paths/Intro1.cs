using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths
{
    public class Intro1 : IIntro
    {
        public int Offset { get; set; }

        public List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(195.2452F, 0),
                ControlPoint1 = new PointF(192.8793F, 139.589F),
                ControlPoint2 = new PointF(514.6467F, 434.1454F),
                EndPoint = new PointF(558.4166F, 476.7318F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(558.4166F, 477.9148F),
                ControlPoint1 = new PointF(664.8837F, 566.6366F),
                ControlPoint2 = new PointF(573.7952F, 728.7018F),
                EndPoint = new PointF(448.4005F, 656.5414F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(446.0345F, 655.3584F),
                ControlPoint1 = new PointF(421.8319F, 640.6024F),
                ControlPoint2 = new PointF(386.291F, 580.7897F),
                EndPoint = new PointF(375.0219F, 521.8439F)
            });

            return paths;
        }
    }
}
