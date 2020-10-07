using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths.Intros;
using System.Linq;

namespace BlazorGalaga.Static
{
    public static class BugFactory
    {
        public static EnemyGrid EnemyGrid = new EnemyGrid();

        public static IAnimatable CreateAnimatable_BugIntro(
            int index,
            int startdelay,
            IIntro intro,
            Sprite.SpriteTypes spritetype,
            int wave,
            int introspeedincrease,
            bool isdivebomber = false)
        {
            var bug = new Bug(spritetype)
            {
                Index = isdivebomber ? -1 : index,
                Paths = intro.GetPaths(),
                RotateAlongPath = true,
                Speed = Constants.BugIntroSpeed + introspeedincrease,
                StartDelay = startdelay,
                Started = false,
                ZIndex = 100,
                RotatIntoPlaceSpeed = Constants.BugRotateIntoPlaceSpeed,
                Wave = wave,
                IsInIntro = true
            };

            bug.MissileCountDowns.Add(Utils.Rnd(1, 10));

            if ((intro as Intro5) != null || (intro as Intro6) != null)
            {
                bug.VSpeed = new List<VSpeed>
                {
                    new VSpeed()
                    {
                        PathPointIndex = 20,
                        Speed = Constants.BugIntroSpeed - 2
                    },
                    new VSpeed()
                    {
                        PathPointIndex = 30,
                        Speed = Constants.BugIntroSpeed - 3
                    }
                };
            }
            //if ((intro as Intro7) != null || (intro as Intro8) != null)
            //{
            //    bug.VSpeed = new List<VSpeed>
            //    {
            //        new VSpeed()
            //        {
            //            PathPointIndex = 10,
            //            Speed = Constants.BugIntroSpeed - 2
            //        },
            //        new VSpeed()
            //        {
            //            PathPointIndex = 15,
            //            Speed = Constants.BugIntroSpeed - 3
            //        }
            //    };
            //}

            switch (spritetype)
            {
                case Sprite.SpriteTypes.BlueBug:
                    bug.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.BlueBug_DownFlap));
                    break;
                case Sprite.SpriteTypes.RedBug:
                    bug.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.RedBug_DownFlap));
                    break;
                case Sprite.SpriteTypes.GreenBug:
                    bug.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.GreenBug_DownFlap));
                    break;
            }

            if (intro.IntroLocation == IntroLocation.Top)
            {
                //For bugs dropping from the top, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - (1000 + bug.StartDelay)),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - 50),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - 1000),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - 50),
                    IsIntroPath = true
                });
            }
            else if (intro.IntroLocation == IntroLocation.LowerLeft)
            {
                //For bugs coming from the left side, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X - (1000 + bug.StartDelay), bug.Paths[0].StartPoint.Y),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X - 50, bug.Paths[0].StartPoint.Y),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X - 1000, bug.Paths[0].StartPoint.Y),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X - 50, bug.Paths[0].StartPoint.Y),
                    IsIntroPath = true
                });
            }
            else if (intro.IntroLocation == IntroLocation.LowerRight)
            {
                //For bugs coming from the right side, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X + (1000 + bug.StartDelay), bug.Paths[0].StartPoint.Y),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X + 50, bug.Paths[0].StartPoint.Y),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X + 1000, bug.Paths[0].StartPoint.Y),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X + 50, bug.Paths[0].StartPoint.Y),
                    IsIntroPath = true
                });
            }

            if (!intro.IsChallenge)
            {
                if (isdivebomber)
                {
                    bug.IsDiveBomber = true;
                    bug.DiveBombLocation = new PointF(Utils.Rnd(50, Constants.CanvasSize.Width - 50), Constants.CanvasSize.Height + 50);
                    bug.Paths.Last().EndPoint = new PointF(bug.Paths.Last().EndPoint.X , bug.Paths.Last().EndPoint.Y - 100);
                }
                else
                    bug.HomePoint = GetGridPoint(index);
            }
            else
                bug.DoLineToLocation = false;

            return bug;

        }

        private static Point GetGridPoint(int index)
        {

            return index switch
            {
                0 => new Point(5, 5),
                1 => new Point(5, 6),
                2 => new Point(4, 5),
                3 => new Point(4, 6),
                4 => new Point(3, 4),
                5 => new Point(3, 5),
                6 => new Point(2, 4),
                7 => new Point(2, 5),
                8 => new Point(2, 6),
                9 => new Point(1, 1),
                10 => new Point(3, 6),
                11 => new Point(1, 2),
                12 => new Point(3, 3),
                13 => new Point(1, 3),
                14 => new Point(2, 3),
                15 => new Point(1, 4),
                16 => new Point(2, 7),
                17 => new Point(2, 8),
                18 => new Point(3, 7),
                19 => new Point(3, 8),
                20 => new Point(2, 1),
                21 => new Point(2, 2),
                22 => new Point(3, 1),
                23 => new Point(3, 2),
                24 => new Point(4, 7),
                25 => new Point(4, 8),
                26 => new Point(5, 7),
                27 => new Point(5, 8),
                28 => new Point(4, 4),
                29 => new Point(4, 3),
                30 => new Point(5, 4),
                31 => new Point(5, 3),
                32 => new Point(4, 2),
                33 => new Point(4, 1),
                34 => new Point(5, 2),
                35 => new Point(5, 1),
                36 => new Point(4, 9),
                37 => new Point(4, 10),
                38 => new Point(5, 9),
                39 => new Point(5, 10),
                _ => new Point(0, 0),
            };
        }
    }
}
