using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;
using BlazorGalaga.Models.Paths.Challenges.Challenge3;
using BlazorGalaga.Services;

namespace BlazorGalaga.Static.Levels
{
    public static class Level11
    {
        public static void InitIntro(AnimationService animationService, int introspeedincrease)
        {
            //introspeedincrease = -5;
            //two groups of four from top
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i, (i * Constants.BugIntroSpacing * 2) + 75, new Challenge1(), Sprite.SpriteTypes.MosquitoBug, 1, introspeedincrease, false));
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing * 2, new Challenge2(), Sprite.SpriteTypes.MosquitoBug, 1, introspeedincrease, false));

            //two groups of four from bottom
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 8, i * Constants.BugIntroSpacing, new Challenge3(), Sprite.SpriteTypes.GreenBug, 2, introspeedincrease, false));
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 12, i * Constants.BugIntroSpacing, new Challenge4(), Sprite.SpriteTypes.MosquitoBug, 2, introspeedincrease, false));

            //two groups of four from bottom
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Challenge3(), Sprite.SpriteTypes.MosquitoBug, 3, introspeedincrease, false));
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 20, i * Constants.BugIntroSpacing, new Challenge4(), Sprite.SpriteTypes.MosquitoBug, 3, introspeedincrease, false));

            //two groups of eight from top
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 24, (i * Constants.BugIntroSpacing * 2) + 75, new Challenge1(), Sprite.SpriteTypes.MosquitoBug, 4, introspeedincrease, false));
            for (int i = 0; i < 4; i++)
                animationService.Animatables.Add(BugFactory.CreateAnimatable_BugIntro(i + 28, i * Constants.BugIntroSpacing * 2, new Challenge2(), Sprite.SpriteTypes.MosquitoBug, 4, introspeedincrease, false));
        }
    }
}
