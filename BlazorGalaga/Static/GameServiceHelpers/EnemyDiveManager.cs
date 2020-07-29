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
    public static class EnemyDiveManager
    {
        public static Bug DoEnemyDive(List<Bug> bugs, AnimationService animationService, Ship ship)
        {
            Bug bug = null;
            int loopcount = 0;

            while (bug == null || bug.IsMoving)
            {
                bug = bugs.FirstOrDefault(a => a.Index == Utils.Rnd(0, bugs.OrderBy(b=>b.Index).Last().Index));
                loopcount++;
                if (loopcount > 50) return null;
            }

            bug.SpriteBankIndex = null;
            bug.IsDiving = true;

            IDive dive = null;
            //dive = new GreenBugDive1();

            if (bug.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug)
            {
                if (Utils.Rnd(0, 10) % 2 == 0)
                    dive = new BlueBugDive1();
                else
                    dive = new BlueBugDive2();
            }
            else if (bug.Sprite.SpriteType == Sprite.SpriteTypes.RedBug)
            {
                if (Utils.Rnd(0, 10) % 2 == 0)
                    dive = new RedBugDive1();
                else
                    dive = new RedBugDive2();
            }
            else if (bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug)
            {
                var childbugs = bugs.Where(a =>
                    (a.HomePoint == new Point(2, bug.HomePoint.Y + 1) ||
                    a.HomePoint == new Point(2, bug.HomePoint.Y + 2))
                    && !a.IsMoving);

                bug.ChildBugs.AddRange(childbugs);
                bug.ChildBugOffset = new Point(35, 35);

                if (Utils.Rnd(0, 10) % 2 == 0)
                    dive = new GreenBugDive1();
                else
                    dive = new GreenBugDive2();
            }
            else
            {
                dive = new RedBugDive1();
            }

            animationService.Animatables.ForEach(a => a.ZIndex = 0);

            var paths = dive.GetPaths(bug, ship);

            bug.RotateAlongPath = true;
            bug.ZIndex = 100;
            bug.Speed = 5;
            bug.Paths.AddRange(paths);

            paths.ForEach(p => {
                p.DrawPath = true;
                bug.PathPoints.AddRange(animationService.ComputePathPoints(p));
            });

            return bug;
        }

        public static void DoEnemyFire(Bug bug, AnimationService animationService, Ship ship)
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(bug.Location.X +5, bug.Location.Y + 5),
                EndPoint = new PointF(ship.Location.X + 20, ship.Location.Y + 20)
            });

            var missle = new BugMissle()
            {
                Paths = paths,
                DrawPath = false,
                PathIsLine = true,
                RotateAlongPath = false,
                Started = true,
                Speed = 5,
                DestroyAfterComplete = true
            };

            missle.Paths.ForEach(p => {
                missle.PathPoints.AddRange(animationService.ComputePathPoints(p, true));
            });

            animationService.Animatables.Add(missle);
        }
    }
}
