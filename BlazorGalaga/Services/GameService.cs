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
using static BlazorGalaga.Pages.Index;

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
        private bool capturehappened;
        private int hits = 0;

        private int divespeedincrease = 0;
        private int missileincrease = 0;
        private int introspeedincrease = 0;
        private int maxwaittimebetweendives = 5000;

        public void Init()
        {
            //Level = 7;
            Lives = 2;
            ShipManager.InitShip(animationService);
        }

        private void InitLevel(int level)
        {
            List<int> delays = null;
            switch (level)
            {
                case 1:
                    Level1.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    break;
                case 2:
                    Level2.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    break;
                case 3: //challenge
                    Level3.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 10000, 17000, 22000, -1 };
                    break;
                case 4:
                    Level4.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    maxwaittimebetweendives = 4000;
                    break;
                case 5:
                    Level5.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    maxwaittimebetweendives = 3000;
                    divespeedincrease = 1;
                    missileincrease = 1;
                    break;
                case 6:
                    Level6.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    maxwaittimebetweendives = 2500;
                    divespeedincrease = 1;
                    missileincrease = 1;
                    introspeedincrease = 1;
                    break;
                case 7:
                    Level7.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    maxwaittimebetweendives = 2500;
                    divespeedincrease = 1;
                    missileincrease = 2;
                    introspeedincrease = 1;
                    break;
                case 8: //challenge
                    Level8.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 10000, 17000, 22000, -1 };
                    break;
                case 9:
                    Level9.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    maxwaittimebetweendives = 2000;
                    divespeedincrease = 2;
                    missileincrease = 2;
                    introspeedincrease = 1;
                    break;
                case 10:
                    Level10.InitIntro(animationService, introspeedincrease);
                    delays = new List<int>() { 2000, 5000, 9000, 14000, 18000, 22000 };
                    maxwaittimebetweendives = 1500;
                    divespeedincrease = 3;
                    missileincrease = 3;
                    introspeedincrease = 2;
                    break;
            }

           GetBugs().ForEach(a => {
                a.Paths.ForEach(p => {
                    if (a.Index == 0 || a.Index == 0) p.DrawPath = true;
                    if (a.Index == 0) p.OutPutDebug = true;
                    if (p.IsIntroPath)
                        a.PathPoints.AddRange(animationService.ComputePathPoints(p, false, 20));
                    else
                        a.PathPoints.AddRange(animationService.ComputePathPoints(p,false));
                });
            });

            Task.Delay(delays[0], cancellationTokenSource.Token).ContinueWith((task) =>
            {
                GetBugs().Where(a => a.Wave == 1).ToList().ForEach(a => a.Started = true);
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
            if (GetBugs().Count == 0 || Ship.Disabled)
            {
                Task.Delay(1000, cancellationTokenSource.Token).ContinueWith((task) =>
                {
                    DiveAndFire();
                });
                return;
            }

            Task.Delay(Utils.Rnd(500,maxwaittimebetweendives), cancellationTokenSource.Token).ContinueWith((task) =>
            {
                var bug = EnemyDiveManager.DoEnemyDive(GetBugs(), animationService, Ship, Constants.BugDiveSpeed + divespeedincrease,null,false,capturehappened);

                if (bug != null && bug.CaptureState == Bug.enCaptureState.Started) capturehappened = true;

                if (bug != null && bug.IsDiving && bug.CaptureState == Bug.enCaptureState.NotStarted)
                {
                    var maxmissleperbug = Utils.Rnd(0 + missileincrease, 3 + missileincrease);
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
                a as Bug !=null
            ).Select(a=> a as Bug).ToList();
        }

        private void MoveToNextLevel(float timestamp)
        {
            if (WaitManager.WaitFor(2000, timestamp, WaitManager.WaitStep.enStep.Pause1))
            {
                WaitManager.DoOnce(async () =>
                {
                    Level += 1;
                    capturehappened = false;
                    hits = 0;
                    GalagaCaptureManager.Reset();
                    EnemyGridManager.BreathSoundPlayed = false;
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

        public async void Process(float timestamp, GameLoopObject glo)
        {
            //Begin Init - Only happens once
            if (!consoledrawn && Ship.Sprite.BufferCanvas != null)
            {
                Ship.Visible = false;
                await ConsoleManager.DrawConsole(Lives, spriteService, Ship);
                consoledrawn = true;
            }
            //End Init - Only happens once

            var bugs = GetBugs();

            //adjust score when bugs are destroyed
            if (bugs.Count != prevbugcount)
            {
                await ConsoleManager.DrawScore(spriteService, Score);
                prevbugcount = bugs.Count();
            }

            //all bugs destroyed, increment to next level
            if (bugs.Count == 0)
            {
                WaitManager.DoOnce(() =>
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource = new CancellationTokenSource();
                    EnemyGridManager.EnemyGridBreathing = false;
                }, WaitManager.WaitStep.enStep.CleanUp);

                if ((Level == 3 || Level == 8) && !WaitManager.Steps.Any(a=> a.Step == WaitManager.WaitStep.enStep.Pause1))
                {
                    if (WaitManager.WaitFor(1500, timestamp, WaitManager.WaitStep.enStep.ShowNumberOfHitsLabel))
                    {
                        await ConsoleManager.DrawConsoleNumberOfHitsLabel(spriteService);
                        if (WaitManager.WaitFor(1500, timestamp, WaitManager.WaitStep.enStep.ShowNumberOfHits))
                        {
                            await ConsoleManager.DrawConsoleNumberOfHits(spriteService, hits);
                            if (WaitManager.WaitFor(1500, timestamp, WaitManager.WaitStep.enStep.ShowBonusLabel))
                            {
                                await ConsoleManager.DrawConsoleBonusLabel(spriteService);
                                if (WaitManager.WaitFor(1500, timestamp, WaitManager.WaitStep.enStep.ShowBonus))
                                {
                                    await ConsoleManager.DrawConsoleBonus(spriteService,hits * 1000);
                                    if (WaitManager.WaitFor(2000, timestamp, WaitManager.WaitStep.enStep.Pause3))
                                    {
                                        Score += hits * 1000;
                                        await ConsoleManager.ClearConsoleLevelText(spriteService);
                                        MoveToNextLevel(timestamp);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MoveToNextLevel(timestamp);
                }
            }

            //animate explosions
            if (timestamp - EnemyGridManager.LastEnemyGridMoveTimeStamp > 35)
            {
                EnemyExplosionManager.DoEnemyExplosions(bugs, animationService, this);
            }

            //animate child bugs
            ChildBugsManager.MoveChildBugs(bugs, animationService);

            //animated the moving enemy grid
            if (timestamp - EnemyGridManager.LastEnemyGridMoveTimeStamp > 100 || EnemyGridManager.LastEnemyGridMoveTimeStamp == 0)
            {
                EnemyGridManager.MoveEnemyGrid(bugs, animationService, Ship);
                EnemyGridManager.LastEnemyGridMoveTimeStamp = timestamp;
            }

            //animate the flapping wings
            if (timestamp - FlapWingsManager.LastWingFlapTimeStamp > 500 || FlapWingsManager.LastWingFlapTimeStamp == 0)
            {
                FlapWingsManager.FlapWings(bugs);
                FlapWingsManager.LastWingFlapTimeStamp = timestamp;
            }

            //animate ship missiles
            if (Ship.IsFiring && !Ship.Disabled)
            {
                Ship.IsFiring = false;
                ShipManager.Fire(Ship, animationService);
            }

            //center the ship if it's disabled
            //happens after a galaga capture
            if (Ship.Disabled)
            {

                if (Ship.Location.X > 320)
                    Ship.Speed = Constants.ShipMoveSpeed * -1;
                else if (Ship.Location.X < 310)
                    Ship.Speed = Constants.ShipMoveSpeed;
                else
                    Ship.Speed = 0;
            }

            //ship missile detection
            if (!Ship.Disabled)
                hits += (ShipManager.CheckMissileCollisions(bugs, animationService) ? 1 : 0);

            //for debugging purposes
            if (glo.captureship)
            {
                bugs.ForEach(a => {
                    a.Location = BugFactory.EnemyGrid.GetPointByRowCol(a.HomePoint.X, a.HomePoint.Y);
                    a.CurPathPointIndex = 0;
                    a.PathPoints.Clear();
                    a.Paths.Clear();
                    a.IsMoving = false;
                    a.StartDelay = 0;
                    a.Started = true;
                });
                var bug = bugs.FirstOrDefault(a => a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug);
                EnemyDiveManager.DoEnemyDive(bugs, animationService, Ship, Constants.BugDiveSpeed, bug, true);
            }
        }
    }
}
