using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths.Intros
{
    public class Intro3 : IntroBase
    {
        public Intro3()
        {
            IntroLocation = IntroLocation.LowerLeft;
        }

        public override List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>
            {
                new BezierCurve() {StartPoint = new PointF(1.520065F,665.9338F),
                ControlPoint1 = new PointF(208.5863F,814.8393F),
                ControlPoint2 = new PointF(296.1383F,510.5748F),
                EndPoint = new PointF(295.2715F,468.966F)},

                new BezierCurve() {StartPoint = new PointF(294.4046F,460.2975F),
                ControlPoint1 = new PointF(293.5378F,358.0092F),
                ControlPoint2 = new PointF(124.5016F,349.3407F),
                EndPoint = new PointF(130.0312F,480.4071F)},

                new BezierCurve() {StartPoint = new PointF(131.2323F,487.6132F),
                ControlPoint1 = new PointF(137.5044F,559.9853F),
                ControlPoint2 = new PointF(297.0052F,598.1267F),
                EndPoint = new PointF(303.94F,462.8981F)},
            };

            return paths;
        }
    }
}
