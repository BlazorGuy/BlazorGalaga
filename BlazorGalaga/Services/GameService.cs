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
        public bool Started { get; set; }

        private bool consoledrawn = false;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private int prevbugcount = 0;
        private bool capturehappened;
        private int hits = 0;
        private int wave = 1;
        private bool hideintroscreen;
        private bool introsounddone;
        private int divespeedincrease = 0;
        private int missileincrease = 0;
        private int introspeedincrease = 0;
        private int maxwaittimebetweendives = 5000;
        private bool skipintro = true;
        private bool soundoff = true;

        public void Init()
        {
            //Level = 1;
            Lives = 2;
            ShipManager.InitShip(animationService);
        }

        private void InitLevel(int level)
        {
            switch (level)
            {
                case 1:
                    Level1.InitIntro(animationService, introspeedincrease);
                    break;
                case 2:
                    Level2.InitIntro(animationService, introspeedincrease);
                    break;
                case 3: //challenge
                    Level3.InitIntro(animationService, -2);
                    break;
                case 4:
                    Level4.InitIntro(animationService, introspeedincrease);
                    maxwaittimebetweendives = 4000;
                    break;
                case 5:
                    Level5.InitIntro(animationService, introspeedincrease);
                    maxwaittimebetweendives = 3000;
                    divespeedincrease = 1;
                    missileincrease = 1;
                    break;
                case 6:
                    Level6.InitIntro(animationService, introspeedincrease);
                    maxwaittimebetweendives = 2500;
                    divespeedincrease = 1;
                    missileincrease = 1;
                    introspeedincrease = 1;
                    break;
                case 7:
                    Level7.InitIntro(animationService, introspeedincrease);
                    maxwaittimebetweendives = 2500;
                    divespeedincrease = 1;
                    missileincrease = 2;
                    introspeedincrease = 1;
                    break;
                case 8: //challenge
                    Level8.InitIntro(animationService, -2);
                    break;
                case 9:
                    Level9.InitIntro(animationService, introspeedincrease);
                    maxwaittimebetweendives = 2000;
                    divespeedincrease = 2;
                    missileincrease = 2;
                    introspeedincrease = 1;
                    break;
                case 10:
                    Level10.InitIntro(animationService, introspeedincrease);
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

            GetBugs().Where(a => a.Wave == 1).ToList().ForEach(a => a.Started = true);
        }

        private float LastDiveTimeStamp = 0;
        private int NextDiveWaitTime = 0;

        private void Dive()
        {
            if (GetBugs().Count == 0 || Ship.Disabled)
                return;

            var bug = EnemyDiveManager.DoEnemyDive(GetBugs(), animationService, Ship, Constants.BugDiveSpeed + divespeedincrease,null,false,capturehappened);

            if (bug != null && bug.CaptureState == Bug.enCaptureState.Started) capturehappened = true;

            foreach (var b in GetBugs())
            {
                if (b.IsDiving && b.CaptureState == Bug.enCaptureState.NotStarted && !b.IsExploding && b.MissileCountDowns.Count==0)
                {
                    var maxmissleperbug = Utils.Rnd(0 + missileincrease, 3 + missileincrease);
                    for (int i = 1; i <= maxmissleperbug; i++)
                    {
                        b.MissileCountDowns.Add(Utils.Rnd(4, 10));
                    }
                }
            }
           
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
                    wave = 1;
                    GalagaCaptureManager.Reset();
                    await ConsoleManager.ClearConsoleLevelText(spriteService);
                    await ConsoleManager.DrawConsoleLevelText(spriteService, Level);
                    SoundManager.StopAllSounds();
                    if (Level == 3 || Level == 8)
                        SoundManager.PlaySound(SoundManager.SoundManagerSounds.challengingstage);
                    else
                    {
                        SoundManager.PlaySound(SoundManager.SoundManagerSounds.levelup);
                    }
                }, WaitManager.WaitStep.enStep.ShowLevelText);
                if (WaitManager.WaitFor(2000, timestamp, WaitManager.WaitStep.enStep.Pause2))
                {
                    WaitManager.DoOnce(async () =>
                    {
                        await ConsoleManager.ClearConsoleLevelText(spriteService);
                        InitLevel(Level);
                        Ship.Visible = true;
                        EnemyGridManager.BreathSoundPlayed = false;
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
                await ConsoleManager.DrawConsole(Lives, spriteService, Ship, false);
                consoledrawn = true;
                SoundManager.OnEnd += SoundManager_OnEnd; ;
            }
            //End Init - Only happens once

            if (skipintro)
            {
                skipintro = false;
                hideintroscreen = true;
                introsounddone = true;
                Started = true;
                Ship.Visible = true;
            }

            if (soundoff && !SoundManager.SoundIsOff)
            {
                SoundManager.TurnSoundOff();
            }

            //show the intro screen if the space bar hasn't been pressed yet
            if (!hideintroscreen)
            {
                if (KeyBoardHelper.SpaceBarPressed)
                {
                    SoundManager.PlaySound(SoundManager.SoundManagerSounds.coin, true);
                    await ConsoleManager.ClearConsole(spriteService);
                    await ConsoleManager.DrawConsole(Lives, spriteService, Ship, true);
                    Started = true;
                }
                else
                {
                    await ConsoleManager.DrawIntroScreen(spriteService, Ship);
                    return;
                }
            }

            //if the intro sound isn't done, exit
            if (!introsounddone)
            {
                await ConsoleManager.DrawConsolePlayer1(spriteService);
                return;
            }

            var bugs = GetBugs();

            //dive the bugs
            if (timestamp - LastDiveTimeStamp > NextDiveWaitTime && EnemyGridManager.EnemyGridBreathing)
            {
                Dive();
                LastDiveTimeStamp = timestamp;
                NextDiveWaitTime = Utils.Rnd(500, maxwaittimebetweendives);
            }

            //if the bug intro wave is done, increment to the next wave]
            //or start diving and firing
            if ((bugs.Count(a=>a.Started && !a.IsMoving && a.Wave == wave) > 0 || bugs.Count(a=>a.Wave==wave) == 0) && wave <= 6 && bugs.Count() > 0)
            {
                wave += 1;
                if (wave == 6)
                {
                    EnemyGridManager.EnemyGridBreathing = true;
                    NextDiveWaitTime = Utils.Rnd(500, maxwaittimebetweendives);
                }
                else
                    GetBugs().Where(a => a.Wave == wave).ToList().ForEach(a => a.Started = true);
            }

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

                //are we at a challenging stage?
                if ((Level == 3 || Level == 8) && !WaitManager.Steps.Any(a=> a.Step == WaitManager.WaitStep.enStep.Pause1))
                {
                    SoundManager.PlaySound(SoundManager.SoundManagerSounds.challengingstageover, true);
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
               
               //fire missiles
               foreach(var bug in bugs.Where(a=>(a.MissileCountDowns.Count > 0 && a.Started) &&
               ((a.IsDiving && a.Location.Y <= Constants.CanvasSize.Height - 300 && a.IsMovingDown) || //for diving bugs
               (a.IsInIntro && a.Wave==wave && a.Location.Y > 100 && a.Location.X > 150 & a.Location.X < Constants.CanvasSize.Width-150 && a.Location.Y <= Constants.CanvasSize.Height - 400)))) //for intro bugs
                {
                    for (int i = 0; i <= bug.MissileCountDowns.Count - 1; i++)
                    {
                        bug.MissileCountDowns[i] -= 1;
                        if (bug.MissileCountDowns[i] <= 0)
                        {
                            EnemyDiveManager.DoEnemyFire(bug, animationService, Ship);
                            bug.MissileCountDowns.RemoveAll(a => a <= 0);
                        }
                    }
                }
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

            //draw fighter captured text if a fighter is captured
            if (bugs.Any(a => a.FighterCapturedMessageShowing))
                await ConsoleManager.DrawConsoleFighterCaptured(spriteService);


            //hide fighter captured text if a fighter is captured
            //and bug had flown back home
            if (bugs.Any(a => a.ClearFighterCapturedMessage))
            {
                await ConsoleManager.ClearConsoleLevelText(spriteService);
                bugs.FirstOrDefault(a => a.ClearFighterCapturedMessage).ClearFighterCapturedMessage = false;
            }

            //if morphed bugs go offscreen, destroy them immediately
            bugs.Where(a => a.IsMorphedBug && a.Location.Y >= Constants.CanvasSize.Height).ToList().ForEach(a => a.DestroyImmediately = true);

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

            //for debugging purposes
            if (glo.morphbug)
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
                var bug = bugs.FirstOrDefault(a => a.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug);
                bug.MorphState = Bug.enMorphState.Started;
                //EnemyDiveManager.DoEnemyDive(bugs, animationService, Ship, Constants.BugDiveSpeed, bug,false,false,true);
            }
        }

        private void SoundManager_OnEnd(Howler.Blazor.Components.Events.HowlEventArgs e)
        {
            var soundname = SoundManager.Sounds.FirstOrDefault(a => a.SoundId == e.SoundId).SoundName;

            if (soundname == SoundManager.SoundManagerSounds.coin)
            {
                hideintroscreen = true;
                SoundManager.PlaySound(SoundManager.SoundManagerSounds.introsong);
            }
            else if (soundname == SoundManager.SoundManagerSounds.introsong)
            {
                introsounddone = true;
            }
        }

    }
}
