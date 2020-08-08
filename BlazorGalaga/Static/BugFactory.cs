using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;
using BlazorGalaga.Models.Paths.Challenges;

namespace BlazorGalaga.Static
{
    public static class BugFactory
    {
        public enum BugIntro
        {
            TwoGroupsOfFourFromTop,
            TwoGroupsOfEightFromBottom,
            TwoGroupsOfEightFromTop,
            TwoGroupsOfStackedEightFromBottom,
            TwoGroupsOfStackedEightFromTop,
            Challenge1_TwoGroupsOfFourFromTop,
            Challenge1_TwoGroupsOfEightFromBottom,
            Challenge1_TwoGroupsOfEightFromTop,
        }

        public static EnemyGrid EnemyGrid = new EnemyGrid();
        public static List<IAnimatable> CreateAnimation_BugIntro(BugIntro intro)
        {
            List<IAnimatable> animatables = new List<IAnimatable>();

            switch (intro)
            {
                case BugIntro.TwoGroupsOfFourFromTop:
                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing, new Intro2(),Sprite.SpriteTypes.RedBug));
                    break;
                case BugIntro.TwoGroupsOfEightFromBottom:
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 8, i * Constants.BugIntroSpacing, new Intro3(), i % 2 != 0 ? Sprite.SpriteTypes.GreenBug : Sprite.SpriteTypes.RedBug));

                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Intro4(), Sprite.SpriteTypes.RedBug));
                    break;
                case BugIntro.TwoGroupsOfEightFromTop:
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug));
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Intro2(), Sprite.SpriteTypes.BlueBug));
                    break;
                case BugIntro.TwoGroupsOfStackedEightFromBottom:
                    for (int i = 1; i < 8; i+=2)
                        animatables.Add(CreateAnimatable_BugIntro(i + 8, (i * Constants.BugIntroSpacing) / 2, new Intro3(), Sprite.SpriteTypes.GreenBug));

                    for (int i = 0; i < 8; i+=2)
                        animatables.Add(CreateAnimatable_BugIntro(i + 8, (i * Constants.BugIntroSpacing) / 2, new Intro5(), Sprite.SpriteTypes.RedBug));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Intro4(), Sprite.SpriteTypes.RedBug));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 20, i * Constants.BugIntroSpacing, new Intro6(), Sprite.SpriteTypes.RedBug));
                    break;
                case BugIntro.TwoGroupsOfStackedEightFromTop:
                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Intro2(), Sprite.SpriteTypes.BlueBug));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 28, i * Constants.BugIntroSpacing, new Intro7(), Sprite.SpriteTypes.BlueBug));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Intro1(), Sprite.SpriteTypes.BlueBug));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 36, i * Constants.BugIntroSpacing, new Intro8(), Sprite.SpriteTypes.BlueBug));
                    break;
                case BugIntro.Challenge1_TwoGroupsOfFourFromTop:
                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i, i * Constants.BugIntroSpacing, new Challenge1(), Sprite.SpriteTypes.BlueBug));

                    for (int i = 0; i < 4; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 4, i * Constants.BugIntroSpacing, new Challenge2(), Sprite.SpriteTypes.BlueBug));
                    break;
                case BugIntro.Challenge1_TwoGroupsOfEightFromBottom:
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 8, i * Constants.BugIntroSpacing, new Challenge3(), i % 2 != 0 ? Sprite.SpriteTypes.GreenBug : Sprite.SpriteTypes.BlueBug));

                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 16, i * Constants.BugIntroSpacing, new Challenge4(), Sprite.SpriteTypes.BlueBug));
                    break;
                case BugIntro.Challenge1_TwoGroupsOfEightFromTop:
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 24, i * Constants.BugIntroSpacing, new Challenge5(), Sprite.SpriteTypes.BlueBug));
                    for (int i = 0; i < 8; i++)
                        animatables.Add(CreateAnimatable_BugIntro(i + 32, i * Constants.BugIntroSpacing, new Challenge6(), Sprite.SpriteTypes.BlueBug));
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

            if ((intro as Intro5) != null || (intro as Intro6) != null || (intro as Intro7) != null || (intro as Intro8) != null)
            {
                bug.VSpeed = new List<VSpeed>
                {
                    new VSpeed()
                    {
                        PathPointIndex = 1000,
                        Speed = Constants.BugIntroSpeed - 2
                    },
                    new VSpeed()
                    {
                        PathPointIndex = 1300,
                        Speed = Constants.BugIntroSpeed - 3
                    }
                };
            }

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

            if (intro as Intro1 != null || intro as Intro2 != null || intro as Intro7 != null
                || (intro as Intro8) != null || (intro as Challenge1) != null
                || (intro as Challenge2) != null || (intro as Challenge5) != null || (intro as Challenge6) != null)
                //For bugs dropping from the top, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - 400),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y - 400),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y)
                });
            else if (intro as Intro3 != null || intro as Intro5 != null || intro as Challenge3 != null)
                //For bugs coming from the left side, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X - 800, bug.Paths[0].StartPoint.Y),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X - 800, bug.Paths[0].StartPoint.Y),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y)
                });
            else if (intro as Intro4 != null || intro as Intro6 != null || intro as Challenge4 != null)
                //For bugs coming from the right side, add an offscreen path to make the bug fly in from off screen
                bug.Paths.Insert(0, new BezierCurve()
                {
                    StartPoint = new PointF(bug.Paths[0].StartPoint.X + 800, bug.Paths[0].StartPoint.Y),
                    EndPoint = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y),
                    ControlPoint1 = new PointF(bug.Paths[0].StartPoint.X + 800, bug.Paths[0].StartPoint.Y),
                    ControlPoint2 = new PointF(bug.Paths[0].StartPoint.X, bug.Paths[0].StartPoint.Y)
                });

            if (!intro.IsChallenge)
                bug.HomePoint = GetGridPoint(index);
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
