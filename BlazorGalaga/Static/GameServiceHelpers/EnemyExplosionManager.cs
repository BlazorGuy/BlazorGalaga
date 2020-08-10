using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;
using BlazorGalaga.Services;
using BlazorGalaganimatable.Models.Paths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class EnemyExplosionManager
    {
        public static void DoEnemyExplosions(List<Bug> bugs, AnimationService animationService,GameService gameService)
        {
            bugs.Where(a => a.IsExploding && a.Visible).ToList().ForEach(bug => {

                CreateExplosion(bug, animationService, gameService);

                //remove bug as child bug if we are destroying it
                bugs.ForEach(b => b.ChildBugs.RemoveAll(r => r.Index == bug.Index));

                if (bug.ChildBugs != null && bug.ChildBugs.Count > 0)
                {
                    bug.Visible = false;
                    bug.DestroyAfterComplete = true;
                }
                else
                    bug.DestroyImmediately = true;
        });

            animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.EnemyExplosion1).ToList().ForEach(exp =>
            {
                if (exp.SpriteBankIndex < 4)
                    exp.SpriteBankIndex += 1;
                else
                    exp.DestroyImmediately = true;
            });
        }

        private static void CreateExplosion(Bug bug, AnimationService animationService, GameService gameService)
        {
            var exp = new EnemyExplosion()
            {
                DrawPath = false,
                RotateAlongPath = false,
                Started = true,
                DestroyAfterComplete = false,
                IsMoving = false,
                PathIsLine = true,
                Location = new PointF(bug.Location.X + 5, bug.Location.Y)
            };

            exp.SpriteBankIndex = -1;

            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion1));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion2));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion3));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion4));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion5));

            animationService.Animatables.Add(exp);

            if (bug.Sprite.SpriteType == Sprite.SpriteTypes.RedBug)
                gameService.Score += bug.IsDiveBomber || bug.IsDiving ? Constants.Score_RedBugDiving : Constants.Score_RedBug;
            if (bug.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug)
                gameService.Score += bug.IsDiveBomber || bug.IsDiving ? Constants.Score_BlueBugDiving : Constants.Score_BlueBug;
            if (bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug)
                gameService.Score += bug.IsDiveBomber || bug.IsDiving ? Constants.Score_GreenBugDiving : Constants.Score_GreenBug;
        }
    }
}
