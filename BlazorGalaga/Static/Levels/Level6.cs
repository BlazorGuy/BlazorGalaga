using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths.Intros;
using BlazorGalaga.Models.Paths.Challenges;
using BlazorGalaga.Services;

namespace BlazorGalaga.Static.Levels
{
    public static class Level6
    {
        public static void InitIntro(AnimationService animationService, int introspeedincrease, int maxintromissiles)
        {
            //two groups of four from top
            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug,1, introspeedincrease, i==4));
            for (int i = 0; i < 5; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing, new Intro2(), Sprite.SpriteTypes.RedBug,1, introspeedincrease, i==4));

            //two groups of eight from bottom
            for (int i = 0; i < 8; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 8, i * Constants.BugIntroSpacing, new Intro3(), i % 2 != 0 ? Sprite.SpriteTypes.GreenBug : Sprite.SpriteTypes.RedBug, 2, introspeedincrease));
            for (int i = 0; i < 8; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Intro4(), Sprite.SpriteTypes.RedBug, 3, introspeedincrease));

            //two groups of eight from top
            for (int i = 0; i < 9; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug, 4, introspeedincrease, i==8));
            for (int i = 0; i < 9; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Intro2(), Sprite.SpriteTypes.BlueBug, 5, introspeedincrease, i==8));
        }
    }
}
