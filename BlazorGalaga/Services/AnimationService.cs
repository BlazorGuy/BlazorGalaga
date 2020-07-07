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

            //var shipAnimation = new Animation() { Speed = 0 };
            //List<BezierCurve> paths2 = new List<BezierCurve>();
            //paths2.Add(new BezierCurve()
            //{
            //    StartPoint = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
            //    EndPoint = new PointF(Constants.CanvasSize.Width - Constants.SpriteDestSize.Width, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
            //    ControlPoint1 = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height),
            //    ControlPoint2 = new PointF(0, Constants.CanvasSize.Height - Constants.SpriteDestSize.Height)
            //});
            //var ship = new Ship()
            //{
            //    Paths = paths2,
            //    DrawPath = false,
            //    PathIsLine = true,
            //    RotateAlongPath = false
            //};
            //shipAnimation.Animatables.Add(ship);
            //Animations.Add(shipAnimation);

        }

        public async Task ResetCanvas()
        {
            await CanvasCtx.ClearRectAsync(0, 0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
            //await CanvasCtx.SetFillStyleAsync("#000000");
            //await CanvasCtx.FillRectAsync(0, 0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
        }

        public void Animate()
        {
            foreach (IAnimatable animatable in Animatables) {
                if (animatable.CurPathPointIndex > 0)
                    animatable.PevLocation = animatable.PathPoints[animatable.CurPathPointIndex - 1];

                animatable.Location = animatable.PathPoints[animatable.CurPathPointIndex];

                if (animatable.CurPathPointIndex < animatable.PathPoints.Count)
                    animatable.NextLocation = animatable.PathPoints[animatable.CurPathPointIndex + 1];

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
                    foreach (BezierCurve path in animatable.Paths)
                    {
                        bezierCurveService.DrawCurve(CanvasCtx, path);
                        for (var percent = 0F; percent <= 100; percent+=.1F)
                        {
                            var point = bezierCurveService.getCubicBezierXYatPercent(path, percent);
                            animatable.PathPoints.Add(point);
                        }
                    }
                    animatable.PathPoints = bezierCurveService.GetEvenlyDistributedPathPointsByLength(animatable.PathPoints, pointgranularity);
                }
            }
        }

        public void Draw()
        {

            foreach (IAnimatable animatable in Animatables) {
                spriteService.DrawSprite(animatable.Sprite, animatable.Location, animatable.RotateAlongPath ? animatable.Rotation : 0);

                if (animatable.DrawPath)
                {
                    foreach (BezierCurve path in animatable.Paths)
                        bezierCurveService.DrawCurve(CanvasCtx, path);
                }

                if (animatable.DrawPathPoints)
                {
                    bezierCurveService.DrawPathPoints(CanvasCtx, animatable.PathPoints);
                }
            }
            
        }
    }
}
