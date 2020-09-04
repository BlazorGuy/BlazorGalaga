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

        public static void DoRecapture(Bug bug, AnimationService animationService, Ship ship)
        {
            bug.OutputDebugInfo = true;
            bug.RotateWhileStill = true;
            bug.ManualRotationRate = 15;
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
                if (tb.SpriteBank.First().DestRect.Value.Height < Constants.BiggerSpriteDestSize.Height && ship.Visible != false)
                {
                    tb.SpriteBank.ForEach(a =>
                    {
                        a.SourceRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.SourceRect.Value.Height + 20);
                        a.DestRect = new Rectangle(0, 0, Constants.BiggerSpriteDestSize.Width, a.DestRect.Value.Height + 20);
                    });
                }
                else
                {
                    if (CapturedShip == null)
                    {
                        //hide the real ship and create the
                        //captured ship sprite
                        ship.Visible = false;
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
                            }
                            else
                            {
                                bug.CaptureState = Bug.enCaptureState.FlyingBackHome;
                            }
                        }
                    }
                }
                
                tb.SpriteBankIndex += 1;
                
                if (tb.SpriteBankIndex > 2)
                    tb.SpriteBankIndex = 0;
            }
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
