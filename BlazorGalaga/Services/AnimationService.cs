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
using System.Numerics;
using System.Net;

namespace BlazorGalaga.Services
{
    public class AnimationService
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public List<IAnimatable> Animatables = new List<IAnimatable>();

        private BezierCurveService bezierCurveService;
        private SpriteService spriteService;
        private List<PathCache> PathCaches;

        public AnimationService(BezierCurveService bcs, SpriteService ss)
        {
            bezierCurveService = bcs;
            spriteService = ss;

            PathCaches = new List<PathCache>();
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

                if (animatable.CurPathPointIndex - 1 > 0 && animatable.CurPathPointIndex - 1 < animatable.PathPoints.Count)
                    animatable.PevLocation = animatable.PathPoints[animatable.CurPathPointIndex - 1];

                if (animatable.CurPathPointIndex > 0 && animatable.CurPathPointIndex  < animatable.PathPoints.Count)
                    animatable.Location = animatable.PathPoints[animatable.CurPathPointIndex];

                if (animatable.CurPathPointIndex + 1 > 0 && animatable.CurPathPointIndex + 1 < animatable.PathPoints.Count)
                    animatable.NextLocation = animatable.PathPoints[animatable.CurPathPointIndex + 1];

                animatable.CurPathPointIndex += animatable.Speed;

                //are we at the end of the last path?
                if (animatable.CurPathPointIndex > animatable.PathPoints.Count - 1)
                {
                    //stop the curve animation
                    animatable.CurPathPointIndex = animatable.PathPoints.Count - 1;
                    //do we have an extra lineto to animate?
                    if (animatable.IsMoving && !animatable.PathIsLine &&
                        (Vector2.Distance(animatable.LineFromLocation,new Vector2(animatable.Location.X, animatable.Location.Y)) + animatable.Speed < animatable.LineToLocationDistance ||
                        animatable.LineToLocationDistance == 0))
                    {
                        //lineto animation
                        animatable.LineToLocationDistance = Vector2.Distance(animatable.LineFromLocation, animatable.LineToLocation);
                        Vector2 direction = Vector2.Normalize(animatable.LineToLocation - animatable.LineFromLocation);

                        animatable.PevLocation = new PointF(animatable.Location.X + direction.X, animatable.Location.Y + direction.Y);
                        animatable.Location = new PointF(animatable.Location.X + direction.X * animatable.Speed, animatable.Location.Y + direction.Y * animatable.Speed);
                        animatable.NextLocation = new PointF(animatable.Location.X + direction.X * (animatable.Speed * 2), animatable.Location.Y + direction.Y * (animatable.Speed * 2));

                        animatable.PathPoints.Clear();
                        animatable.IsMoving = true;

                        animatable.Rotation = bezierCurveService.GetRotationAngleAlongPath(animatable);
                    }
                    else
                    {
                        //animation is complete
                        if (!animatable.PathIsLine)
                            animatable.Location = new PointF(animatable.LineToLocation.X,animatable.LineToLocation.Y);
                        animatable.IsMoving = false;
                        animatable.ZIndex = 0;
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
                    //animation continues
                    animatable.IsMoving = true;
                    if (animatable.RotateAlongPath && animatable.Speed != 0)
                        animatable.Rotation = bezierCurveService.GetRotationAngleAlongPath(animatable);
                }
            }

            CleanUpOffScreenAnimatables();
        }

        public void CleanUpOffScreenAnimatables()
        {
            for(var i = 0; i <= Animatables.Count - 1; i++)
            {
                if ((!Animatables[i].IsMoving
                && Animatables[i].Started
                && (Animatables[i].Location.X < 0 ||
                    Animatables[i].Location.Y < 0 ||
                    Animatables[i].Location.X > Constants.CanvasSize.Width ||
                    Animatables[i].Location.Y > Constants.CanvasSize.Height)) ||
                    (!Animatables[i].IsMoving && Animatables[i].DestroyAfterComplete) ||
                    (Animatables[i].DestroyImmediately))
                {
                    (Animatables[i] as AnimatableBase).Dispose();
                    Animatables[i] = null;
                }
            }

            Animatables.RemoveAll(a => a == null);

        }

        public List<PointF> ComputePathPoints(BezierCurve path, bool pathisline=false,float granularity = .1F)
        {
            var cachedPath = PathCaches.FirstOrDefault(a => a.Path.StartPoint == path.StartPoint &&
                                                        a.Path.ControlPoint1 == path.ControlPoint1 &&
                                                        a.Path.ControlPoint2 == path.ControlPoint2 &&
                                                        a.Path.EndPoint == path.EndPoint);

            if (cachedPath != null && !pathisline) return cachedPath.PathPoints;

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

            PathCaches.Add(new PathCache() { Path = path, PathPoints = pathpoints });

            return pathpoints;
        }

        public void ComputePathPoints()
        {
           
            foreach (var animatable in Animatables)
            {
                if (animatable.Paths != null)
                {
                    animatable.PathPoints = new List<PointF>();
                    foreach (BezierCurve path in animatable.Paths)
                       animatable.PathPoints.AddRange(ComputePathPoints(path, animatable.PathIsLine));
                }
            }

        }

        public void Draw()
        {
            spriteService.DynamicCtx1.BeginBatchAsync();
            //spriteService.DynamicCtx2.BeginBatchAsync();

            ResetCanvas(spriteService.DynamicCtx1);
            //ResetCanvas(spriteService.DynamicCtx2);

            foreach (IAnimatable animatable in Animatables.Where(a => a.Started).OrderByDescending(a => a.ZIndex))
            {
                if(animatable.Sprite.SpriteType == Sprite.SpriteTypes.EnemyExplosion1)
                {
                    Utils.dOut("exp", animatable.SpriteBankIndex);
                }

                spriteService.DrawSprite(
                    animatable.SpriteBankIndex == null ? animatable.Sprite : animatable.SpriteBank[(int)animatable.SpriteBankIndex],
                    animatable.Location,
                    (animatable.RotateAlongPath && animatable.IsMoving) ? animatable.Rotation : 0
                    );

                //foreach (BezierCurve path in animatable.Paths.Where(a=>a.DrawPath==true))
                //{
                //    if (animatable.DrawPath)
                //        bezierCurveService.DrawCurve(spriteService.DynamicCtx, path);
                //    if (animatable.DrawControlLines)
                //        bezierCurveService.DrawCurveControlLines(spriteService.DynamicCtx, path);
                //}
            }

            spriteService.DynamicCtx1.EndBatchAsync();
            //spriteService.DynamicCtx2.EndBatchAsync();
        }
    }
}
