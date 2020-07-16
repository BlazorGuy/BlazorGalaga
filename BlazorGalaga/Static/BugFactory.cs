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
        public static List<IAnimatable> CreateAnimation_BugIntro(int introindex)
        {
            List<IAnimatable> animatables = new List<IAnimatable>();

            switch (introindex)
            {
                case 1:
                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i, i * Constants.BugIntroSpacing, new Intro1()));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing, new Intro2()));
                    break;
                case 2:
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 8, i * Constants.BugIntroSpacing, new Intro3()));

                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Intro4()));
                    break;
                case 3:
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Intro1()));
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Intro2()));
                    break;
            }

            return animatables;
        }

        public static IAnimatable CreateAnimatable_BugIntro(int index, int startdelay, IIntro intro)
        {
            var bug = new Bug()
            {
                Index = index,
                Paths = intro.GetPaths(),
                RotateAlongPath = true,
                Speed = Constants.BugIntroSpeed,
                StartDelay = startdelay,
                Started = false
            };

            if (index < 8 || index >= 24)
                //For bugs dropping from the top, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - 400),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - 400),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y)
                });
            else if (index >=8 && index <16)
                //For bugs coming from the left side, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X - 800, bug.Paths[0].StartPoint.Y),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X - 800, bug.Paths[0].StartPoint.Y),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y)
                });
            else if (index >= 16 && index < 25)
                //For bugs coming from the right side, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X + 800, bug.Paths[0].StartPoint.Y),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X + 800, bug.Paths[0].StartPoint.Y),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y)
                });

            //add the bugs destination path from the enemy grid
            //bug.Paths.Add(GetGridPath(bug.Paths[bug.Paths.Count - 1].EndPoint, index));

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

                case 8:
                    endpoint = enemyGrid.GetPointByRowCol(2, 3);
                    break;
                case 9:
                    endpoint = enemyGrid.GetPointByRowCol(2, 6);
                    break;
                case 10:
                    endpoint = enemyGrid.GetPointByRowCol(3, 3);
                    break;
                case 11:
                    endpoint = enemyGrid.GetPointByRowCol(3, 6);
                    break;
                case 12:
                    endpoint = enemyGrid.GetPointByRowCol(1, 1);
                    break;
                case 13:
                    endpoint = enemyGrid.GetPointByRowCol(1, 2);
                    break;
                case 14:
                    endpoint = enemyGrid.GetPointByRowCol(1, 3);
                    break;
                case 15:
                    endpoint = enemyGrid.GetPointByRowCol(1, 4);
                    break;

                case 16:
                    endpoint = enemyGrid.GetPointByRowCol(2, 7);
                    break;
                case 17:
                    endpoint = enemyGrid.GetPointByRowCol(2, 8);
                    break;
                case 18:
                    endpoint = enemyGrid.GetPointByRowCol(3, 7);
                    break;
                case 19:
                    endpoint = enemyGrid.GetPointByRowCol(3, 8);
                    break;
                case 20:
                    endpoint = enemyGrid.GetPointByRowCol(2, 1);
                    break;
                case 21:
                    endpoint = enemyGrid.GetPointByRowCol(2, 2);
                    break;
                case 22:
                    endpoint = enemyGrid.GetPointByRowCol(3, 1);
                    break;
                case 23:
                    endpoint = enemyGrid.GetPointByRowCol(3, 2);
                    break;

                case 24:
                    endpoint = enemyGrid.GetPointByRowCol(4, 7);
                    break;
                case 25:
                    endpoint = enemyGrid.GetPointByRowCol(4, 8);
                    break;
                case 26:
                    endpoint = enemyGrid.GetPointByRowCol(5, 7);
                    break;
                case 27:
                    endpoint = enemyGrid.GetPointByRowCol(5, 8);
                    break;
                case 28:
                    endpoint = enemyGrid.GetPointByRowCol(4, 4);
                    break;
                case 29:
                    endpoint = enemyGrid.GetPointByRowCol(4, 3);
                    break;
                case 30:
                    endpoint = enemyGrid.GetPointByRowCol(5, 4);
                    break;
                case 31:
                    endpoint = enemyGrid.GetPointByRowCol(5, 3);
                    break;

                case 32:
                    endpoint = enemyGrid.GetPointByRowCol(4, 2);
                    break;
                case 33:
                    endpoint = enemyGrid.GetPointByRowCol(4, 1);
                    break;
                case 34:
                    endpoint = enemyGrid.GetPointByRowCol(5, 2);
                    break;
                case 35:
                    endpoint = enemyGrid.GetPointByRowCol(5, 1);
                    break;
                case 36:
                    endpoint = enemyGrid.GetPointByRowCol(4, 9);
                    break;
                case 37:
                    endpoint = enemyGrid.GetPointByRowCol(4, 10);
                    break;
                case 38:
                    endpoint = enemyGrid.GetPointByRowCol(5, 9);
                    break;
                case 39:
                    endpoint = enemyGrid.GetPointByRowCol(5, 10);
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
