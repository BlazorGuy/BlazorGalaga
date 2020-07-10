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
            {
                animatables.Add(CreateAnimatable_BugIntro1(i));
            }

            return animatables;
        }

        public static IAnimatable CreateAnimatable_BugIntro1(int index)
        {

            var bug = new Bug()
            {
                Paths = Intro1.GetPaths(),
                RotateAlongPath = true,
                Speed=10,
                //LoopBack = true,
                StartDelay = index * 60,
            };

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
