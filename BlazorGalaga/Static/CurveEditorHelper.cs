using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Services;
using Microsoft.AspNetCore.Components;
using static BlazorGalaga.Pages.Index;
using BlazorGalaga.Models;
using System.Linq;

namespace BlazorGalaga.Static
{
    public static class CurveEditorHelper
    {
        private static bool isDraggingCurve;

        public static void ResetAnimation(AnimationService animationService)
        {
            foreach (var animatable in animationService.Animatables.Where(a => a.Sprite.SpriteType != Sprite.SpriteTypes.Ship))
            {
                animatable.CurPathPointIndex = animatable.StartDelay;
            }
        }

        public static void DisableLines(AnimationService animationService)
        {
            foreach (var animatable in animationService.Animatables)
            {
                animatable.DrawControlLines = false;
                animatable.DrawPath = false;
            }
        }

        public static void EditCurves(AnimationService animationService, GameLoopObject glo)
        {
            foreach (var animatable in animationService.Animatables.Where(a => a.Sprite.SpriteType != Sprite.SpriteTypes.Ship))
            {
                animatable.DrawControlLines = true;
                animatable.DrawPath = true;
            }

            if (glo.addpath)
            {
                animationService.Animatables[0].Paths.Add(new BezierCurve()
                {
                    StartPoint = new PointF(10, 10),
                    EndPoint = new PointF(10, 100),
                    ControlPoint1 = new PointF(10, 50),
                    ControlPoint2 = new PointF(30, 30)
                });
            }

            if (MouseHelper.MouseIsDown)
            {
                foreach (var animatable in animationService.Animatables.Where(a => a.Sprite.SpriteType != Sprite.SpriteTypes.Ship))
                {
                    foreach (var path in animatable.Paths)
                    {
                        if (Utils.GetDistance(MouseHelper.Position, path.StartPoint) <= 5) path.StartPointDragged = true;
                        else if (Utils.GetDistance(MouseHelper.Position, path.EndPoint) <= 5) path.EndPointDragged = true;
                        else if (Utils.GetDistance(MouseHelper.Position, path.ControlPoint1) <= 5) path.ControlPoint1Dragged = true;
                        else if (Utils.GetDistance(MouseHelper.Position, path.ControlPoint2) <= 5) path.ControlPoint2Dragged = true;

                        if (path.StartPointDragged)
                        {
                            path.StartPoint = MouseHelper.Position;
                            isDraggingCurve = true;
                            break;
                        }
                        else if (path.EndPointDragged)
                        {
                            path.EndPoint = MouseHelper.Position;
                            isDraggingCurve = true;
                            break;
                        }
                        else if (path.ControlPoint1Dragged)
                        {
                            path.ControlPoint1 = MouseHelper.Position;
                            isDraggingCurve = true;
                            break;
                        }
                        else if (path.ControlPoint2Dragged)
                        {
                            path.ControlPoint2 = MouseHelper.Position;
                            isDraggingCurve = true;
                            break;
                        }
                    }
                }
            }
            else if (isDraggingCurve)
            {
                isDraggingCurve = false;
                animationService.ComputePathPoints();
                string curvedata = "";
                foreach (var animatable in animationService.Animatables.Where(a => a.Sprite.SpriteType != Sprite.SpriteTypes.Ship))
                {
                    foreach (var path in animatable.Paths)
                    {
                        path.StartPointDragged = false;
                        path.EndPointDragged = false;
                        path.ControlPoint1Dragged = false;
                        path.ControlPoint2Dragged = false;
                        curvedata += "<br/>paths.Add(new BezierCurve() {" +
                            "StartPoint = new PointF(" + path.StartPoint.X + "F," + path.StartPoint.Y + "F),<br/>" +
                            "ControlPoint1 = new PointF(" + path.ControlPoint1.X + "F," + path.ControlPoint1.Y + "F),<br/>" +
                            "ControlPoint2 = new PointF(" + path.ControlPoint2.X + "F," + path.ControlPoint2.Y + "F),<br/>" +
                            "EndPoint = new PointF(" + path.EndPoint.X + "F," + path.EndPoint.Y + "F)});<br/>";
                    }
                }
                Utils.dOut("CurveData", curvedata);
            }
        }

    }
}
