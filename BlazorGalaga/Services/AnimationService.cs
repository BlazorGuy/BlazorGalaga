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
        public List<PathCache> PathCaches;

        private readonly BezierCurveService bezierCurveService;
        private readonly SpriteService spriteService;

        public AnimationService(BezierCurveService bcs, SpriteService ss)
        {
            bezierCurveService = bcs;
            spriteService = ss;

            PathCaches = new List<PathCache>();
        }

        public void ResetCanvas(Canvas2DContext ctx)
        {
            if (spriteService.IsRotated)
                ctx.SetTransformAsync(1, 0, 0, 1, 0, 0);
            ctx.ClearRectAsync(0, 0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
        }

        private int loopcount = 0;

        public void Animate()
        {
            foreach (IAnimatable animatable in Animatables.Where(a => a.Started))
            {
                if (animatable.CurPathPointIndex <= animatable.PathPoints.Count - 1)
                {
                    //if the next path isn't connected to the current path then jump to the next path
                    if (animatable.PathPoints[animatable.CurPathPointIndex].Equals(new PointF(-999, -999)))
                    {
                        animatable.Location = new PointF(animatable.LineToLocation.X,animatable.LineToLocation.Y);
                        animatable.CurPathPointIndex += 1;
                    }

                    //store how far the animatable is to the next destination point
                    var linetolocationdistance = Vector2.Distance(new Vector2(animatable.Location.X, animatable.Location.Y), animatable.LineToLocation);

                    //if the animatable is just starting, or it is close enough or overshot the destination
                    //then move it to the next line segment in the path
                    if (animatable.CurPathPointIndex == 0 || linetolocationdistance <= animatable.Speed || linetolocationdistance > animatable.LastLineToLocationDistance)
                    {
                        animatable.Location = animatable.PathPoints[animatable.CurPathPointIndex];

                        //are we at the end of the path?
                        if (animatable.CurPathPointIndex >= animatable.PathPoints.Count - 1)
                        {
                            //this stops the animation
                            animatable.CurPathPointIndex += 1;
                            return;
                        }

                        animatable.LineFromLocation = new Vector2(animatable.Location.X, animatable.Location.Y);
                        animatable.LineToLocation = new Vector2(animatable.PathPoints[animatable.CurPathPointIndex + 1].X, animatable.PathPoints[animatable.CurPathPointIndex + 1].Y);
                        animatable.CurPathPointIndex += 1;
                        animatable.IsMoving = true;
                    }

                    animatable.LastLineToLocationDistance = Vector2.Distance(new Vector2(animatable.Location.X, animatable.Location.Y), animatable.LineToLocation);

                    //if lineto == linefrom then make lineto slightly bigger so we don't hang up
                    if (animatable.LineToLocation.X == animatable.LineFromLocation.X && animatable.LineToLocation.Y == animatable.LineFromLocation.Y)
                        animatable.LineToLocation = new Vector2(animatable.LineToLocation.X + .01F, animatable.LineToLocation.Y + .01F);

                    Vector2 direction = Vector2.Normalize(animatable.LineToLocation - animatable.LineFromLocation);

                    //store prev, current, and next location
                    //this is used to calculate rotation later
                    animatable.PevLocation = new PointF(animatable.Location.X + direction.X, animatable.Location.Y + direction.Y);
                    animatable.Location = new PointF(animatable.Location.X + direction.X * animatable.Speed, animatable.Location.Y + direction.Y * animatable.Speed);
                    animatable.NextLocation = new PointF(animatable.Location.X + direction.X * (animatable.Speed * 2), animatable.Location.Y + direction.Y * (animatable.Speed * 2));

                    if (animatable.RotateAlongPath)
                        animatable.Rotation = bezierCurveService.GetRotationAngleAlongPath(animatable);

                     loopcount++;

                    //if ((animatable as Bug) != null && (animatable as Bug).Tag != null && ((animatable as Bug).Tag.IndexOf("Dive") != -1))
                    //{
                    //    Utils.dOut("Animate Debug: ", "<br/> ppi: " + animatable.CurPathPointIndex +
                    //                                  "<br/> pp: " + animatable.PathPoints.Count +
                    //                                  "<br/> lfl: " + animatable.LineFromLocation.X + "," + animatable.LineFromLocation.Y +
                    //                                  "<br/> ltl: " + animatable.LineToLocation.X + "," + animatable.LineToLocation.Y +
                    //                                  "<br/> direction: " + direction.X + "," + direction.Y +
                    //                                  "<br/> speed: " + animatable.Speed +
                    //                                  "<br/> location: " + animatable.Location +
                    //                                  "<br/> loopcount: " + loopcount +
                    //                                  "<br/> End Animate Debug");
                    //}
                }
                else if (animatable.PathPoints.Count > 0)
                {
                    animatable.CurPathPointIndex = 0;
                    animatable.PathPoints.Clear();
                    animatable.Paths.Clear();
                    animatable.IsMoving = false;
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

        public List<PointF> ComputePathPoints(BezierCurve path, bool pathisline=false)
        {
            var cachedPath = PathCaches.FirstOrDefault(a => a.Path.StartPoint.Equals(path.StartPoint) &&
                                                        a.Path.ControlPoint1.Equals(path.ControlPoint1) &&
                                                        a.Path.ControlPoint2.Equals(path.ControlPoint2) &&
                                                        a.Path.EndPoint.Equals(path.EndPoint));

            if (cachedPath != null && !pathisline)
                return cachedPath.PathPoints;

            List<PointF> pathpoints = new List<PointF>();
            var granularity = 10;

            if (path.BreakPath)
                pathpoints.Add(new PointF(-999, -999));

            for (var percent = 0F; percent <= 100; percent += granularity)
            {
                PointF point;
                if (pathisline)
                    point = bezierCurveService.getLineXYatPercent(path, percent);
                else
                    point = bezierCurveService.getCubicBezierXYatPercent(path, percent);
                pathpoints.Add(point);
            }

            PathCaches.Add(new PathCache() { Path = path, PathPoints = pathpoints });

            return pathpoints;
        }

        public void Draw()
        {
            spriteService.DynamicCtx1.BeginBatchAsync();
            //spriteService.DynamicCtx2.BeginBatchAsync();

            ResetCanvas(spriteService.DynamicCtx1);
            //ResetCanvas(spriteService.DynamicCtx2);

            Utils.dOut("Animatables", Animatables.Count);

            foreach (IAnimatable animatable in Animatables.Where(a => a.Started && a.Visible).OrderByDescending(a => a.ZIndex))
            {
                spriteService.DrawSprite(
                    animatable.SpriteBankIndex == null ? animatable.Sprite : animatable.SpriteBank[(int)animatable.SpriteBankIndex],
                    animatable.Location,
                    (animatable.RotateAlongPath && animatable.IsMoving) ? animatable.Rotation : 0
                    );

                if (animatable.DrawPath || animatable.DrawControlLines)
                {
                    foreach (BezierCurve path in animatable.Paths.Where(a => a.DrawPath == true))
                    {
                        bezierCurveService.DrawPathPoints(spriteService.DynamicCtx1, animatable.PathPoints);
                        if (animatable.DrawPath)
                            bezierCurveService.DrawCurve(spriteService.DynamicCtx1, path);
                        if (animatable.DrawControlLines)
                            bezierCurveService.DrawCurveControlLines(spriteService.DynamicCtx1, path);
                    }
                }
            }

            spriteService.DynamicCtx1.EndBatchAsync();
            //spriteService.DynamicCtx2.EndBatchAsync();
        }
    }
}
