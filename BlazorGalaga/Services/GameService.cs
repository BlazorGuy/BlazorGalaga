using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        public Ship Ship { get; set; }

        private bool levelInitialized = false;

        public void Init()
        {
            InitShip();
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
            Utils.dOut("diving 1", "");
            while(bug == null || bug.IsMoving)
            {
                bug = bugs.FirstOrDefault(a => a.Index == Utils.Rnd(0, bugs.Count - 1));
            } 

            Utils.dOut("diving 2", "");

            IDive dive;
            //dive = new Dive2();
            if (Utils.Rnd(0, 10) % 2 == 0)
                dive = new BlueBugDive1();
            else
                dive = new BlueBugDive2();

            var paths = dive.GetPaths(bug, Ship);

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

            animationService.Animatables.Add(ship);
            animationService.ComputePathPoints();
        }

        public void Process(Ship ship)
        {
            this.Ship = ship;

            if (!levelInitialized)
            {
                InitLevel(1);
            }
        }
    }
}
