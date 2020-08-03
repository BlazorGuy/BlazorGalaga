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
using BlazorGalaga.Static.GameServiceHelpers.Levels;
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
                    Level2.Init(animationService);
                    break;
            }

            //Task.Delay(22000).ContinueWith((task) =>
            //{
            //    EnemyGridManager.EnemyGridBreathing = true;
            //    DiveAndFire();
            //});

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
                this.Ship = ship;
                InitLevel(1);
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
