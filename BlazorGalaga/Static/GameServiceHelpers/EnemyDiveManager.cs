using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;
using BlazorGalaga.Services;
using BlazorGalaganimatable.Models.Paths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class EnemyDiveManager
    {
        public static Bug DoEnemyDive(List<Bug> bugs, AnimationService animationService, Ship ship, int speed)
        {
            Bug bug = null;
            int loopcount = 0;

            while (bug == null || bug.IsMoving || bug.IsCapturing)
            {
                //bug = bugs[Utils.Rnd(0, bugs.Count - 1)];
                bug = bugs.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug).FirstOrDefault();
                loopcount++;
                if (loopcount > 50) return null;
            }

            bug.SpriteBankIndex = null;
            bug.IsDiving = true;

            IDive dive = null;

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
                dive = new CaptureDive();

                bug.RotateWhileStill = true;
                bug.IsCapturing = true;

                //var childbugs = bugs.Where(a =>
                //    (a.HomePoint == new Point(2, bug.HomePoint.Y + 1) ||
                //    a.HomePoint == new Point(2, bug.HomePoint.Y + 2))
                //    && !a.IsMoving);

                //bug.ChildBugs.AddRange(childbugs);
                //bug.ChildBugOffset = new Point(35, 35);

                //if (Utils.Rnd(0, 10) % 2 == 0)
                //    dive = new GreenBugDive1();
                //else
                //    dive = new GreenBugDive2();
            }
            else
                dive = new RedBugDive1();

            animationService.Animatables.ForEach(a => a.ZIndex = 0);

            var paths = dive.GetPaths(bug, ship);

            bug.RotateAlongPath = true;
            bug.ZIndex = 100;
            bug.Speed = speed;
            bug.Paths.AddRange(paths);

            paths.ForEach(p => {
                bug.PathPoints.AddRange(animationService.ComputePathPoints(p,false));
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
                Speed = Utils.Rnd(Constants.EnemyMissileSpeed-1, Constants.EnemyMissileSpeed + 1),
                DestroyAfterComplete = true
            };

            missle.Paths.ForEach(p => {
                missle.PathPoints.AddRange(animationService.ComputePathPoints(p, true));
            });

            animationService.Animatables.Add(missle);
        }
    }
}
