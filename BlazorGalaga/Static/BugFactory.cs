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
        public static EnemyGrid EnemyGrid = new EnemyGrid();
        public static List<IAnimatable> CreateAnimation_BugIntro(int introindex)
        {
            List<IAnimatable> animatables = new List<IAnimatable>();

            switch (introindex)
            {
                case 1:
                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing, new Intro2(),Sprite.SpriteTypes.RedBug));
                    break;
                case 2:
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 8, i * Constants.BugIntroSpacing, new Intro3(), i % 2 != 0 ? Sprite.SpriteTypes.GreenBug : Sprite.SpriteTypes.RedBug));

                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Intro4(), Sprite.SpriteTypes.RedBug));
                    break;
                case 3:
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug));
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Intro2(), Sprite.SpriteTypes.BlueBug));
                    break;
            }

            return animatables;
        }

        public static IAnimatable CreateAnimatable_BugIntro(int index, int startdelay, IIntro intro, Sprite.SpriteTypes spritetype)
        {
            var bug = new Bug(spritetype)
            {
                Index = index,
                Paths = intro.GetPaths(),
                RotateAlongPath = true,
                Speed = Constants.BugIntroSpeed,
                StartDelay = startdelay,
                Started = false,
                ZIndex = 100,
                RotatIntoPlaceSpeed = Constants.BugRotateIntoPlaceSpeed
            };

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

            bug.HomePoint = GetGridPoint(index);

            return bug;

        }

        private static Point GetGridPoint(int index)
        {

            switch (index)
            {
                case 0:
                    return new Point(5, 5);
                case 1:
                   return new Point(5, 6);
                case 2:
                   return new Point(4, 5);
                case 3:
                   return new Point(4, 6);
                case 4:
                   return new Point(3, 4);
                case 5:
                   return new Point(3, 5);
                case 6:
                   return new Point(2, 4);
                case 7:
                   return new Point(2, 5);


                case 8:
                   return new Point(2, 6);
                case 9:
                   return new Point(1, 1);
                case 10:
                   return new Point(3, 6);
                case 11:
                   return new Point(1, 2);
                case 12:
                   return new Point(3, 3);
                case 13:
                   return new Point(1, 3);
                case 14:
                   return new Point(2, 3);
                case 15:
                   return new Point(1, 4);

                case 16:
                   return new Point(2, 7);
                case 17:
                   return new Point(2, 8);
                case 18:
                   return new Point(3, 7);
                case 19:
                   return new Point(3, 8);
                case 20:
                   return new Point(2, 1);
                case 21:
                   return new Point(2, 2);
                case 22:
                   return new Point(3, 1);
                case 23:
                   return new Point(3, 2);

                case 24:
                   return new Point(4, 7);
                case 25:
                   return new Point(4, 8);
                case 26:
                   return new Point(5, 7);
                case 27:
                   return new Point(5, 8);
                case 28:
                   return new Point(4, 4);
                case 29:
                   return new Point(4, 3);
                case 30:
                   return new Point(5, 4);
                case 31:
                   return new Point(5, 3);

                case 32:
                   return new Point(4, 2);
                case 33:
                   return new Point(4, 1);
                case 34:
                   return new Point(5, 2);
                case 35:
                   return new Point(5, 1);
                case 36:
                   return new Point(4, 9);
                case 37:
                   return new Point(4, 10);
                case 38:
                   return new Point(5, 9);
                case 39:
                   return new Point(5, 10);

                default:
                    return new Point(0,0);
            }
        }
    }
}
