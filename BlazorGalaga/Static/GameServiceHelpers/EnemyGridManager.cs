using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class EnemyGridManager
    {
        public static int MoveEnemyGridIncrement = 3;
        public static int BreathEnemyGridIncrement = 1;
        public static bool EnemyGridBreathing = false;
        public static float LastEnemyGridMoveTimeStamp = 0;
        public static bool BreathSoundPlayed = false;

        public static void MoveEnemyGrid(List<Bug> bugs, AnimationService animationService, Ship ship)
        {
            if (BugFactory.EnemyGrid.GridLeft > 330 || BugFactory.EnemyGrid.GridLeft < 160)
                MoveEnemyGridIncrement *= -1;

            if (!EnemyGridBreathing)
            {
                BugFactory.EnemyGrid.GridLeft += MoveEnemyGridIncrement;
            }
            else
            {
                if (BugFactory.EnemyGrid.GridLeft == Constants.EnemyGridLeft)
                {
                    BugFactory.EnemyGrid.GridLeft = Constants.EnemyGridLeft;

                    if (BugFactory.EnemyGrid.HSpacing > 60 || BugFactory.EnemyGrid.HSpacing < 45)
                        BreathEnemyGridIncrement *= -1;

                    BugFactory.EnemyGrid.HSpacing += BreathEnemyGridIncrement;
                    BugFactory.EnemyGrid.VSpacing += BreathEnemyGridIncrement;

                    if (!BreathSoundPlayed)
                    {
                        SoundManager.PlaySound(SoundManager.SoundManagerSounds.breathing);
                        BreathSoundPlayed = true;
                    }
                }
                else
                    BugFactory.EnemyGrid.GridLeft += MoveEnemyGridIncrement;
            }

            bugs.ForEach(bug =>
            {
                //if the player shoots the bug befire it's captured in the tractor beam
                //then destroy the tracktor beam
                if (!bugs.Any(a => a.CaptureState == Bug.enCaptureState.Started))
                    animationService.Animatables.RemoveAll(a => a.Sprite.SpriteType == Sprite.SpriteTypes.TractorBeam);

                if (bug.IsDiveBomber)
                {
                    if (bug.CurPathPointIndex >= bug.PathPoints.Count - 1)
                    {
                        bug.Speed = Constants.BugDiveSpeed + 2;
                        bug.Paths.Add(new BezierCurve() {
                            StartPoint = bug.Location,
                            ControlPoint1 = bug.Location,
                            ControlPoint2 = bug.DiveBombLocation,
                            EndPoint = bug.DiveBombLocation
                        });
                        bug.PathPoints.AddRange(animationService.ComputePathPoints(bug.Paths.Last(), true));
                    }
                }
                else if (bug.CaptureState == Bug.enCaptureState.Started)
                {
                    if (!bug.IsMoving)
                    {
                        bug.Rotation = -90;
                        GalagaCaptureManager.DoTractorBeam(bug, animationService, ship);
                    }
                }
                else if (bug.CaptureState == Bug.enCaptureState.RecaptureStarted)
                {
                    GalagaCaptureManager.DoRecapture(bug, animationService, ship);
                }
                else
                {
                    //after the ship has been captured and disabled
                    //wait for all bugs to finish diving before re-enabling it
                    if (ship.Disabled)
                    {
                        if (bugs.Count(a => a.IsDiving) == 0)
                            ship.Disabled = false;
                    }

                    var homepoint = BugFactory.EnemyGrid.GetPointByRowCol(bug.HomePoint.X, bug.HomePoint.Y);
                    if (!(homepoint.X == 0 && homepoint.Y == 0))
                    {
                        homepoint.Y += bug.HomePointYOffset;
                        if (bug.IsMoving)
                        {
                            //this makes the bugs go to their spot on the moving enemy grid
                            if (bug.PathPoints.Count > 0)
                            {
                                bug.PathPoints[bug.PathPoints.Count - 1] = homepoint;
                                if (bug.CurPathPointIndex == bug.PathPoints.Count - 1)
                                    bug.LineToLocation = new Vector2(homepoint.X, homepoint.Y);
                            }
                        }
                        //snap to grid if bug isn't moving
                        else if (bug.Started)
                        {
                            bug.Location = homepoint;
                            bug.IsDiving = false;
                            if (bug.CaptureState == Bug.enCaptureState.FlyingBackHome)
                            {
                                bug.CaptureState = Bug.enCaptureState.Complete;
                                SoundManager.MuteAllSounds = false;
                                bug.FighterCapturedMessageShowing = false;
                                bug.ClearFighterCapturedMessage = true;
                                ship.Visible = true;
                                ship.Disabled = false;
                            }
                        }
                    }
                }
            });
        }
    }
}
