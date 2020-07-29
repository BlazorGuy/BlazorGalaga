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
            bugs.ForEach(bug =>
            {
                if (bug != null && bug.ChildBugs != null)
                {
                    if (bug.CurPathPointIndex < bug.PathPoints.Count - 1)
                    {
                        bug.ChildBugs.ForEach(childbug =>
                        {
                            childbug.CurPathPointIndex = 0;
                            childbug.PathPoints.Add(new PointF(0, 0));
                            childbug.Speed = 0;
                            childbug.IsMoving = true;
                            childbug.RotateAlongPath = true;
                            if (childbug.HomePoint.Y == bug.HomePoint.Y + 1)
                                childbug.Location = new PointF(bug.Location.X + bug.ChildBugOffset.X, bug.Location.Y + bug.ChildBugOffset.Y);
                            else
                                childbug.Location = new PointF(bug.Location.X - bug.ChildBugOffset.X, bug.Location.Y + bug.ChildBugOffset.Y);
                            childbug.Rotation = bug.Rotation;
                        });
                    }
                    else
                    {
                        bug.ChildBugs.ForEach(childbug =>
                        {
                            childbug.PathPoints.Clear();
                            childbug.Paths.Clear();
                            childbug.LineToLocationDistance = 0;
                            childbug.RotateAlongPath = true;
                            childbug.IsMoving = true;
                            childbug.Speed = 5;
                        //add a minimum path that is 2X the speed just to kick off the line to location logic
                        childbug.Paths.Add(new BezierCurve()
                            {
                                StartPoint = childbug.Location,
                                ControlPoint1 = childbug.Location,
                                ControlPoint2 = childbug.Location,
                                EndPoint = new PointF(childbug.Location.X + 10, childbug.Location.Y + 10)
                            });
                            childbug.PathPoints.AddRange(animationService.ComputePathPoints(childbug.Paths.First()));
                        });
                        bug.ChildBugs.Clear();
                    }
                }
            });
        }
    }
}
