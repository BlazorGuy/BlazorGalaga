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
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 12, i * Constants.BugIntroSpacing, new Challenge4(), Sprite.SpriteTypes.RedBug, 2, introspeedincrease));

            //two groups of four from bottom
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Challenge3(), Sprite.SpriteTypes.RedBug, 3, introspeedincrease));
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 20, i * Constants.BugIntroSpacing, new Challenge4(), Sprite.SpriteTypes.RedBug, 3, introspeedincrease));

            //two groups of eight from top
            for (int i = 0; i < 8; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Challenge5(), Sprite.SpriteTypes.RedBug, 4, introspeedincrease));
            for (int i = 0; i < 8; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 28, i * Constants.BugIntroSpacing, new Challenge6(), Sprite.SpriteTypes.RedBug, 5, introspeedincrease));
        }
    }
}
