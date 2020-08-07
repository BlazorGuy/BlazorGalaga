using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class ShipManager
    {
        public static void InitShip(AnimationService animationService)
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(Constants.SpriteDestSize.Width / 2, Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
                EndPoint = new PointF(Constants.CanvasSize.Width - (Constants.SpriteDestSize.Width / 2), Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
                ControlPoint1 = new PointF(Constants.SpriteDestSize.Width / 2, Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
                ControlPoint2 = new PointF(Constants.CanvasSize.Width - (Constants.SpriteDestSize.Width / 2), Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2))
            });

            var ship = new Ship()
            {
                Paths = paths,
                DrawPath = false,
                PathIsLine = true,
                RotateAlongPath = false,
                Started = true,
                Index = -999
            };

            ship.Paths.ForEach(a => {
                ship.PathPoints.AddRange(animationService.ComputePathPoints(a));
            });

            ship.CurPathPointIndex = (int)(ship.PathPoints.Count / 2) - (int)(ship.Sprite.SpriteDestRect.Width / 2);

            animationService.Animatables.Add(ship);
        }

        public static void Fire(Ship ship, AnimationService animationService)
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(ship.Location.X + (ship.Sprite.SpriteDestRect.Width / 2) - 14, ship.Location.Y - 5),
                EndPoint = new PointF(ship.Location.X + (ship.Sprite.SpriteDestRect.Width / 2) - 16, -14)
            });

            var missle = new ShipMissle()
            {
                Paths = paths,
                DrawPath = false,
                PathIsLine = true,
                RotateAlongPath = false,
                Started = true,
                Speed = Constants.ShipMissleSpeed,
                DestroyAfterComplete = true
            };

            missle.Paths.ForEach(p => {
                missle.PathPoints.AddRange(animationService.ComputePathPoints(p, true));
            });

            animationService.Animatables.Add(missle);
        }

        public static void CheckMissileCollisions(List<Bug> bugs, AnimationService animationService)
        {

            animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.ShipMissle).ToList().ForEach(missile => {
                var missilerect = new Rectangle((int)missile.Location.X + 5, (int)missile.Location.Y + 10, 5, 5);
                foreach(var bug in bugs)
                {
                    var bugrect = new Rectangle((int)bug.Location.X, (int)bug.Location.Y, (int)bug.Sprite.SpriteDestRect.Width, (int)bug.Sprite.SpriteDestRect.Height);
                    if (missilerect.IntersectsWith(bugrect))
                    {
                        missile.DestroyImmediately = true;
                        if (bug.Sprite.SpriteType== Sprite.SpriteTypes.GreenBug || bug.Sprite.SpriteType== Sprite.SpriteTypes.GreenBug_DownFlap)
                        {
                            bug.Sprite = new Sprite(Sprite.SpriteTypes.GreenBug_Blue);
                            bug.SpriteBank.Clear();
                            bug.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.GreenBug_Blue_DownFlap));
                        }
                        else
                            bug.IsExploding = true;
                        return;
                    }
                }
            });

        }
    }
}
