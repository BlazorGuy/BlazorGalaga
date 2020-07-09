using System;
using System.Collections.Generic;
using System.Drawing;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Static;

namespace BlazorGalaga.Services
{
    public class BezierCurveService
    {
        public BezierCurveService()
        {
        }

        public async void DrawCurve(Canvas2DContext ctx, BezierCurve curve)
        {
            await ctx.BeginPathAsync();
            //await ctx.SetLineWidthAsync(2);
            await ctx.MoveToAsync(curve.StartPoint.X, curve.StartPoint.Y);
            await ctx.BezierCurveToAsync(curve.ControlPoint1.X,curve.ControlPoint1.Y,curve.ControlPoint2.X, curve.ControlPoint2.Y,curve.EndPoint.X,curve.EndPoint.Y);
            //await ctx.SetStrokeStyleAsync("white");
            await ctx.StrokeAsync();

            await ctx.BeginPathAsync();
            //await ctx.SetFillStyleAsync("yellow");
            await ctx.ArcAsync(curve.StartPoint.X, curve.StartPoint.Y, 5, 0, Math.PI * 2);
            await ctx.FillAsync();

            await ctx.BeginPathAsync();
            //await ctx.SetFillStyleAsync("yellow");
            await ctx.ArcAsync(curve.EndPoint.X, curve.EndPoint.Y, 5, 0, Math.PI * 2);
            await ctx.FillAsync();

        }

        public async void DrawCurveControlLines(Canvas2DContext ctx, BezierCurve curve)
        {

            await ctx.BeginPathAsync();
            await ctx.MoveToAsync(curve.ControlPoint1.X, curve.ControlPoint1.Y);
            await ctx.LineToAsync(curve.StartPoint.X, curve.StartPoint.Y);
            //await ctx.SetStrokeStyleAsync("red");
            await ctx.StrokeAsync();

            await ctx.BeginPathAsync();
            //await ctx.SetFillStyleAsync("yellow");
            await ctx.ArcAsync(curve.ControlPoint1.X, curve.ControlPoint1.Y, 5, 0, Math.PI * 2);
            await ctx.FillAsync();

            await ctx.BeginPathAsync();
            await ctx.MoveToAsync(curve.ControlPoint2.X, curve.ControlPoint2.Y);
            await ctx.LineToAsync(curve.EndPoint.X, curve.EndPoint.Y);
            //await ctx.SetStrokeStyleAsync("red");
            await ctx.StrokeAsync();

            await ctx.BeginPathAsync();
           // await ctx.SetFillStyleAsync("yellow");
            await ctx.ArcAsync(curve.ControlPoint2.X, curve.ControlPoint2.Y, 5, 0, Math.PI * 2);
            await ctx.FillAsync();
        }

        public List<PointF> GetEvenlyDistributedPathPointsByLength(List<PointF> points, float segmentLength)
        {
            List<float> lengths = new List<float>();

            lengths.Add(0);

            for (var i = 1; i < points.Count; i++)
            {
                var dx = points[i].X - points[i - 1].X;
                var dy = points[i].Y - points[i - 1].Y;
                lengths.Add(MathF.Sqrt(dx * dx + dy * dy));
            }

            var accumLength = segmentLength;
            var nextLength = segmentLength;
            List<PointF> sPoints = new List<PointF>();

            sPoints.Add(new PointF(points[0].X, points[0].Y));

            for (var i = 1; i < lengths.Count; i++)
            {
                accumLength += lengths[i];
                if (accumLength >= nextLength)
                {
                    sPoints.Add(new PointF(points[i].X, points[i].Y));
                    nextLength += segmentLength;
                }
            }
            return sPoints;
        }

        public float GetRotationAngleAlongPath(IAnimatable animatable)
        {
            double dx = animatable.PevLocation.X - animatable.NextLocation.X;
            double dy = animatable.PevLocation.Y - animatable.NextLocation.Y;
            double angrad = Math.Atan2(dy, dx);
            double angdeg = angrad * 180.0f / Math.PI;

            return (float)angdeg;
        }

        //for strait lines
        public PointF getLineXYatPercent(BezierCurve curve, float percent)
        {
            percent /= 100;

            var dx = curve.EndPoint.X - curve.StartPoint.X;
            var dy = curve.EndPoint.Y - curve.StartPoint.Y;
            var X = curve.StartPoint.X + dx * percent;
            var Y = curve.StartPoint.Y + dy * percent;

            return new PointF(X, Y);
        }

        public PointF getQuadraticBezierXYatPercent(BezierCurve curve, float percent)
        {
            var x = MathF.Pow(1 - percent, 2) * curve.StartPoint.X + 2 * (1 - percent) * percent * curve.ControlPoint1.X + MathF.Pow(percent, 2) * curve.EndPoint.X;
            var y = MathF.Pow(1 - percent, 2) * curve.StartPoint.Y + 2 * (1 - percent) * percent * curve.ControlPoint1.Y + MathF.Pow(percent, 2) * curve.EndPoint.Y;

            return new PointF(x, y);
        }

        public PointF getCubicBezierXYatPercent(BezierCurve curve, float percent)
        {
            percent /= 100;

            var x = CubicPoint(percent, curve.StartPoint.X, curve.ControlPoint1.X, curve.ControlPoint2.X, curve.EndPoint.X);
            var y = CubicPoint(percent, curve.StartPoint.Y, curve.ControlPoint1.Y, curve.ControlPoint2.Y, curve.EndPoint.Y);

            return new PointF(x,y);
        }

        private float CubicPoint(float percent, float p1, float p2, float p3, float p4)
        {
            var b = (1 - percent);
            return MathF.Pow(b, 3) * p1 + 3 * MathF.Pow(b, 2)
                * percent * p2 + 3 * b * MathF.Pow(percent, 2)
                * p3 + MathF.Pow(percent, 3) * p4;
        }
    }
}
