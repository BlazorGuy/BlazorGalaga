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
                animationService.Animatables.Where(a => a.Index >= 8 && a.Index < 16).ToList().ForEach(bug => {
                    bug.StartDelay = i * Constants.BugIntroSpacing;
                    if (bug.Sprite.SpriteType == Sprite.SpriteTypes.RedBug)
                    {
                        bug.Paths[0].StartPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - offset);
                        bug.Paths[0].ControlPoint1 = new PointF(bug.Paths[0].ControlPoint1.X, bug.Paths[0].ControlPoint1.Y - offset);
                        bug.Paths[0].ControlPoint2 = new PointF(bug.Paths[0].ControlPoint2.X, bug.Paths[0].ControlPoint2.Y - offset);
                        bug.Paths[0].EndPoint = new PointF(bug.Paths[1].StartPoint.X, bug.Paths[1].StartPoint.Y - offset);

                        bug.Paths[1].StartPoint = new PointF(bug.Paths[1].StartPoint.X, bug.Paths[1].StartPoint.Y - offset);
                        bug.Paths[1].ControlPoint1 = new PointF(bug.Paths[1].ControlPoint1.X, bug.Paths[1].ControlPoint1.Y - offset);
                        bug.Paths[1].ControlPoint2 = new PointF(bug.Paths[1].ControlPoint2.X - offset, bug.Paths[1].ControlPoint2.Y);
                        bug.Paths[1].EndPoint = new PointF(bug.Paths[1].EndPoint.X - offset, bug.Paths[1].EndPoint.Y);

                        bug.Paths[2].StartPoint = new PointF(bug.Paths[2].StartPoint.X - offset, bug.Paths[2].StartPoint.Y);
                        bug.Paths[2].ControlPoint1 = new PointF(bug.Paths[2].ControlPoint1.X - offset, bug.Paths[2].ControlPoint1.Y + offset);
                        bug.Paths[2].ControlPoint2 = new PointF(bug.Paths[2].ControlPoint2.X + offset, bug.Paths[2].ControlPoint2.Y + offset);
                        bug.Paths[2].EndPoint = new PointF(bug.Paths[2].EndPoint.X + offset, bug.Paths[2].EndPoint.Y);

                        bug.Paths[3].StartPoint = new PointF(bug.Paths[3].StartPoint.X + offset, bug.Paths[3].StartPoint.Y);
                        bug.Paths[3].ControlPoint1 = new PointF(bug.Paths[3].ControlPoint1.X + offset, bug.Paths[3].ControlPoint1.Y - offset);
                        bug.Paths[3].ControlPoint2 = new PointF(bug.Paths[3].ControlPoint2.X, bug.Paths[3].ControlPoint2.Y - offset);
                        bug.Paths[3].EndPoint = new PointF(bug.Paths[3].EndPoint.X - offset, bug.Paths[3].EndPoint.Y - offset);

                        bug.VSpeed = new List<VSpeed>();
                        bug.VSpeed.Add(new VSpeed()
                        {
                            PathPointIndex = 1000,
                            Speed = Constants.BugIntroSpeed - 2
                        });
                        bug.VSpeed.Add(new VSpeed()
                        {
                            PathPointIndex = 1400,
                            Speed = Constants.BugIntroSpeed - 3
                        });;
                        bug.PathPoints.Clear();
                        bug.Paths.ForEach(b =>
                        {
                            b.DrawPath = true;
                            bug.PathPoints.AddRange(animationService.ComputePathPoints(b));
                        });
                    }
                    else
                    {
                        i++;
                    }
                    bug.Paths.ForEach(b => {
                        b.DrawPath = true;
                    });
                    bug.DrawPath = true;
                    bug.Started = true;
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
