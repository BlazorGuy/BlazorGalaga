using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;

namespace BlazorGalaga.Static
{
    public static class BugFactory
    {
        public static List<IAnimatable> CreateAnimation_BugIntro1()
        {
            List<IAnimatable> animatables = new List<IAnimatable>();

            for (int i = 0; i < 4; i++)
            {
                animatables.Add(CreateAnimatable_BugIntro1(i*60));
            }

            return animatables;
        }

        public static IAnimatable CreateAnimatable_BugIntro1(int startdelay)
        {

            var bug = new Bug()
            {
                Paths = Intro1.GetPaths(),
                //DrawPath = true,
                //DrawControlLines = true,
                RotateAlongPath = true,
                Speed=10,
                LoopBack = true,
                StartDelay = startdelay
            };

            return bug;

        }
    }
}
