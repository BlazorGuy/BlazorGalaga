using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers.Levels
{
    public static class Level2
    {
        public static void Init(AnimationService animationService)
        {
            //creates two top down bug lines of 4 each, these enter at the same time
            animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(1));
            //creates two side bug lines of 8 each, these enter one after the other
            animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(2));
            //creates two top down bug lines of 8 each, these enter one after the other
            animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(3));
            animationService.ComputePathPoints();

            //move in 2 sets of 4 (red and blue) from the top at the same time
            //Task.Delay(2000).ContinueWith((task) =>
            //{
            //    animationService.Animatables.Where(a => a.Index < 8).ToList().ForEach(a => a.Started = true);
            //});
            //move in 1 set of 8 red and green bugs from the bottom left
            Task.Delay(1000).ContinueWith((task) =>
            {
                var i = 0;
                var offset = 46;
                animationService.Animatables.Where(a => a.Index >= 8 && a.Index < 16).ToList().ForEach(a => {
                    a.StartDelay = i * Constants.BugIntroSpacing;
                    if (a.Sprite.SpriteType == Sprite.SpriteTypes.RedBug)
                    {
                        a.Paths[1].StartPoint = new PointF(a.Paths[1].StartPoint.X, a.Paths[1].StartPoint.Y - offset);
                        a.Paths[1].ControlPoint1 = new PointF(a.Paths[1].ControlPoint1.X, a.Paths[1].ControlPoint1.Y - offset);
                        a.Paths[1].ControlPoint2 = new PointF(a.Paths[1].ControlPoint2.X - offset, a.Paths[1].ControlPoint2.Y);
                        a.Paths[1].EndPoint = new PointF(a.Paths[1].EndPoint.X - offset, a.Paths[1].EndPoint.Y);

                        a.Paths[2].StartPoint = new PointF(a.Paths[2].StartPoint.X - offset, a.Paths[2].StartPoint.Y);
                        a.Paths[2].ControlPoint1 = new PointF(a.Paths[2].ControlPoint1.X - offset, a.Paths[2].ControlPoint1.Y + offset);
                        a.Paths[2].ControlPoint2 = new PointF(a.Paths[2].ControlPoint2.X + offset, a.Paths[2].ControlPoint2.Y + offset);
                        a.Paths[2].EndPoint = new PointF(a.Paths[2].EndPoint.X + offset, a.Paths[2].EndPoint.Y);

                        a.Paths[3].StartPoint = new PointF(a.Paths[3].StartPoint.X + offset, a.Paths[3].StartPoint.Y);
                        a.Paths[3].ControlPoint1 = new PointF(a.Paths[3].ControlPoint1.X + offset, a.Paths[3].ControlPoint1.Y - offset);
                        a.Paths[3].ControlPoint2 = new PointF(a.Paths[3].ControlPoint2.X, a.Paths[3].ControlPoint2.Y - offset);
                        a.Paths[3].EndPoint = new PointF(a.Paths[3].EndPoint.X - offset, a.Paths[3].EndPoint.Y - offset);

                        a.PathPoints.Clear();
                        a.Paths.ForEach(b =>
                        {
                            b.DrawPath = true;
                            a.PathPoints.AddRange(animationService.ComputePathPoints(b, false, .1F, true));
                        });
                        a.Speed = Constants.BugIntroSpeed - 1;
                    }
                    else
                    {
                        i++;
                    }
                    a.Paths.RemoveAt(0);
                    a.Paths.ForEach(b => {
                        b.DrawPath = true;
                    });
                    a.DrawPath = true;
                    a.Started = true;
                });
            });
            ////move in 1 set of 8 red bugs from the bottom right
            //Task.Delay(9000).ContinueWith((task) =>
            //{
            //    animationService.Animatables.Where(a => a.Index >= 16 && a.Index < 24).ToList().ForEach(a => a.Started = true);
            //});
            ////move in 1 set of 8 blue bugs from the top left
            //Task.Delay(14000).ContinueWith((task) =>
            //{
            //    animationService.Animatables.Where(a => a.Index >= 24 && a.Index < 32).ToList().ForEach(a => a.Started = true);
            //});
            ////move in 1 set of 8 blue bugs from the top right
            //Task.Delay(18000).ContinueWith((task) =>
            //{
            //    animationService.Animatables.Where(a => a.Index >= 32 && a.Index < 40).ToList().ForEach(a => a.Started = true);
            //});
        }
    }
}
