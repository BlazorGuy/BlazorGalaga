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

            animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(BugFactory.BugIntro.TwoGroupsOfStackedEightFromTop));

            animationService.ComputePathPoints();

            Task.Delay(1000).ContinueWith((task) =>
            {
                //animationService.Animatables.Where(a => a.Index >= 8 && a.Index < 16).ToList().ForEach(bug => bug.Started = true);
                //animationService.Animatables.Where(a => a.Index >= 16 && a.Index < 32).ToList().ForEach(bug => bug.Started = true);
                animationService.Animatables.Where(a => a.Index <=8).ToList().ForEach(bug => {
                    bug.Started = true;
                    bug.Paths.ForEach(a => a.DrawPath = true);
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
