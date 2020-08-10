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
using BlazorGalaga.Static.Levels;
using BlazorGalaganimatable.Models.Paths;

namespace BlazorGalaga.Services
{
    public class GameService
    {
        public AnimationService animationService { get; set; }
        public SpriteService spriteService { get; set; }
        public Ship Ship { get; set; }
        public int Lives { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }

        private bool consoledrawn = false;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private int prevbugcount = 0;

        public void Init()
        {
            //Level = 5;
            Lives = 2;
            ShipManager.InitShip(animationService);
        }

        private void CachPaths()
        {
            //TODO init paths here

            //animatables.ForEach(a => {
            //    a.Paths.ForEach(p => { a.PathPoints.AddRange(animationService.ComputePathPoints(p)); });
            //});
        }

        private void InitLevel(int level)
        {
            List<int> delays = null;
            switch (level)
            {
                case 1:
                    Level1.InitIntro(animationService);
                    delays = new List<int>() { 2000,5000,9000,14000,18000,22000 };
                    break;
                case 2:
                    Level2.InitIntro(animationService);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    break;
                case 3:
                    Level3.InitIntro(animationService);
                    delays = new List<int>() { 2000, 5000, 10000, 17000, 22000, -1 };
                    break;
                case 4:
                    Level4.InitIntro(animationService);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    break;
                case 5:
                    Level5.InitIntro(animationService);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    break;
                case 6:
                    Level6.InitIntro(animationService);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    break;
                case 7:
                    Level7.InitIntro(animationService);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    break;
            }

            animationService.Animatables.ForEach(a => {
                a.Paths.ForEach(p => {
                    //if (a.Index == 32 || a.Index == 24) p.DrawPath=true;
                    //if (a.Index == 32) p.OutPutDebug = true;
                    a.PathPoints.AddRange(animationService.ComputePathPoints(p));
                });
            });

            Task.Delay(delays[0], cancellationTokenSource.Token).ContinueWith((task) =>
            {
                GetBugs().Where(a => a.Wave==1).ToList().ForEach(a => a.Started = true);
            });
            Task.Delay(delays[1], cancellationTokenSource.Token).ContinueWith((task) =>
            {
                GetBugs().Where(a => a.Wave == 2).ToList().ForEach(a => a.Started = true);
            });
            Task.Delay(delays[2], cancellationTokenSource.Token).ContinueWith((task) =>
            {
                GetBugs().Where(a => a.Wave == 3).ToList().ForEach(a => a.Started = true);
            });
            Task.Delay(delays[3], cancellationTokenSource.Token).ContinueWith((task) =>
            {
                GetBugs().Where(a => a.Wave == 4).ToList().ForEach(a => a.Started = true);
            });
            Task.Delay(delays[4], cancellationTokenSource.Token).ContinueWith((task) =>
            {
                GetBugs().Where(a => a.Wave == 5).ToList().ForEach(a => a.Started = true);
            });
            if (delays[5] != -1)
            {
                Task.Delay(delays[5], cancellationTokenSource.Token).ContinueWith((task) =>
                {
                    EnemyGridManager.EnemyGridBreathing = true;
                    DiveAndFire();
                });
            }
        }
        private void DiveAndFire()
        {
            if (GetBugs().Count == 0)
                return;

            Task.Delay(Utils.Rnd(500,5000), cancellationTokenSource.Token).ContinueWith((task) =>
            {
                var bug = EnemyDiveManager.DoEnemyDive(GetBugs(), animationService, Ship);
                if (bug != null && bug.IsDiving)
                {
                    var maxmissleperbug = Utils.Rnd(0, 3);
                    for (int i = 1; i <= maxmissleperbug; i++)
                    {
                        Task.Delay(Utils.Rnd(200, 1000), cancellationTokenSource.Token).ContinueWith((task) =>
                        {
                            EnemyDiveManager.DoEnemyFire(bug, animationService, Ship);
                        });
                    }
                }
                DiveAndFire();
            });
        }

        private List<Bug> GetBugs()
        {
            return animationService.Animatables.Where(a =>
                a.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug
                || a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug
                || a.Sprite.SpriteType == Sprite.SpriteTypes.RedBug
                || a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug_Blue
            ).Select(a=> a as Bug).ToList();
        }

        public async void Process(float timestamp)
        {
            //Begin Init - Only happens once
            if (!consoledrawn && Ship.Sprite.BufferCanvas != null)
            {
                Ship.Visible = false;
                await ConsoleManager.DrawConsole(Lives, spriteService, Ship);
                CachPaths();
                consoledrawn = true;
            }
            //End Init - Only happens once

            var bugs = GetBugs();

            if (bugs.Count != prevbugcount)
            {
                await ConsoleManager.DrawScore(spriteService, Score);
                prevbugcount = bugs.Count();
            }

            if (bugs.Count == 0)
            {
                WaitManager.DoOnce(() =>
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource = new CancellationTokenSource();
                    EnemyGridManager.EnemyGridBreathing = false;
                }, WaitManager.WaitStep.enStep.CleanUp);

                if (WaitManager.WaitFor(2000, timestamp, WaitManager.WaitStep.enStep.Pause1))
                {
                    WaitManager.DoOnce(async ()=>
                    {
                        Level += 1;
                        await ConsoleManager.DrawConsoleLevelText(spriteService, Level); 
                    }, WaitManager.WaitStep.enStep.ShowLevelText);
                    if (WaitManager.WaitFor(2000, timestamp, WaitManager.WaitStep.enStep.Pause2))
                    {
                        WaitManager.DoOnce(async () =>
                        {
                            await ConsoleManager.ClearConsoleLevelText(spriteService);
                            InitLevel(Level);
                            Ship.Visible = true;
                            WaitManager.ClearSteps();
                        }, WaitManager.WaitStep.enStep.ClearLevelText);
                    }
                }
            }

            if (timestamp - EnemyGridManager.LastEnemyGridMoveTimeStamp > 35)
            {
                EnemyExplosionManager.DoEnemyExplosions(bugs,animationService,this);
            }

            ChildBugsManager.MoveChildBugs(bugs, animationService);

            if (timestamp - EnemyGridManager.LastEnemyGridMoveTimeStamp > 100 || EnemyGridManager.LastEnemyGridMoveTimeStamp == 0)
            {
                EnemyGridManager.MoveEnemyGrid(bugs,Ship);
                EnemyGridManager.LastEnemyGridMoveTimeStamp = timestamp;
            }

            if (timestamp - FlapWingsManager.LastWingFlapTimeStamp > 500 || FlapWingsManager.LastWingFlapTimeStamp == 0)
            {
                FlapWingsManager.FlapWings(bugs);
                FlapWingsManager.LastWingFlapTimeStamp = timestamp;
            }

            if (Ship.IsFiring)
            {
                Ship.IsFiring = false;
                ShipManager.Fire(Ship, animationService);
            }

            ShipManager.CheckMissileCollisions(bugs, animationService);

        }
    }
}
