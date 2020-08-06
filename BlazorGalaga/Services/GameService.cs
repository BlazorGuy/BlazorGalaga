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
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(BugFactory.BugIntro.TwoGroupsOfFourFromTop));
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(BugFactory.BugIntro.TwoGroupsOfEightFromBottom));
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(BugFactory.BugIntro.TwoGroupsOfEightFromTop));
                    break;
                case 2:
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(BugFactory.BugIntro.TwoGroupsOfFourFromTop));
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(BugFactory.BugIntro.TwoGroupsOfStackedEightFromBottom));
                    animationService.Animatables.AddRange(BugFactory.CreateAnimation_BugIntro(BugFactory.BugIntro.TwoGroupsOfStackedEightFromTop));
                    break;
            }

            animationService.ComputePathPoints();

            //move in 2 sets of 4 (red and blue) from the top at the same time
            Task.Delay(2000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index < 8).ToList().ForEach(a => a.Started = true);
            });
            //move red and green bugs from the bottom left
            Task.Delay(5000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index >= 8 && a.Index < 16).ToList().ForEach(a => a.Started = true);
            });
            //move red bugs from the bottom right
            Task.Delay(9000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index >= 16 && a.Index < 24).ToList().ForEach(a => a.Started = true);
            });
            //move blue bugs from the top left
            Task.Delay(14000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index >= 24 && a.Index < 32).ToList().ForEach(a => a.Started = true);
            });
            //move blue bugs from the top right
            Task.Delay(18000).ContinueWith((task) =>
            {
                animationService.Animatables.Where(a => a.Index >= 32 && a.Index < 40).ToList().ForEach(a => a.Started = true);
            });

            Task.Delay(22000).ContinueWith((task) =>
            {
                EnemyGridManager.EnemyGridBreathing = true;
                DiveAndFire();
            });

            levelInitialized = true;
        }
        private void DiveAndFire()
        {
            if (GetBugs().Count == 0) return;

            Task.Delay(Utils.Rnd(500,5000)).ContinueWith((task) =>
            {
                var bug = EnemyDiveManager.DoEnemyDive(GetBugs(), animationService, Ship);
                if (bug != null && bug.IsDiving)
                {
                    var maxmissleperbug = Utils.Rnd(0, 3);
                    for (int i = 1; i <= maxmissleperbug; i++)
                    {
                        Task.Delay(Utils.Rnd(200, 1000)).ContinueWith((task) =>
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

        public async void Process(Ship ship, float timestamp)
        {
            var bugs = GetBugs();

            //Begin Init - Only happens once
            if (!levelInitialized)
            {
                await ConsoleManager.DrawConsoleLevelText(spriteService, 2);
                await ConsoleManager.ClearConsoleLevelText(spriteService);
                this.Ship = ship;
                InitLevel(2);
            }

            if (!consoledrawn && Ship.Sprite.BufferCanvas != null)
            {
                await ConsoleManager.DrawConsole(Lives, spriteService, ship);
                consoledrawn = true;
            }
            //End Init - Only happens once

            if (timestamp - EnemyGridManager.LastEnemyGridMoveTimeStamp > 35)
            {
                EnemyExplosionManager.DoEnemyExplosions(bugs,animationService);
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
