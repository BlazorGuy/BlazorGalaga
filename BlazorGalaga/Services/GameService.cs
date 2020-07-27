using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using BlazorGalaga.Interfaces;
using BlazorGalaga.Models;
using BlazorGalaga.Models.Paths;
using BlazorGalaga.Static;
using BlazorGalaganimatable.Models.Paths;

namespace BlazorGalaga.Services
{
    public class GameService
    {
        public AnimationService animationService { get; set; }
        public SpriteService spriteService { get; set; }
        public Ship Ship { get; set; }
        public int Lives { get; set; }

        private bool levelInitialized = false;
        private bool consoledrawn = false;
        private int WingFlapCount;
        private int MoveEnemyGridIncrement = 3;
        private float LastEnemyGridMoveTimeStamp = 0;
        private float LastWingFlapTimeStamp = 0;

        public void Init()
        {
            Lives = 2;
            InitShip();
        }

        private void MoveChildBugs()
        {
            GetBugs().ForEach(bug =>
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
                        if(childbug.HomePoint.Y == bug.HomePoint.Y + 1)
                            childbug.Location = new PointF(bug.Location.X + 35, bug.Location.Y + 35);
                        else
                            childbug.Location = new PointF(bug.Location.X - 35, bug.Location.Y - 35);
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
                        childbug.Paths.Add(new BezierCurve() {
                            StartPoint = childbug.Location,
                            ControlPoint1 = childbug.Location,
                            ControlPoint2 = childbug.Location,
                            EndPoint = new PointF(childbug.Location.X + 10, childbug.Location.Y + 10)});
                        childbug.PathPoints.AddRange(animationService.ComputePathPoints(childbug.Paths.First()));
                    });
                    bug.ChildBugs.Clear();
                }
            });
        }

        private void MoveEnemyGrid()
        {
            var bugs = GetBugs();

            if (BugFactory.EnemyGrid.GridLeft > 350 || BugFactory.EnemyGrid.GridLeft < 180)
                MoveEnemyGridIncrement *= -1;
            
            BugFactory.EnemyGrid.GridLeft += MoveEnemyGridIncrement;

            bugs.ForEach(bug =>
            {
                var homepoint = BugFactory.EnemyGrid.GetPointByRowCol(bug.HomePoint.X, bug.HomePoint.Y);
                if (bug.IsMoving)
                {
                    //this animates the line to location logic
                    bug.LineFromLocation = new Vector2(bug.Paths.Last().EndPoint.X, bug.Paths.Last().EndPoint.Y);
                    bug.LineToLocation = new Vector2(homepoint.X, homepoint.Y);
                }
                //snap to grid if bug isn't moving
                else
                {
                    bug.LineToLocation = new Vector2(homepoint.X,homepoint.Y);
                    //bug.Location = homepoint;
                }
            });
        }

        private void FlapWings()
        {
            var bugs = GetBugs();

            WingFlapCount++;

            bugs.Where(a => a.Started).ToList().ForEach(bug =>
              {
                  if (bug.IsMoving)
                  {
                      if (bug.IsDiving)
                          bug.SpriteBankIndex = Utils.Rnd(1, 10) > 6 ? null : (int?)0;
                      else
                          bug.SpriteBankIndex = WingFlapCount % 4 == 0 ? null : (int?)0;
                  }
                  else
                      bug.SpriteBankIndex = WingFlapCount % 2 == 0 ? null : (int?)0;
              });
        }

        private void InitLevel(int level)
        {
            switch (level)
            {
                case 1:
                    //animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(2));
                    //animationService.ComputePathPoints();
                    //animationService.Animatables.ForEach(a => a.Started = true);
                    //levelInitialized = true;
                    //return;

                    //creates two top down bug lines of 4 each, these enter at the same time
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(1));
                    //creates two side bug lines of 8 each, these enter one after the other
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(2));
                    //creates two top down bug lines of 8 each, these enter one after the other
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(3));
                    animationService.ComputePathPoints();

                    Task.Delay(2000).ContinueWith((task) =>
                    {
                        animationService.Animatables.Where(a => a.Index < 8).ToList().ForEach(a => a.Started = true);
                    });
                    //Task.Delay(6000).ContinueWith((task) =>
                    //{
                    //    DoEnemyDive();
                    //});
                    Task.Delay(5000).ContinueWith((task) =>
                    {
                        animationService.Animatables.Where(a => a.Index >= 8 && a.Index < 16).ToList().ForEach(a => a.Started = true);
                    });

                    Task.Delay(9000).ContinueWith((task) =>
                    {
                        animationService.Animatables.Where(a => a.Index >= 16 && a.Index < 24).ToList().ForEach(a => a.Started = true);
                    });

                    Task.Delay(14000).ContinueWith((task) =>
                    {
                        animationService.Animatables.Where(a => a.Index >= 24 && a.Index < 32).ToList().ForEach(a => a.Started = true);
                    });

                    Task.Delay(18000).ContinueWith((task) =>
                    {
                        animationService.Animatables.Where(a => a.Index >= 32 && a.Index < 40).ToList().ForEach(a => a.Started = true);
                    });
                    Task.Delay(22000).ContinueWith((task) =>
                    {
                        for (int i = 0; i <= 100; i++)
                        {
                            Task.Delay(i * 3000).ContinueWith((task) =>
                            {
                                DoEnemyDive();
                            });
                        }
                    });
                    break;
            }

            levelInitialized = true;
        }

        private void DoEnemyDive()
        {
            var bugs = GetBugs();
            Bug bug = null;

            while(bug == null || bug.IsMoving)
            {
                bug = bugs.FirstOrDefault(a => a.Index == Utils.Rnd(0, bugs.Count - 1));
            }

            bug.SpriteBankIndex = null;
            bug.IsDiving = true;

            IDive dive = null;
            //dive = new GreenBugDive1();

            if (bug.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug)
            {
                if (Utils.Rnd(0, 10) % 2 == 0)
                    dive = new BlueBugDive1();
                else
                    dive = new BlueBugDive2();
            }
            else if (bug.Sprite.SpriteType == Sprite.SpriteTypes.RedBug)
            {
                if (Utils.Rnd(0, 10) % 2 == 0)
                    dive = new RedBugDive1();
                else
                    dive = new RedBugDive2();
            }
            else if (bug.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug)
            {
                var childbugs = bugs.Where(a =>
                    (a.HomePoint == new Point(2, bug.HomePoint.Y+1) ||
                    a.HomePoint == new Point(2, bug.HomePoint.Y+2))
                    && !a.IsMoving);

                bug.ChildBugs.AddRange(childbugs);

                if (Utils.Rnd(0, 10) % 2 == 0)
                    dive = new GreenBugDive1();
                else
                    dive = new GreenBugDive2();
            }
            else
            {
                dive = new RedBugDive1();
            }

            animationService.Animatables.ForEach(a => a.ZIndex = 0);

            var paths = dive.GetPaths(bug, Ship);

            bug.RotateAlongPath = true;
            bug.ZIndex = 100;
            bug.Speed = 5;
            bug.Paths.AddRange(paths);

            paths.ForEach(p => {
                p.DrawPath = true;
                bug.PathPoints.AddRange(animationService.ComputePathPoints(p));
            });
        }

        private List<Bug> GetBugs()
        {
            return animationService.Animatables.Where(a =>
                a.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug
                || a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug
                || a.Sprite.SpriteType == Sprite.SpriteTypes.RedBug
            ).Select(a=> a as Bug).ToList();
        }

        private void DoFireFromShip()
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(Ship.Location.X + (Ship.Sprite.SpriteDestRect.Width / 2)-14, Ship.Location.Y-5),
                EndPoint = new PointF(Ship.Location.X + (Ship.Sprite.SpriteDestRect.Width / 2)-16, -14)
            });

            var missle = new ShipMissle()
            {
                Paths = paths,
                DrawPath = false,
                PathIsLine = true,
                RotateAlongPath = false,
                Started = true,
                Speed=10
            };

            missle.Paths.ForEach(p => {
                missle.PathPoints.AddRange(animationService.ComputePathPoints(p,true));
            });

            animationService.Animatables.Add(missle);
        }

        private async Task DrawConsole()
        {
            //draw the ships
            int left = 5;
            for (var i = 1; i <= Lives; i++)
            {
                await spriteService.StaticCtx.DrawImageAsync(
                    Ship.Sprite.BufferCanvas.Canvas,
                    left,
                    Constants.CanvasSize.Height - Ship.Sprite.SpriteDestRect.Height - 5
                );
                left += (int)Ship.Sprite.SpriteDestRect.Width + 10;
            }

            //draw the badges
            await spriteService.StaticCtx.DrawImageAsync(
                spriteService.SpriteSheet,
                305,
                175,
                10,
                16,
                Constants.CanvasSize.Width-30,
                Constants.CanvasSize.Height-45,
                28,
                45
            );

            //await spriteService.StaticCtx.SetStrokeStyleAsync("red");
            //await spriteService.StaticCtx.SetLineWidthAsync(1);
            await spriteService.StaticCtx.SetFillStyleAsync("Red");
            await spriteService.StaticCtx.SetFontAsync("32px Sarif");

            //await spriteService.StaticCtx.StrokeTextAsync("This Is A Test", 50, 25);
            await spriteService.StaticCtx.FillTextAsync("1UP", 50, 25);
            await spriteService.StaticCtx.FillTextAsync("HIGH SCORE", 250, 25);

            await spriteService.StaticCtx.SetFillStyleAsync("White");
            await spriteService.StaticCtx.FillTextAsync("00", 50, 50);
            await spriteService.StaticCtx.FillTextAsync("20000", 300, 50);

            consoledrawn = true;
        }

        private void InitShip()
        {
            List<BezierCurve> paths = new List<BezierCurve>();

            paths.Add(new BezierCurve()
            {
                StartPoint = new PointF(Constants.SpriteDestSize.Width / 2, Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
                EndPoint = new PointF(Constants.CanvasSize.Width - (Constants.SpriteDestSize.Width / 2), Constants.CanvasSize.Height - (Constants.SpriteDestSize.Height * 2)),
            });

            var ship = new Ship()
            {
                Paths = paths,
                DrawPath = false,
                PathIsLine = true,
                RotateAlongPath = false,
                Started = true
            };

            ship.Paths.ForEach(a => {
                ship.PathPoints.AddRange(animationService.ComputePathPoints(a));
            });

            ship.CurPathPointIndex = (int)(ship.PathPoints.Count / 3)- (int)(ship.Sprite.SpriteDestRect.Width/2);

            animationService.Animatables.Add(ship);
        }

        int firecount = 0;

        public async void Process(Ship ship, float timestamp)
        {
            //Begin Init - Only happens once
            if (!levelInitialized)
            {
                this.Ship = ship;
                InitLevel(1);
            }

            if (!consoledrawn && Ship.Sprite.BufferCanvas != null)
                await DrawConsole();
            //End Init - Only happens once

            MoveChildBugs();

            if (timestamp - LastEnemyGridMoveTimeStamp > 100 || LastEnemyGridMoveTimeStamp == 0)
            {
                MoveEnemyGrid();
                LastEnemyGridMoveTimeStamp = timestamp;
            }

            if (timestamp - LastWingFlapTimeStamp > 500 || LastWingFlapTimeStamp == 0)
            {
                FlapWings();
                LastWingFlapTimeStamp = timestamp;
            }

            if (ship.IsFiring)
            {
                firecount += 1;
                ship.IsFiring = false;
                DoFireFromShip();
            }

        }
    }
}
