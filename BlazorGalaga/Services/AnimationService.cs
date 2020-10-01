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
                    if (animatable.CurPathPointIndex == 0 || linetolocationdistance <= animatable.Speed ||
                        (linetolocationdistance > animatable.LastLineToLocationDistance && !animatable.AllowNegativeSpeed))
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
                    {
                        //if we are at the last point in the line, stop the animation
                        if (animatable.PathPoints.Last() == new PointF(animatable.LineToLocation.X, animatable.LineToLocation.Y))
                        {
                            animatable.CurPathPointIndex += 1;
                            return;
                        }
                        //animatable.LineToLocation = new Vector2(animatable.LineToLocation.X + .01F, animatable.LineToLocation.Y + .01F);
                    }

                    Vector2 direction = Vector2.Normalize(animatable.LineToLocation - animatable.LineFromLocation);

                    var speed = 0;
                    if (animatable.VSpeed != null)
                    {
                        var vspeed = animatable.VSpeed.LastOrDefault(a => a.PathPointIndex <= animatable.CurPathPointIndex);
                        speed = vspeed == null ? animatable.Speed : vspeed.Speed;
                    }
                    else
                        speed = animatable.Speed;

                    //store prev, current, and next location
                    //this is used to calculate rotation later
                    animatable.PevLocation = animatable.Location;
                    animatable.Location = new PointF(animatable.Location.X + direction.X * speed, animatable.Location.Y + direction.Y * speed);
                    animatable.NextLocation = new PointF(animatable.Location.X + (direction.X * speed * 2) , animatable.Location.Y + (direction.Y * speed * 2));

                    animatable.IsMovingDown = animatable.NextLocation.Y - animatable.PevLocation.Y > 0;

                    if (animatable.ManualRotation != 0)
                    {
                        animatable.Rotation = animatable.ManualRotation;
                    }
                    else if (animatable.RotateAlongPath)
                    {
                        var rotation = bezierCurveService.GetRotationAngleAlongPath(animatable);
                        animatable.Rotation = rotation;
                    }

                    loopcount++;
                }
                else if (animatable.PathPoints.Count > 0)
                {
                    animatable.CurPathPointIndex = 0;
                    animatable.PathPoints.Clear();
                    animatable.Paths.Clear();
                    animatable.IsMoving = false;
                }
                else if (animatable.RotateWhileStill)
                {
                    if (animatable.ManualRotationRate != 0)
                        animatable.Rotation += animatable.ManualRotationRate;
                    else if(animatable.ManualRotation != 0)
                        animatable.Rotation = animatable.ManualRotation;
                }

                if ((animatable as Bug) != null && (animatable as Bug).OutputDebugInfo)
                {
                    Utils.dOut("*** Animate Debug: *** ", "<br/><br/> CurPathPointIndex: " + animatable.CurPathPointIndex +
                                    "<br/> PathPoints: " + animatable.PathPoints.Count +
                                    "<br/> LineFromLocation: " + animatable.LineFromLocation.X + "," + animatable.LineFromLocation.Y +
                                    "<br/> LineToLocation: " + animatable.LineToLocation.X + "," + animatable.LineToLocation.Y +
                                    "<br/> speed: " + animatable.Speed +
                                    "<br/> CaptureState: " + ((Bug)animatable).CaptureState.ToString() +
                                    "<br/> location: " + animatable.Location +
                                    "<br/> IsMoving: " + animatable.IsMoving +
                                    "<br/> loopcount: " + loopcount +
                                    "<br/> RotateWhileStill: " + animatable.RotateWhileStill +
                                    "<br/> ManualRotation: " + animatable.ManualRotation +
                                    "<br/> Rotation: " + animatable.Rotation +
                                    "<br/> ManualRotationRate: " + animatable.ManualRotationRate +
                                    "<br/> startpoint: " + (animatable.Paths == null || animatable.Paths.Count==0 ? "NA" : animatable.Paths.First().StartPoint.ToString()) +
                                    "<br/> endpoint: " + (animatable.Paths == null || animatable.Paths.Count == 0 ? "NA" : animatable.Paths.Last().EndPoint.ToString()) +
                                    "<br/><br/>*** End Animate Debug ***");
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

        public List<PointF> ComputePathPoints(BezierCurve path, bool pathisline=false,int granularity=10)
        {
            var cachedPath = PathCaches.FirstOrDefault(a => a.Path.StartPoint.Equals(path.StartPoint) &&
                                                        a.Path.ControlPoint1.Equals(path.ControlPoint1) &&
                                                        a.Path.ControlPoint2.Equals(path.ControlPoint2) &&
                                                        a.Path.EndPoint.Equals(path.EndPoint));

            if (cachedPath != null && !pathisline)
                return cachedPath.PathPoints;

            List<PointF> pathpoints = new List<PointF>();

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

            pathpoints = bezierCurveService.GetEvenlyDistributedPathPointsByLength(pathpoints, 5);

            PathCaches.Add(new PathCache() { Path = path, PathPoints = pathpoints });

            return pathpoints;
        }

        public void Draw()
        {
            spriteService.DynamicCtx1.BeginBatchAsync();

            ResetCanvas(spriteService.DynamicCtx1);

            Utils.dOut("Animatables", Animatables.Count);

            foreach (IAnimatable animatable in Animatables.Where(a => a.Started && a.Visible).OrderByDescending(a => a.ZIndex))
            {

                spriteService.DrawSprite(
                    animatable.SpriteBankIndex == null || animatable.SpriteBank == null || animatable.SpriteBank.Count == 0 ? animatable.Sprite : animatable.SpriteBank[(int)animatable.SpriteBankIndex],
                    animatable.Location,
                    ((animatable.RotateAlongPath && animatable.IsMoving) || animatable.RotateWhileStill) ? animatable.Rotation : 0
                    );

                //if(animatable as Bug !=null && (animatable as Bug).Tag == "capturedship")
                //{
                //    Utils.dOut("Draw Debug: ", "<br/> ppi: " + animatable.CurPathPointIndex +
                //                                  "<br/> pp: " + animatable.PathPoints.Count +
                //                                  "<br/> lfl: " + animatable.LineFromLocation.X + "," + animatable.LineFromLocation.Y +
                //                                  "<br/> ltl: " + animatable.LineToLocation.X + "," + animatable.LineToLocation.Y +
                //                                  "<br/> speed: " + animatable.Speed +
                //                                  "<br/> location: " + animatable.Location +
                //                                  "<br/> loopcount: " + loopcount +
                //                                  "<br/> End Animate Debug");
                //}

                if ((animatable.DrawPath || animatable.DrawControlLines) && !animatable.PathDrawn)
                {
                    animatable.PathDrawn = true;
                    spriteService.StaticCtx.BeginBatchAsync();
                    spriteService.StaticCtx.SetStrokeStyleAsync("white");
                    spriteService.StaticCtx.SetFillStyleAsync("yellow");
                    spriteService.StaticCtx.SetFontAsync("48px serif");
                    spriteService.StaticCtx.SetLineWidthAsync(2);
                    foreach (BezierCurve path in animatable.Paths.Where(a => a.DrawPath == true))
                    {
                        bezierCurveService.DrawPathPoints(spriteService.StaticCtx, animatable.PathPoints);
                        if (animatable.DrawPath)
                            bezierCurveService.DrawCurve(spriteService.StaticCtx, path);
                        if (animatable.DrawControlLines)
                            bezierCurveService.DrawCurveControlLines(spriteService.StaticCtx, path);
                    }
                    spriteService.StaticCtx.EndBatchAsync();
                }
            }

            spriteService.DynamicCtx1.EndBatchAsync();
        }
    }
}
