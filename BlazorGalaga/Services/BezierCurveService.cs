using System;
using System.Drawing;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;

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
            await ctx.SetLineWidthAsync(2);
            await ctx.MoveToAsync(curve.StartPoint.X, curve.StartPoint.Y);
            await ctx.BezierCurveToAsync(curve.ControlPoint1.X,curve.ControlPoint1.Y,curve.ControlPoint2.X, curve.ControlPoint2.Y,curve.EndPoint.X,curve.EndPoint.Y);
            await ctx.SetStrokeStyleAsync("white");
            await ctx.StrokeAsync();

            await ctx.BeginPathAsync();
            await ctx.SetLineWidthAsync(2);
            await ctx.MoveToAsync(curve.ControlPoint1.X, curve.ControlPoint1.Y);
            await ctx.LineToAsync(curve.StartPoint.X, curve.StartPoint.Y);
            await ctx.SetStrokeStyleAsync("yellow");
            await ctx.StrokeAsync();

            await ctx.BeginPathAsync();
            await ctx.SetLineWidthAsync(2);
            await ctx.MoveToAsync(curve.ControlPoint2.X, curve.ControlPoint2.Y);
            await ctx.LineToAsync(curve.EndPoint.X, curve.EndPoint.Y);
            await ctx.SetStrokeStyleAsync("yellow");
            await ctx.StrokeAsync();
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

        public PointF getCubicBezierXYatPercent(BezierCurve curve, float percent)
        {

            percent /= 100;

            var x = CubicN(percent, curve.StartPoint.X, curve.ControlPoint1.X, curve.ControlPoint2.X, curve.EndPoint.X);
            var y = CubicN(percent, curve.StartPoint.Y, curve.ControlPoint1.Y, curve.ControlPoint2.Y, curve.EndPoint.Y);

            return new PointF(x, y);
        }

        // cubic helper formula at percent distance
        private float CubicN(float percent, float a, float b, float c, float d)
        {
            var t2 = percent * percent;
            var t3 = t2 * percent;
            return a + (-a * 3 + percent * (3 * a - a * percent)) * percent + (3 * b + percent * (-6 * b + b * 3 * percent)) * percent + (c * 3 - c * 3 * percent) * t2 + d * t3;
        }
    }
}
