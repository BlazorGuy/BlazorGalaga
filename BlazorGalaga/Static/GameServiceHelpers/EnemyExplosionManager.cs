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
        public static void DoEnemyExplosions(List<Bug> bugs, AnimationService animationService)
        {
            bugs.Where(a => a.IsExploding).ToList().ForEach(bug => {
                CreateExplosion(bug.Location, animationService);
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

        private static void CreateExplosion(PointF location, AnimationService animationService)
        {
            var exp = new EnemyExplosion()
            {
                DrawPath = false,
                RotateAlongPath = false,
                Started = true,
                DestroyAfterComplete = false,
                IsMoving = false,
                PathIsLine = true,
                Location = location
            };

            exp.SpriteBankIndex = -1;

            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion1));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion2));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion3));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion4));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion5));

            animationService.Animatables.Add(exp);
        }
    }
}
