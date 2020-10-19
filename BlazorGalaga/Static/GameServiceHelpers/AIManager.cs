using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class AIManager
    {
        private static Bug aibug;
        private static bool aifred = false;
        public static bool aidodgeing = false;

        private static void AIFire(Ship ship)
        {
            if (!aifred)
            {
                ship.IsFiring = true;
                aifred = true;
                Task.Delay(Utils.Rnd(100, 350)).ContinueWith((task) =>
                {
                    aifred = false;
                });
            }
        }

        public static void AI(List<Bug> bugs, AnimationService animationService, Ship ship)
        {
            if (aidodgeing) return;

            foreach (var missile in animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.BugMissle).OrderByDescending(a => a.Location.Y))
            {
                var missilerect = new RectangleF(missile.Location.X, missile.Location.Y, 100, 400);
                var shiprect = new RectangleF(ship.Location.X, ship.Location.Y, 80, 80);
                if (shiprect.IntersectsWith(missilerect))
                {
                    if (ship.Location.X <= missile.Location.X)
                    {
                        ship.Speed = Constants.ShipMoveSpeed * -1;
                    }
                    else
                        ship.Speed = Constants.ShipMoveSpeed;
                    aidodgeing = true;
                    break;
                }
            }

            if (bugs == null || bugs.Count == 0 || !bugs.Any(a => a.Started)) return;

            bugs = bugs.Where(a => a.Started).ToList();

            if (aibug == null || !bugs.Contains(aibug)) aibug = bugs[Utils.Rnd(0, bugs.Count - 1)];

            //always choose a diving bug when there is one
            if (!aibug.IsDiving && bugs.Any(a => a.IsDiving)) aibug = bugs.OrderByDescending(a => a.Location.Y).FirstOrDefault(a => a.IsDiving);

            foreach (var b in bugs.OrderByDescending(a => a.Location.Y))
            {
                var bugrect = new RectangleF(b.Location.X, b.Location.Y, 100, 200);
                var shiprect = new RectangleF(ship.Location.X, ship.Location.Y, 80, 80);
                if (shiprect.IntersectsWith(bugrect))
                {
                    if (ship.Location.X <= b.Location.X)
                        ship.Speed = Constants.ShipMoveSpeed * -1;
                    else
                        ship.Speed = Constants.ShipMoveSpeed;
                    aidodgeing = true;
                    break;
                }
            }

            if (aidodgeing)
            {
                AIFire(ship);
                Task.Delay(250).ContinueWith((task) =>
                {
                    aidodgeing = false;
                });
                return;
            }

            if (ship.Location.X <= aibug.Location.X)
                ship.Speed = Constants.ShipMoveSpeed;
            else if (ship.Location.X >= aibug.Location.X + 16 + Constants.ShipMoveSpeed)
                ship.Speed = Constants.ShipMoveSpeed * -1;
            else
            {
                AIFire(ship);
                ship.Speed = 0;
            }

        }
    }
}
