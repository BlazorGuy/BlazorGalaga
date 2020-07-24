using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Models;
using BlazorGalaga.Static;
using System.Linq;
using BlazorGalaga.Interfaces;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace BlazorGalaga.Services
{
    public class AnimationService
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public List<IAnimatable> Animatables = new List<IAnimatable>();

        private BezierCurveService bezierCurveService;
        private SpriteService spriteService;

        public AnimationService(BezierCurveService bcs, SpriteService ss)
        {
            bezierCurveService = bcs;
            spriteService = ss;
        }

        public void ResetCanvas(Canvas2DContext ctx)
        {
            ctx.ClearRectAsync(0, 0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
        }

        public void Animate()
        {
            foreach (IAnimatable animatable in Animatables.Where(a=>a.Started))
            {
                if (animatable.StartDelay > 0 && !animatable.StartDelayStarted)
                {
                    animatable.CurPathPointIndex = animatable.StartDelay;
                    animatable.StartDelayStarted = true;
                }

                try
                {
                    if (animatable.CurPathPointIndex - 1 > 0 && animatable.CurPathPointIndex - 1 < animatable.PathPoints.Count)
                        animatable.PevLocation = animatable.PathPoints[animatable.CurPathPointIndex - 1];

                    if (animatable.CurPathPointIndex > 0 && animatable.CurPathPointIndex  < animatable.PathPoints.Count)
                        animatable.Location = animatable.PathPoints[animatable.CurPathPointIndex];

                    if (animatable.CurPathPointIndex + 1 > 0 && animatable.CurPathPointIndex + 1 < animatable.PathPoints.Count)
                        animatable.NextLocation = animatable.PathPoints[animatable.CurPathPointIndex + 1];
                }
                catch (Exception ex)
                {
                    Utils.dOut("Animation Error", ex.Message + "<br/>" + ex.StackTrace + "<br/>" + " animatable.CurPathPointIndex: " + animatable.CurPathPointIndex + " animatable.PathPoints.Count: " + animatable.PathPoints.Count);
                }

                animatable.CurPathPointIndex += animatable.Speed;

                if (animatable.CurPathPointIndex > animatable.PathPoints.Count - 1)
                {
                    //this stops the animation
                    animatable.CurPathPointIndex = animatable.PathPoints.Count - 1;
                    if (animatable.LineToLocation != null && animatable.LineToLocationPercent <= 100)
                    {
                        animatable.PevLocation = animatable.Location;
                        animatable.Location = bezierCurveService.getLineXYatPercent((PointF)animatable.LineFromToLocation, (PointF)animatable.LineToLocation, animatable.LineToLocationPercent);
                        animatable.NextLocation = bezierCurveService.getLineXYatPercent((PointF)animatable.LineFromToLocation, (PointF)animatable.LineToLocation, animatable.LineToLocationPercent + animatable.LineToLocationSpeed);

                        animatable.LineToLocationPercent += animatable.LineToLocationSpeed;

                        animatable.PathPoints.Clear();
                        animatable.IsMoving = true;
                        var rotation = bezierCurveService.GetRotationAngleAlongPath(animatable);

                        //this fixes gittery rotation while traveling the strait line
                        if (rotation > animatable.Rotation && rotation - animatable.Rotation > 5)
                            animatable.Rotation += 5;
                        else if (rotation < animatable.Rotation && animatable.Rotation - rotation > 5)
                            animatable.Rotation -= 5;
                        else
                            animatable.Rotation = rotation;
                    }
                    else
                    {
                        //if (animatable.RotateAlongPath && (int)(animatable.Rotation + animatable.Sprite.InitialRotationOffset) > 0)
                        //    animatable.Rotation -= animatable.RotatIntoPlaceSpeed;
                        //else if (animatable.RotateAlongPath && (int)(animatable.Rotation + animatable.Sprite.InitialRotationOffset) < 0)
                        //    animatable.Rotation += animatable.RotatIntoPlaceSpeed;
                        //else
                        //{
                            animatable.IsMoving = false;
                            animatable.ZIndex = 0;
                        //}
                    }
                }
                else if (animatable.CurPathPointIndex < 0)
                {
                    //this stops the animation
                    animatable.CurPathPointIndex = 0;
                    animatable.IsMoving = false;
                }
                else
                {
                    animatable.IsMoving = true;
                    if (animatable.RotateAlongPath && animatable.Speed != 0)
                        animatable.Rotation = bezierCurveService.GetRotationAngleAlongPath(animatable);
                }
            }

            CleanUpOffScreenAnimatables();
        }

        public void CleanUpOffScreenAnimatables()
        {
            Utils.dOut("Animatables", Animatables.Count);

            for(var i = 0; i <= Animatables.Count - 1; i++)
            {
                if (!Animatables[i].IsMoving
                && Animatables[i].Started
                && (Animatables[i].Location.X < 0 ||
                    Animatables[i].Location.Y < 0 ||
                    Animatables[i].Location.X > Constants.CanvasSize.Width ||
                    Animatables[i].Location.Y > Constants.CanvasSize.Height))
                {
                    (Animatables[i] as AnimatableBase).Dispose();
                    Animatables[i] = null;
                }
            }

            Animatables.RemoveAll(a => a == null);

        }

        public List<PointF> ComputePathPoints(BezierCurve path, bool pathisline=false,float granularity = .1F)
        {
            float pointgranularity = 1F; //the lower the more granular
            List<PointF> pathpoints = new List<PointF>();

            for (var percent = 0F; percent <= 100; percent += granularity)
            {
                PointF point;
                if (pathisline)
                    point = bezierCurveService.getLineXYatPercent(path, percent);
                else
                    point = bezierCurveService.getCubicBezierXYatPercent(path, percent);
                pathpoints.Add(point);
            }

            pathpoints = bezierCurveService.GetEvenlyDistributedPathPointsByLength(pathpoints, pointgranularity);

            return pathpoints;
        }

        public void ComputePathPoints()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            float pointgranularity = 1F; //the lower the more granular 
            
            foreach (var animatable in Animatables)
            {
                if (animatable.Paths != null)
                {
                    animatable.PathPoints = new List<PointF>();
                    foreach (BezierCurve path in animatable.Paths)
                    {
                        for (var percent = 0F; percent <= 100; percent+= .1F)
                        {
                            PointF point;
                            if (animatable.PathIsLine)
                                point = bezierCurveService.getLineXYatPercent(path, percent);
                            else
                                point = bezierCurveService.getCubicBezierXYatPercent(path, percent);
                            animatable.PathPoints.Add(point);
                        }
                    }
                    if (animatable.PathPoints.Count >0)
                        animatable.PathPoints = bezierCurveService.GetEvenlyDistributedPathPointsByLength(animatable.PathPoints, pointgranularity);
                }
            }
            Utils.dOut("ComputePathPoints", stopwatch.ElapsedMilliseconds);
        }

        public void Draw()
        {
            spriteService.DynamicCtx.BeginBatchAsync();

            ResetCanvas(spriteService.DynamicCtx);

            foreach (IAnimatable animatable in Animatables.Where(a=>a.Started).OrderByDescending(a=>a.ZIndex)) {

                spriteService.DrawSprite(
                    animatable.SpriteBankIndex == null ? animatable.Sprite : animatable.SpriteBank[(int)animatable.SpriteBankIndex],
                    animatable.Location,
                    (animatable.RotateAlongPath && animatable.IsMoving) ? animatable.Rotation : 0
                    );

                foreach (BezierCurve path in animatable.Paths.Where(a=>a.DrawPath==true))
                {
                    if (animatable.DrawPath)
                        bezierCurveService.DrawCurve(spriteService.DynamicCtx, path);
                    if (animatable.DrawControlLines)
                        bezierCurveService.DrawCurveControlLines(spriteService.DynamicCtx, path);
                }
            }

            spriteService.DynamicCtx.EndBatchAsync();
        }
    }
}
