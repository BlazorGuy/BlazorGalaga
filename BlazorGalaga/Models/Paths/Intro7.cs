using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths
{
    public class Intro7 : IIntro
    {
        public int Offset { get; set; }

        public List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>();
            
            Offset = 52;

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(459.1159F + Offset, 0),
                ControlPoint1 = new PointF(462.719F + Offset, 121.3028F),
                ControlPoint2 = new PointF(166.0624F + Offset, 405.944F),
                EndPoint = new PointF(112.0156F + Offset, 464.7939F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(110.8146F + Offset, 467.1959F),
                ControlPoint1 = new PointF(7.525259F + Offset , 559.6743F + Offset),
                ControlPoint2 = new PointF(109.6136F + Offset, 682.1781F - Offset),
                EndPoint = new PointF(191.2842F, 649.7506F - Offset)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(193.6863F, 649.7506F - Offset),
                ControlPoint1 = new PointF(276.558F, 587.2977F - Offset),
                ControlPoint2 = new PointF(288.5684F-(Offset/2), 542.86F - Offset),
                EndPoint = new PointF(295.7746F-(Offset/2), 491.2163F - Offset)
            });

            return paths;
        }
    }
}
