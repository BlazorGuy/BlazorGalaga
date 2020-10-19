using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using static BlazorGalaga.Pages.Index;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class DebugManager
    {
        public static void DoDebugLogic(GameLoopObject glo, List<Bug> bugs, AnimationService animationService, Ship ship)
        {
            //for debugging purposes
            if (MouseHelper.MouseIsDown)
            {
                bugs.ForEach(a => a.OutputDebugInfo = false);
                bugs.ForEach(a => {
                    var bugrect = new RectangleF(a.Location.X, a.Location.Y, 32, 32);
                    var mouserect = new RectangleF(MouseHelper.Position.X, MouseHelper.Position.Y, 10, 10);
                    if (bugrect.IntersectsWith(mouserect)) a.OutputDebugInfo = true;
                });
            }

            //for debugging purposes
            if (glo.captureship)
            {
                bugs.ForEach(a => {
                    a.Location = BugFactory.EnemyGrid.GetPointByRowCol(a.HomePoint.X, a.HomePoint.Y);
                    a.CurPathPointIndex = 0;
                    a.PathPoints.Clear();
                    a.Paths.Clear();
                    a.IsMoving = false;
                    a.StartDelay = 0;
                    a.Started = true;
                });
                var bug = bugs.FirstOrDefault(a => a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug);
                EnemyDiveManager.DoEnemyDive(bugs, animationService, ship, Constants.BugDiveSpeed, bug, true);
            }

            //for debugging purposes
            if (glo.morphbug)
            {
                ship.Sprite = new Sprite(Sprite.SpriteTypes.DoubleShip);
                bugs.Where(a => a.IsInIntro).ToList().ForEach(a => {
                    a.Location = BugFactory.EnemyGrid.GetPointByRowCol(a.HomePoint.X, a.HomePoint.Y);
                    a.CurPathPointIndex = 0;
                    a.PathPoints.Clear();
                    a.Paths.Clear();
                    a.IsMoving = false;
                    a.StartDelay = 0;
                    a.Started = true;
                });
                var redblubugs = bugs.Where(a => (a.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug || a.Sprite.SpriteType == Sprite.SpriteTypes.RedBug) && a.MorphState != Bug.enMorphState.Started && !a.IsDiving).ToList();
                var bug = redblubugs[Utils.Rnd(0, redblubugs.Count - 1)];
                bug.MorphState = Bug.enMorphState.Started;
            }

            //for debugging purposes
            if (glo.killbugs)
            {
                bugs.All(a => a.IsExploding = true);
            }
        }
    }
}
