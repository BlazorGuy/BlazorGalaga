using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;

namespace BlazorGalaga.Static
{
    public static class BugFactory
    {
        private static EnemyGrid enemyGrid = new EnemyGrid();
        public static List<IAnimatable> CreateAnimation_BugIntro1()
        {
            List<IAnimatable> animatables = new List<IAnimatable>();

            for (int i = 0; i < 4; i++)
                animatables.Add(CreateAnimatable_BugIntro1(i, i * Constants.BugIntroSpacing, new Intro1()));

            for (int i = 0; i < 4; i++)
                animatables.Add(CreateAnimatable_BugIntro1(i+4, i * Constants.BugIntroSpacing, new Intro2()));

            return animatables;
        }

        public static IAnimatable CreateAnimatable_BugIntro1(int index, int startdelay, IIntro intro)
        {

            var bug = new Bug()
            {
                Index = index,
                Paths = intro.GetPaths(),
                RotateAlongPath = true,
                Speed = Constants.BugIntroSpeed,
                StartDelay = startdelay,
            };

            //add an offscreen path to make the bug fly in from off screen
            bug.Paths.Insert(0, new BezierCurve() {
                StartPoint = new PointF(bug.Paths[0].StartPoint.X,bug.Paths[0].StartPoint.Y - 400),
                EndPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y),
                ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - 400),
                ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y)
            });

            //add the bugs destination path from the enemy grid
            bug.Paths.Add(GetGridPath(bug.Paths[bug.Paths.Count-1].EndPoint,index));

            return bug;

        }

        private static BezierCurve GetGridPath(PointF startpoint, int index)
        {
            PointF endpoint = new PointF(0,0);

            switch (index)
            {
                case 0:
                    endpoint = enemyGrid.GetPointByRowCol(3, 4);
                    break;
                case 1:
                    endpoint = enemyGrid.GetPointByRowCol(3, 5);
                    break;
                case 2:
                    endpoint = enemyGrid.GetPointByRowCol(2, 4);
                    break;
                case 3:
                    endpoint = enemyGrid.GetPointByRowCol(2, 5);
                    break;
                case 4:
                    endpoint = enemyGrid.GetPointByRowCol(5, 5);
                    break;
                case 5:
                    endpoint = enemyGrid.GetPointByRowCol(5, 6);
                    break;
                case 6:
                    endpoint = enemyGrid.GetPointByRowCol(4, 5);
                    break;
                case 7:
                    endpoint = enemyGrid.GetPointByRowCol(4, 6);
                    break;
            }

            return new BezierCurve()
            {
                StartPoint = startpoint,
                EndPoint = endpoint,
                ControlPoint1 = startpoint,
                ControlPoint2 = endpoint
            };
        }
    }
}
