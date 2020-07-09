using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Models;
using BlazorGalaga.Static;
using System.Linq;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Services
{
    public class AnimationService
    {
        public List<IAnimatable> Animatables = new List<IAnimatable>();
        public Canvas2DContext CanvasCtx { get; set; }

        private BezierCurveService bezierCurveService;
        private SpriteService spriteService;

        public AnimationService(BezierCurveService bcs, SpriteService ss)
        {
            bezierCurveService = bcs;
            spriteService = ss;
        }

        public void InitAnimations()
        {
            Animatables.AddRange(BugFactory.CreateAnimation_BugIntro1());

            List<BezierCurve> paths = new List<BezierCurve>();
            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(Constants.SpriteDestSize.Width/2, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
                EndPoint = new PointF(Constants.CanvasSize.Width - (Constants.SpriteDestSize.Width/2), Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
                ControlPoint1 = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
                ControlPoint2 = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height)
            });
            var ship = new Ship()
            {
                Paths = paths,
                DrawPath = false,
                PathIsLine = true,
                RotateAlongPath = false,
            };
            Animatables.Add(ship);

        }

        public void ResetCanvas()
        {
            CanvasCtx.ClearRectAsync(0, 0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
            //await CanvasCtx.SetFillStyleAsync("#000000");
            //await CanvasCtx.FillRectAsync(0, 0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
        }

        public void Animate()
        {

                foreach (IAnimatable animatable in Animatables)
                {

                    if (animatable.StartDelay > 0)
                    {
                        animatable.CurPathPointIndex = animatable.StartDelay;
                        animatable.StartDelay = 0;
                    }

                    try
                    {
                        if (animatable.CurPathPointIndex > 0 && animatable.CurPathPointIndex < animatable.PathPoints.Count)
                            animatable.PevLocation = animatable.PathPoints[animatable.CurPathPointIndex - 1];

                        animatable.Location = animatable.PathPoints[animatable.CurPathPointIndex];

                        if (animatable.CurPathPointIndex < animatable.PathPoints.Count)
                            animatable.NextLocation = animatable.PathPoints[animatable.CurPathPointIndex + 1];
                    }
                    catch (Exception ex)
                    {
                        Utils.dOut("Animation Error", ex.Message + "<br/>" + ex.StackTrace);
                    }

                    animatable.Rotation = bezierCurveService.GetRotationAngleAlongPath(animatable);

                    animatable.CurPathPointIndex += animatable.Speed;

                    if (animatable.CurPathPointIndex > animatable.PathPoints.Count - 1)
                    {
                        //this stops the animation
                        animatable.CurPathPointIndex -= animatable.Speed;
                        if (animatable.LoopBack) animatable.Speed *= -1;
                    }
                    if (animatable.CurPathPointIndex < 0)
                    {
                        //this stops the animation
                        animatable.CurPathPointIndex = 0;
                        if (animatable.LoopBack) animatable.Speed *= -1;
                    }
                }

        }

        public void ComputePathPoints()
        {
            int pointgranularity = 1; //the lower the more granular 

            foreach (var animatable in Animatables)
            {
                if (animatable.Paths != null)
                {
                    animatable.PathPoints = new List<PointF>();
                    foreach (BezierCurve path in animatable.Paths)
                    {
                        for (var percent = 0F; percent <= 100; percent+=.01F)
                        {
                            PointF point;
                            if (animatable.PathIsLine)
                                point = bezierCurveService.getLineXYatPercent(path, percent);
                            else
                                point = bezierCurveService.getCubicBezierXYatPercent(path, percent);
                            animatable.PathPoints.Add(point);
                        }
                    }
                    animatable.PathPoints = bezierCurveService.GetEvenlyDistributedPathPointsByLength(animatable.PathPoints, pointgranularity);
                }
            }
        }

        public void Draw()
        {

            spriteService.CanvasCtx.BeginBatchAsync();

            ResetCanvas();

            foreach (IAnimatable animatable in Animatables) {

                spriteService.DrawSprite(animatable.Sprite, animatable.Location, animatable.RotateAlongPath ? animatable.Rotation : 0);

                if (animatable.DrawPath)
                {
                    foreach (BezierCurve path in animatable.Paths)
                        bezierCurveService.DrawCurve(CanvasCtx, path);
                }

                if (animatable.DrawControlLines)
                {
                    foreach (BezierCurve path in animatable.Paths)
                        bezierCurveService.DrawCurveControlLines(CanvasCtx, path);
                }

                if (animatable.DrawPathPoints)
                {
                    bezierCurveService.DrawPathPoints(CanvasCtx, animatable.PathPoints);
                }
            }

            spriteService.CanvasCtx.EndBatchAsync();
        }
    }
}
