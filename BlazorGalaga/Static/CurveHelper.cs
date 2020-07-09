using System;
using System.Collections.Generic;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorGalaga.Static
{
    public static class CurveHelper
    {
        private static bool isDraggingCurve;

        public static void EditCurves(AnimationService animationService)
        {
            if (MouseHelper.MouseIsDown)
            {
                foreach (var animatable in animationService.Animatables)
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
                foreach (var animatable in animationService.Animatables)
                {
                    foreach (var path in animatable.Paths)
                    {
                        path.StartPointDragged = false;
                        path.EndPointDragged = false;
                        path.ControlPoint1Dragged = false;
                        path.ControlPoint2Dragged = false;
                    }
                }
            }
        }

    }
}
