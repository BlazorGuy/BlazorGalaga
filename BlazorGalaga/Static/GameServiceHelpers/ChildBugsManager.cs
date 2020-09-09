using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class ChildBugsManager
    {
        public static void MoveChildBugs(List<Bug> bugs, AnimationService animationService)
        {
            foreach(var bug in bugs)
            {
                if (bug != null && bug.ChildBugs != null && bug.ChildBugs.Count > 0)
                {
                    //is the parent bug still moving?
                    if (bug.CurPathPointIndex < bug.PathPoints.Count - 1)
                    {
                        //make the child bugs follow along
                        foreach (var childbug in bug.ChildBugs)
                        {
                            childbug.IsMoving = true;
                            childbug.RotateAlongPath = true;
                            if (childbug.HomePoint.Y == bug.HomePoint.Y + 1)
                                childbug.Location = new PointF(bug.Location.X + bug.ChildBugOffset.X, bug.Location.Y + bug.ChildBugOffset.Y);
                            else
                                childbug.Location = new PointF(bug.Location.X - bug.ChildBugOffset.X, bug.Location.Y + bug.ChildBugOffset.Y);
                            childbug.Rotation = bug.Rotation;
                        };
                    }
                    else
                    {
                        //the parent bug has stopped moving
                        foreach (var childbug in bug.ChildBugs)
                        {
                            childbug.PathPoints = new List<PointF>();
                            childbug.Paths = new List<BezierCurve>();
                            childbug.RotateAlongPath = true;
                            childbug.IsMoving = true;
                            childbug.Speed = 5;
                            childbug.Paths.Add(new BezierCurve()
                            {
                                StartPoint = childbug.Location,
                                ControlPoint1 = childbug.Location,
                                ControlPoint2 = childbug.Location,
                                EndPoint = BugFactory.EnemyGrid.GetPointByRowCol(childbug.HomePoint.X,childbug.HomePoint.Y) //new PointF(childbug.Location.X + 10, childbug.Location.Y + 10)
                            });
                            childbug.PathPoints.AddRange(animationService.ComputePathPoints(childbug.Paths.First(), true));
                        }
                        bug.ChildBugs.Clear();
                    }
                }
            }


            //start captured bug logic
            var bugwithcapturedbug = bugs.FirstOrDefault(a => a.CapturedBug != null && a.CaptureState == Bug.enCaptureState.Complete);

            if (bugwithcapturedbug != null)
            {
                if (bugwithcapturedbug.IsMoving)
                {
                    bugwithcapturedbug.CapturedBug.IsMoving = true;
                    bugwithcapturedbug.CapturedBug.RotateAlongPath = true;
                    bugwithcapturedbug.CapturedBug.Location = new PointF(bugwithcapturedbug.Location.X, bugwithcapturedbug.Location.Y -50);
                    bugwithcapturedbug.CapturedBug.Rotation = bugwithcapturedbug.Rotation;
                }
                else
                    bugwithcapturedbug.CapturedBug.RotateAlongPath = false;
            }
            //end captured bug logic
        }
    }
}
