using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;
using BlazorGalaga.Models.Paths.Challenges;
using BlazorGalaga.Services;

namespace BlazorGalaga.Static.Levels
{
    public static class Level5
    {
        public static void InitIntro(AnimationService animationService)
        {
            //two groups of four from top
            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug,1,i==4));
            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing, new Intro2(), Sprite.SpriteTypes.RedBug,1,i==4));


            //two groups of stacked eight from bottom
            for (int i = 1; i < 10; i += 2)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 8, (i * Constants.BugIntroSpacing) / 2, new Intro3(), Sprite.SpriteTypes.GreenBug, 2, i == 9));

            for (int i = 0; i < 11; i += 2)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 8, (i * Constants.BugIntroSpacing) / 2, new Intro5(), Sprite.SpriteTypes.RedBug, 2, i == 10));

            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Intro4(), Sprite.SpriteTypes.RedBug, 3, i == 4));

            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 20, i * Constants.BugIntroSpacing, new Intro6(), Sprite.SpriteTypes.RedBug, 3, i == 4));

            //two groups of stacked eight from top
            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Intro2(), Sprite.SpriteTypes.BlueBug, 4, i == 4));

            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 28, i * Constants.BugIntroSpacing, new Intro7(), Sprite.SpriteTypes.BlueBug, 4, i == 4));

            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug, 5, i == 4));

            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 36, i * Constants.BugIntroSpacing, new Intro8(), Sprite.SpriteTypes.BlueBug, 5, i == 4));
        }
    }
}
