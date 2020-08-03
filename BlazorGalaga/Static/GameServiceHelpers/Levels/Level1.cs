using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers.Levels
{
    public static class Level1
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
            Task.Delay(2000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index < 8).ToList().ForEach(a => a.Started = true);
            });
            //move in 1 set of 8 red and green bugs from the bottom left
            Task.Delay(5000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index >= 8 && a.Index < 16).ToList().ForEach(a => a.Started = true);
            });
            //move in 1 set of 8 red bugs from the bottom right
            Task.Delay(9000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index >= 16 && a.Index < 24).ToList().ForEach(a => a.Started = true);
            });
            //move in 1 set of 8 blue bugs from the top left
            Task.Delay(14000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index >= 24 && a.Index < 32).ToList().ForEach(a => a.Started = true);
            });
            //move in 1 set of 8 blue bugs from the top right
            Task.Delay(18000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index >= 32 && a.Index < 40).ToList().ForEach(a => a.Started = true);
            });
        }
    }
}
