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
    public static class Level3
    {
        public static void InitIntro(AnimationService animationService)
        {
            //two groups of four from top
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i, i * Constants.BugIntroSpacing, new Challenge1(), Sprite.SpriteTypes.BlueBug,1));
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing, new Challenge2(), Sprite.SpriteTypes.BlueBug,1));

            //two groups of eight from bottom
            for (int i = 0; i < 8; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 8, i * Constants.BugIntroSpacing, new Challenge3(), i % 2 != 0 ? Sprite.SpriteTypes.GreenBug : Sprite.SpriteTypes.BlueBug,2));
            for (int i = 0; i < 8; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Challenge4(), Sprite.SpriteTypes.BlueBug,3));

            //two groups of eight from top
            for (int i = 0; i < 8; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Challenge5(), Sprite.SpriteTypes.BlueBug,4));
            for (int i = 0; i < 8; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Challenge6(), Sprite.SpriteTypes.BlueBug,5));

        }
    }
}
