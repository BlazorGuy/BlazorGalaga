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
                StartPoint = new PointF(0, Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
                EndPoint = new PointF(Constants.CanvasSize.Width, Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
                ControlPoint1 = new PointF(0, Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
                ControlPoint2 = new PointF(Constants.CanvasSize.Width, Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
            });

            var ship = new Ship()
            {
                Paths = paths,
                RotateAlongPath = false,
                Started = true,
                Index = -999
            };

            ship.Paths.ForEach(a => {
                a.DrawPath = true;
                ship.PathPoints.AddRange(animationService.ComputePathPoints(a));
            });

            ship.Location = new PointF(0,0);
            ship.LineToLocation = new System.Numerics.Vector2(0,0);
            ship.CurPathPointIndex = ship.PathPoints.Count / 2;

            animationService.Animatables.Add(ship);
        }

        public static void Fire(Ship ship, AnimationService animationService)
        {

            var c = ship.Sprite.SpriteType == Sprite.SpriteTypes.DoubleShip ? 1: 0;
            
            for (var i = 0; i <= c; i++)
            {
                List<BezierCurve> paths = new List<BezierCurve>();

                paths.Add(new BezierCurve()
                {
                    StartPoint = new PointF(ship.Location.X + ((ship.Sprite.SpriteDestRect.Width / 2) - 14) + (i * 45), Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2) - 5),
                    EndPoint = new PointF(ship.Location.X + ((ship.Sprite.SpriteDestRect.Width / 2) - 16) + (i * 45), -14)
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

                missle.Paths.ForEach(p =>
                {
                    missle.PathPoints.AddRange(animationService.ComputePathPoints(p, true));
                });

                animationService.Animatables.Add(missle);
            }
        }

        public static bool CheckMissileCollisions(List<Bug> bugs, AnimationService animationService)
        {
           foreach(var missile in  animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.ShipMissle).ToList())
           { 
                var missilerect = new Rectangle((int)missile.Location.X + 5, (int)missile.Location.Y + 8, 2, 2);
                foreach(var bug in bugs)
                {
                    var bugrect = new Rectangle((int)bug.Location.X+5, (int)bug.Location.Y+5, (int)bug.Sprite.SpriteDestRect.Width-5, (int)bug.Sprite.SpriteDestRect.Height-5);
                    if (missilerect.IntersectsWith(bugrect))
                    {
                        missile.DestroyImmediately = true;
                        if (bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug || bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug_DownFlap)
                        {
                            bug.Sprite = new Sprite(Sprite.SpriteTypes.GreenBug_Blue);
                            bug.SpriteBank.Clear();
                            bug.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.GreenBug_Blue_DownFlap));
                            SoundManager.PlaySound(SoundManager.SoundManagerSounds.galagahit);
                        }
                        else
                        {
                            //if we've shot the captured ship then
                            //remove it from the parent bug if that bug still exists
                            if (bug.Sprite.SpriteType == Sprite.SpriteTypes.CapturedShip)
                            {
                                var b = bugs.FirstOrDefault(a => a.CapturedBug != null);
                                if (b != null)
                                {
                                    b.CapturedBug = null;
                                    b.CaptureState = Bug.enCaptureState.NotStarted;
                                }
                            }
                            if (bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug_Blue || bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug_Blue_DownFlap)
                                SoundManager.PlaySound(SoundManager.SoundManagerSounds.galagadesgtroyed);
                            else if (bug.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug ||  bug.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug_DownFlap)
                                SoundManager.PlaySound(SoundManager.SoundManagerSounds.bluebughit);
                            else
                                SoundManager.PlaySound(SoundManager.SoundManagerSounds.redbughit);
                            bug.IsExploding = true;
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
