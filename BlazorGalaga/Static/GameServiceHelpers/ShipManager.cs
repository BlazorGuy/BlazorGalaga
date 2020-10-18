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
                Index = -999,
                Visible = false
            };

            ship.Paths.ForEach(a => {
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
                    DestroyAfterComplete = true,
                };

                missle.Paths.ForEach(p =>
                {
                    missle.PathPoints.AddRange(animationService.ComputePathPoints(p, true));
                });

                animationService.Animatables.Add(missle);
            }
        }

        public static void DoShipExplosion(Ship ship, AnimationService animationService, GameService gameService)
        {

            if (!animationService.Animatables.Any(a => a.Sprite.SpriteType == Sprite.SpriteTypes.ShipExplosion1))
            {
                CreateExplosion(ship, animationService, gameService);

                SoundManager.PlaySound(SoundManager.SoundManagerSounds.shipexplode);

                if (!ship.IsDoubleShip)
                    ship.Visible = false;

                else if (ship.LeftShipHit)
                    ship.Location = new PointF(ship.Location.X + Constants.SpriteDestSize.Width-3, ship.Location.Y);
            }

            animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.ShipExplosion1).ToList().ForEach(exp =>
            {
                if (exp.SpriteBankIndex < 7)
                    exp.SpriteBankIndex += 1;
                else
                {
                    exp.DestroyImmediately = true;
                    ship.IsExploding = false;
                    ship.HasExploded = true;
                }
            });
        }

        private static void CreateExplosion(Ship ship, AnimationService animationService, GameService gameService)
        {
            var exp = new ShipExplosion()
            {
                DrawPath = false,
                RotateAlongPath = false,
                Started = true,
                DestroyAfterComplete = false,
                IsMoving = false,
                PathIsLine = true,
                Location = new PointF(ship.Location.X, ship.Location.Y)
            };

            if (ship.LeftShipHit)
                exp.Location = new PointF(ship.Location.X, ship.Location.Y);
            else if (ship.RightShipHit)
                exp.Location = new PointF(ship.Location.X + Constants.SpriteDestSize.Width, ship.Location.Y);

            if(ship.Sprite.SpriteType == Sprite.SpriteTypes.DoubleShip)
            {
                ship.IsDoubleShip = true;
                ship.Sprite = new Sprite(Sprite.SpriteTypes.Ship);
            }

            exp.SpriteBankIndex = -1;

            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.ShipExplosion1));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.ShipExplosion1));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.ShipExplosion2));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.ShipExplosion2));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.ShipExplosion3));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.ShipExplosion3));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.ShipExplosion4));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.ShipExplosion4));

            animationService.Animatables.Add(exp);
        }

        public static bool CheckShipCollisions(List<Bug> bugs, AnimationService animationService, Ship ship)
        {
            if (!ship.Visible || ship.Disabled) return false;

            bool shiphit = false;
            var shipwidth = 25;
            RectangleF shiprect = new RectangleF(ship.Location.X, ship.Location.Y, shipwidth, 25);
            RectangleF ship2rect = new RectangleF(ship.Location.X + Constants.SpriteDestSize.Width, ship.Location.Y, shipwidth, 25);

            foreach(var bug in bugs)
            { 
                var bugrect = new RectangleF((int)bug.Location.X, (int)bug.Location.Y, (int)bug.Sprite.SpriteDestRect.Width - 15, (int)bug.Sprite.SpriteDestRect.Height - 15);
                if (bugrect.IntersectsWith(shiprect))
                {
                    shiphit = true;
                    ship.LeftShipHit = true;
                }
                else if (ship.Sprite.SpriteType == Sprite.SpriteTypes.DoubleShip && bugrect.IntersectsWith(ship2rect))
                {
                    shiphit = true;
                    ship.RightShipHit = true;
                }
                if (shiphit)
                {
                    bug.IsExploding = true;
                    break;
                }
            }

            foreach (var missile in animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.BugMissle).ToList())
            {
                var missilerect = new RectangleF((int)missile.Location.X, (int)missile.Location.Y + 8, 25, 20);
                if (missilerect.IntersectsWith(shiprect))
                {
                    shiphit = true;
                    ship.LeftShipHit = true;
                }
                else if (ship.Sprite.SpriteType == Sprite.SpriteTypes.DoubleShip && missilerect.IntersectsWith(ship2rect))
                {
                    shiphit = true;
                    ship.RightShipHit = true;
                }
                if (shiphit)
                {
                    break;
                }
            }

            return shiphit;
        }

        public static int CheckMissileCollisions(List<Bug> bugs, AnimationService animationService)
        {
           foreach(var missile in  animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.ShipMissle).ToList())
           { 
                var missilerect = new Rectangle((int)missile.Location.X + 5, (int)missile.Location.Y + 8, 3, 20);
                foreach(var bug in bugs)
                {
                    var bugrect = new Rectangle((int)bug.Location.X+5, (int)bug.Location.Y+5, (int)bug.Sprite.SpriteDestRect.Width-15, (int)bug.Sprite.SpriteDestRect.Height-15);
                    if (missilerect.IntersectsWith(bugrect))
                    {
                        missile.DestroyImmediately = true;
                        if (bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug || bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug_DownFlap)
                        {
                            bug.Sprite = new Sprite(Sprite.SpriteTypes.GreenBug_Blue);
                            bug.SpriteBank.Clear();
                            bug.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.GreenBug_Blue_DownFlap));
                            SoundManager.PlaySound(SoundManager.SoundManagerSounds.galagahit);
                            return 0;
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
                                SoundManager.PlaySound(SoundManager.SoundManagerSounds.galagadestroyed);
                            else if (bug.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug || bug.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug_DownFlap)
                                SoundManager.PlaySound(SoundManager.SoundManagerSounds.bluebughit);
                            else if (bug.Sprite.SpriteType == Sprite.SpriteTypes.CapturedShip)
                            {
                                SoundManager.StopAllSounds();
                                SoundManager.PlaySound(SoundManager.SoundManagerSounds.capturedfighterdestroyedsong);
                            }
                            else
                                SoundManager.PlaySound(SoundManager.SoundManagerSounds.redbughit);
                            bug.IsExploding = true;
                        }
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
