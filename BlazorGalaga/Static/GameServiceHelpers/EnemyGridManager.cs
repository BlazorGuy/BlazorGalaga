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

        public static void MoveEnemyGrid(List<Bug> bugs, AnimationService animationService, Ship ship)
        {
            if (BugFactory.EnemyGrid.GridLeft > 350 || BugFactory.EnemyGrid.GridLeft < 180)
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
                }
                else
                    BugFactory.EnemyGrid.GridLeft += MoveEnemyGridIncrement;

            }

            bugs.ForEach(bug =>
            {
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
                else
                {
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
                        else
                        {
                            bug.Location = homepoint;
                            if (bug.CaptureState == Bug.enCaptureState.FlyingBackHome)
                            {
                                bug.CaptureState = Bug.enCaptureState.Complete;
                                Utils.dOut("Capture Complete!", "yep");
                            }
                        }
                    }
                }
            });
        }
    }
}
