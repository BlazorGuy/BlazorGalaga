using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths
{
    public class Intro6 : IIntro
    {
        public int Offset { get; set; }
        public bool IsChallenge { get; set; }

        public List<BezierCurve> GetPaths()
        {

            List<BezierCurve> paths = new List<BezierCurve>();
            
            Offset = 52;

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(666.8956F, 865.9338F - Offset),
                ControlPoint1 = new PointF(459.1159F, 889.9542F - Offset),
                ControlPoint2 = new PointF(307.785F + Offset, 497.2214F),
                EndPoint = new PointF(337.8109F + Offset, 458.7888F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(337.8109F + Offset, 453.9847F),
                ControlPoint1 = new PointF(333.0068F + Offset, 339.888F + Offset),
                ControlPoint2 = new PointF(550.3948F - Offset, 290.6463F + Offset),
                EndPoint = new PointF(555.199F - Offset, 455.1858F)
            });

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(555.199F - Offset, 456.3868F),
                ControlPoint1 = new PointF(563.6063F - Offset, 576.4885F - Offset),
                ControlPoint2 = new PointF(336.6099F, 617.3232F - Offset),
                EndPoint = new PointF(336.6099F + Offset, 467.1959F - Offset)
            });

            return paths;
        }
    }
}
