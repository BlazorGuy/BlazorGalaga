using BlazorGalaga.Models;
using System;
using System.Collections.Generic;
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

        public static void MoveEnemyGrid(List<Bug> bugs, Ship ship)
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
                    //if (bug.CurPathPointIndex >= bug.PathPoints.Count -1)
                    //    bug.Speed = Constants.BugDiveSpeed + 2;
                    //bug.LineFromLocation = new Vector2(bug.Paths.Last().EndPoint.X, bug.Paths.Last().EndPoint.Y);
                    //bug.LineToLocation = bug.DiveBombLocation;
                }
                else
                {
                    var homepoint = BugFactory.EnemyGrid.GetPointByRowCol(bug.HomePoint.X, bug.HomePoint.Y);
                    if (!(homepoint.X == 0 && homepoint.Y == 0))
                    {
                        if (bug.IsMoving)
                        {
                            //this animates the line to location logic
                            //bug.LineFromLocation = new Vector2(bug.Paths.Last().EndPoint.X, bug.Paths.Last().EndPoint.Y);
                            //bug.LineToLocation = new Vector2(homepoint.X, homepoint.Y);
                            bug.PathPoints[bug.PathPoints.Count - 1] = homepoint;
                        }
                        //snap to grid if bug isn't moving
                        else
                        {
                            //bug.LineToLocation = new Vector2(homepoint.X, homepoint.Y);
                            bug.Location = homepoint;
                        }
                    }
                }
            });
        }
    }
}
