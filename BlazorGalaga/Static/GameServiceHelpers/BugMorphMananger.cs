using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class BugMorphMananger
    {
        public static void DoMorph(List<Bug> bugs, Bug bug, AnimationService animationService, Ship ship)
        {

            bug.MorphCount++;

            if (bug.MorphCount == 1)
            {
                SetSpriteType(bug);
                bug.preMorphedSpriteDownFlap = bug.SpriteBank[0];
                bug.preMorphedSprite = bug.Sprite;
                bug.SpriteBank.Clear();
                bug.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.RedGreenBug_DownFlap));
            }
            else if (bug.MorphCount == 10)
            {
                bug.Sprite = new Sprite(Sprite.SpriteTypes.RedGreenBug);
            }
            else if (bug.MorphCount == 15)
            {
                var morphedbug = CreateMorphedBug(animationService, bug, true);
                EnemyDiveManager.DoEnemyDive(bugs, animationService, ship, Constants.BugDiveSpeed, morphedbug, false, false);
            }
            else if (bug.MorphCount == 17)
            {
                var morphedbug = CreateMorphedBug(animationService, bug, false);
                EnemyDiveManager.DoEnemyDive(bugs, animationService, ship, Constants.BugDiveSpeed, morphedbug, false, false);
            }
            else if (bug.MorphCount == 19)
            {
                var morphedbug = CreateMorphedBug(animationService, bug, false);
                EnemyDiveManager.DoEnemyDive(bugs, animationService, ship, Constants.BugDiveSpeed, morphedbug, false, false);
            }
            else if (bug.MorphCount == 20)
            {
                bug.DestroyImmediately = true;
                bug.MorphCount = 0;
                bug.preMorphedSprite = null;
                bug.preMorphedSpriteDownFlap = null;
            }

        }

        private static Bug CreateMorphedBug(AnimationService animationService, Bug bug, bool hashomepoint)
        {
            var morphedbug = new Bug(bug.MorphedspriteType)
            {
                Paths = new List<BezierCurve>(),
                Started = true,
                ZIndex = 100,
                RotateAlongPath = true,
                Location = bug.Location,
                IsMorphedBug = true,
                PreMorphedSprite = bug.preMorphedSprite,
                PreMorphedSpriteDownFlap = bug.preMorphedSpriteDownFlap
            };

            if (hashomepoint)
                morphedbug.HomePoint = bug.HomePoint;

            animationService.Animatables.Add(morphedbug);

            return morphedbug;
        }

        private static void SetSpriteType(Bug bug)
        {
            var r = Utils.Rnd(1, 300);

            if (r <= 100)
                bug.MorphedspriteType = Sprite.SpriteTypes.GreenBugShip;
            else if (r <= 200)
                bug.MorphedspriteType = Sprite.SpriteTypes.YellowBugShip;
            else
                bug.MorphedspriteType = Sprite.SpriteTypes.YelloBug;
        }
    }
}
