using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths.Intros
{
    public class Intro8 : IntroBase
    {
        public Intro8()
        {
            IntroLocation = IntroLocation.Top;
        }

        public override List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>();
            
            Offset = 52;

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(195.2452F - Offset, 0),
                ControlPoint1 = new PointF(192.8793F - Offset, 139.589F),
                ControlPoint2 = new PointF(514.6467F - Offset, 434.1454F),
                EndPoint = new PointF(558.4166F - Offset, 476.7318F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(558.4166F - Offset, 477.9148F),
                ControlPoint1 = new PointF(664.8837F - Offset, 566.6366F + Offset),
                ControlPoint2 = new PointF(573.7952F - Offset, 728.7018F - Offset),
                EndPoint = new PointF(448.4005F, 656.5414F - Offset)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(446.0345F, 655.3584F - Offset),
                ControlPoint1 = new PointF(421.8319F, 640.6024F - Offset),
                ControlPoint2 = new PointF(386.291F + (Offset / 2), 580.7897F - Offset),
                EndPoint = new PointF(375.0219F + (Offset / 2), 521.8439F - Offset)
            });

            return paths;
        }
    }
}
