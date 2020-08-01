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
using BlazorGalaga.Static.GameServiceHelpers;
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

        public void DoEnemyExplosions()
        {
            GetBugs().Where(a => a.IsExploding).ToList().ForEach(bug => {
                CreateExplosion(bug.Location);
                bug.DestroyImmediately = true;
            });

            animationService.Animatables.Where(a => a.Sprite.SpriteType == Sprite.SpriteTypes.EnemyExplosion1).ToList().ForEach(exp =>
            {
                if (exp.SpriteBankIndex < 4)
                    exp.SpriteBankIndex += 1;
                else
                    exp.DestroyImmediately = true;
            });
        }


        public void CreateExplosion(PointF location)
        {
            var exp = new EnemyExplosion()
            {
                DrawPath = false,
                RotateAlongPath = false,
                Started = true,
                DestroyAfterComplete = false,
                IsMoving = false,
                PathIsLine = true,
                Location = location
            };

            exp.SpriteBankIndex = 0;

            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion1));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion2));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion3));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion4));
            exp.SpriteBank.Add(new Sprite(Sprite.SpriteTypes.EnemyExplosion5));

            animationService.Animatables.Add(exp);
        }

        public void Init()
        {
            Lives = 2;
            ShipManager.InitShip(animationService);
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
                       //EnemyGridManager.EnemyGridBreathing = true;
                       // for (int i = 0; i <= 100; i++)
                       // {
                       //     Task.Delay(i * 3000).ContinueWith((task) =>
                       //     {
                       //        var bug = EnemyDiveManager.DoEnemyDive(GetBugs(),animationService,Ship);
                       //         if (bug != null)
                       //         {
                       //             Task.Delay(1000).ContinueWith((task) =>
                       //             {
                       //                 EnemyDiveManager.DoEnemyFire(bug, animationService, Ship);
                       //             });
                       //         }
                       //     });
                       // }
                    });
                    break;
            }

            levelInitialized = true;
        }

        private List<Bug> GetBugs()
        {
            return animationService.Animatables.Where(a =>
                a.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug
                || a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug
                || a.Sprite.SpriteType == Sprite.SpriteTypes.RedBug
            ).Select(a=> a as Bug).ToList();
        }

        public async void Process(Ship ship, float timestamp)
        {
            var bugs = GetBugs();

            //Begin Init - Only happens once
            if (!levelInitialized)
            {
                this.Ship = ship;
                InitLevel(1);
            }

            if (!consoledrawn && Ship.Sprite.BufferCanvas != null)
            {
                await ConsoleManager.DrawConsole(Lives, spriteService, ship);
                consoledrawn = true;
            }
            //End Init - Only happens once

            if (timestamp - EnemyGridManager.LastEnemyGridMoveTimeStamp > 40 || EnemyGridManager.LastEnemyGridMoveTimeStamp == 0)
            {
                DoEnemyExplosions();
            }

            ChildBugsManager.MoveChildBugs(bugs, animationService);

            if (timestamp - EnemyGridManager.LastEnemyGridMoveTimeStamp > 100 || EnemyGridManager.LastEnemyGridMoveTimeStamp == 0)
            {
                EnemyGridManager.MoveEnemyGrid(bugs);
                EnemyGridManager.LastEnemyGridMoveTimeStamp = timestamp;
            }

            if (timestamp - FlapWingsManager.LastWingFlapTimeStamp > 500 || FlapWingsManager.LastWingFlapTimeStamp == 0)
            {
                FlapWingsManager.FlapWings(bugs);
                FlapWingsManager.LastWingFlapTimeStamp = timestamp;
            }

            if (ship.IsFiring)
            {
                ship.IsFiring = false;
                ShipManager.Fire(ship, animationService);
            }

            ShipManager.CheckMissileCollisions(bugs, animationService);

        }
    }
}
