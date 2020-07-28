using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            });

            var ship = new Ship()
            {
                Paths = paths,
                DrawPath = false,
                PathIsLine = true,
                RotateAlongPath = false,
                Started = true
            };

            ship.Paths.ForEach(a => {
                ship.PathPoints.AddRange(animationService.ComputePathPoints(a));
            });

            ship.CurPathPointIndex = (int)(ship.PathPoints.Count / 3) - (int)(ship.Sprite.SpriteDestRect.Width / 2);

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
                Speed = 10
            };

            missle.Paths.ForEach(p => {
                missle.PathPoints.AddRange(animationService.ComputePathPoints(p, true));
            });

            animationService.Animatables.Add(missle);
        }
    }
}
