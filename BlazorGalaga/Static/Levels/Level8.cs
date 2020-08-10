using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;
using BlazorGalaga.Models.Paths.Challenges.Challenge2;
using BlazorGalaga.Services;

namespace BlazorGalaga.Static.Levels
{
    public static class Level8
    {
        public static void InitIntro(AnimationService animationService, int introspeedincrease)
        {
            //two groups of four from top
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i, i * Constants.BugIntroSpacing, new Challenge1(), Sprite.SpriteTypes.RedBug,1, introspeedincrease));
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing, new Challenge2(), Sprite.SpriteTypes.RedBug, 1, introspeedincrease));

            //two groups of four from bottom
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 8, i * Constants.BugIntroSpacing, new Challenge3(), Sprite.SpriteTypes.GreenBug, 2, introspeedincrease));
            //for (int i = 0; i < 4; i++)
            //    animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Intro4(), Sprite.SpriteTypes.RedBug, 3, introspeedincrease));

            ////two groups of eight from top
            //for (int i = 0; i < 9; i++)
            //    animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug, 4, introspeedincrease, i==8));
            //for (int i = 0; i < 9; i++)
            //    animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Intro2(), Sprite.SpriteTypes.BlueBug, 5, introspeedincrease, i==8));
        }
    }
}
