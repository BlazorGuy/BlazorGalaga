using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class GalagaCaptureManager
    {
        private static Ship CapturedShip;
        private static int TractorBeamWaitCount;

        public static void Reset()
        {
            CapturedShip = null;
            TractorBeamWaitCount = 0;
        }

        public static void DoRecapture(Bug bug, AnimationService animationService, Ship ship)
        {
            ship.Disabled = true;

            if (bug.Rotation < 1500 && !bug.AligningHorizontally && !bug.AligningVertically)
            {
                //spin for a second or two
                bug.PathPoints.Clear();
                bug.Paths.Clear();
                bug.IsMoving = false;
                bug.RotateWhileStill = true;
                bug.ManualRotationRate = 15;
                bug.Sprite.SpriteType = Sprite.SpriteTypes.Ship;
                bug.Sprite.IsInitialized = false;
                bug.HomePoint = new Point(0, 0);
            }
            else if (!bug.IsMoving && !bug.AligningHorizontally)
            {
                //fly horizontally to align with ship
                bug.AligningHorizontally = true;
                bug.RotateWhileStill = false;
                bug.ManualRotationRate = 0;
                bug.Rotation = 0;
                bug.ManualRotation = 0;
                bug.RotateAlongPath = false;
                bug.Paths.Add(new BezierCurve()
                {
                    StartPoint = bug.Location,
                    EndPoint = new PointF(ship.Location.X + ship.Sprite.SpriteDestRect.Width - 3, bug.Location.Y),
                    ControlPoint1 = bug.Location,
                    ControlPoint2 = new PointF(ship.Location.X + ship.Sprite.SpriteDestRect.Width - 3, bug.Location.Y)
                });
                bug.Paths.ForEach(a => 
                    bug.PathPoints.AddRange(animationService.ComputePathPoints(a, true))
                );
            }
            else if (!bug.IsMoving && !bug.AligningVertically)
            {
                bug.AligningVertically = true;
                //fly vertically to align with ship
                bug.Paths.Add(new BezierCurve()
                {
                    StartPoint = bug.Location,
                    EndPoint = new PointF(ship.Location.X + ship.Sprite.SpriteDestRect.Width - 3, ship.Location.Y),
                    ControlPoint1 = bug.Location,
                    ControlPoint2 = new PointF(ship.Location.X + ship.Sprite.SpriteDestRect.Width - 3, ship.Location.Y),
                });
                bug.Paths.ForEach(a =>
                    bug.PathPoints.AddRange(animationService.ComputePathPoints(a, true))
                );
            }
            else if (!bug.IsMoving && bug.AligningHorizontally && bug.AligningVertically)
            {
                bug.DestroyImmediately = true;
                ship.Sprite = new Sprite(Sprite.SpriteTypes.DoubleShip);
            }
        }

        public static void DoTractorBeam(Bug bug, AnimationService animationService, Ship ship)
        {
            var tb = animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.TractorBeam).FirstOrDefault();

            if (tb == null)
            {
                //create the tractor bream
                //it's height is zero at this point
                CreateTractorBeam(animationService, ship, bug);
            }
            else
            {
                //extend the tractor beam over the course of a few seconds
                if (TractorBeamWaitCount == 0 && tb.SpriteBank.First().DestRect.Value.Height < Constants.BiggerSpriteDestSize.Height && ship.Visible != false)
                {
                    tb.SpriteBank.ForEach(a =>
                    {
                        a.SourceRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.SourceRect.Value.Height + 20);
                        a.DestRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.DestRect.Value.Height + 20);
                    });
                }
                //if the ship is under the tractor beam and the tractor beam is extended
                else if(((ship.Location.X >= bug.Location.X - 75 && ship.Location.X <= bug.Location.X + 75) && tb.SpriteBank.First().DestRect.Value.Height > Constants.BiggerSpriteDestSize.Height-20) || CapturedShip != null)
                {
                    if (CapturedShip == null)
                    {
                        //hide the real ship and create the
                        //captured ship sprite
                        ship.Visible = false;
                        ship.Disabled = true;
                        CreateCapturedShip(animationService, ship, bug);
                    }
                    else
                    {
                        if (CapturedShip.IsMoving)
                        {
                            //captured ship spins up to bug
                            CapturedShip.ManualRotation += 25;
                        }
                        else
                        {
                            CapturedShip.ManualRotation = 0;
                            //retract the tractor beam over the course
                            //of a few seconds
                            if (tb.SpriteBank.First().DestRect.Value.Height > 0)
                            {
                                tb.SpriteBank.ForEach(a =>
                                {
                                    a.SourceRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.SourceRect.Value.Height - 20);
                                    a.DestRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.DestRect.Value.Height - 20);
                                });
                            }
                            else if (bug.CapturedBug == null)
                            {
                                //create captured bug and send the main bug
                                //home
                                CapturedShip.Visible = false;
                                CreateCapturedShipChildBug(animationService, bug);
                                SendBugHome(animationService, bug);
                            }
                            else
                            {
                                bug.CaptureState = Bug.enCaptureState.FlyingBackHome;
                            }
                        }
                    }
                }
                else if (ship.Visible)
                {
                    //ship isn't under the tractor beam
                    TractorBeamWaitCount += 1;
                    if (TractorBeamWaitCount > 20)
                    {
                        if (tb.SpriteBank.First().DestRect.Value.Height > 0)
                        {
                            tb.SpriteBank.ForEach(a =>
                            {
                                a.SourceRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.SourceRect.Value.Height - 20);
                                a.DestRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.DestRect.Value.Height - 20);
                            });
                        }
                        else
                        {
                            SendBugHome(animationService, bug);
                            TractorBeamWaitCount = 0;
                            bug.CaptureState = Bug.enCaptureState.NotStarted;
                            bug.RotateWhileStill = false;
                        }
                    }
                }

                //animate the tractor beam
                tb.SpriteBankIndex += 1;
                
                if (tb.SpriteBankIndex > 2)
                    tb.SpriteBankIndex = 0;
            }
        }

        private static void SendBugHome(AnimationService animationService, Bug bug)
        {
            bug.Paths.Add(new BezierCurve()
            {
                StartPoint = bug.Location,
                EndPoint = BugFactory.EnemyGrid.GetPointByRowCol(bug.HomePoint.X, bug.HomePoint.Y),
                ControlPoint1 = bug.Location,
                ControlPoint2 = BugFactory.EnemyGrid.GetPointByRowCol(bug.HomePoint.X, bug.HomePoint.Y)
            });
            bug.Paths.ForEach(a =>
            {
                bug.PathPoints.AddRange(animationService.ComputePathPoints(a, true));
            });
        }
        private static void CreateCapturedShipChildBug(AnimationService animationService, Bug bug)
        {
            bug.CapturedBug = new Bug(Sprite.SpriteTypes.CapturedShip)
            {
                Started = true,
                ZIndex = 100,
                RotateAlongPath = true,
                Location = bug.Location,
                HomePoint = bug.HomePoint,
                HomePointYOffset = -50
            };
            animationService.Animatables.Add(bug.CapturedBug);
            bug.ChildBugs.Add(bug.CapturedBug);
            bug.ChildBugOffset = new Point(0, 25);
            bug.RotateWhileStill = false;
        }

        private static void CreateTractorBeam(AnimationService animationService, Ship ship, Bug bug)
        {
            var tb = new TractorBeam()
            {
                DrawPath = false,
                RotateAlongPath = false,
                Started = true,
                DestroyAfterComplete = false,
                IsMoving = false,
                PathIsLine = true,
                Location = new PointF(bug.Location.X, bug.Location.Y + 155)
            };

            tb.SpriteBankIndex = 0;

            tb.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.TractorBeam));
            tb.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.TractorBeam2));
            tb.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.TractorBeam3));
            tb.SpriteBank.ForEach(a => {
                a.SourceRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, 0);
                a.DestRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, 0);
            });

            animationService.Animatables.Add(tb);
        }

        private static void CreateCapturedShip(AnimationService animationService,Ship ship, Bug bug)
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = ship.Location,
                EndPoint = new PointF(bug.Location.X,bug.Location.Y + 50),
                ControlPoint1 = ship.Location,
                ControlPoint2 = bug.Location,
            });

            CapturedShip = new Ship()
            {
                Sprite = new Sprite(Sprite.SpriteTypes.CapturedShip),
                Paths = paths,
                RotateAlongPath = true,
                Started = true,
                Index = -999,
                Speed = 4
            };

            CapturedShip.Paths.ForEach(a => {
                CapturedShip.PathPoints.AddRange(animationService.ComputePathPoints(a,true));
            });

            animationService.Animatables.Add(CapturedShip);
        }
    }
}
