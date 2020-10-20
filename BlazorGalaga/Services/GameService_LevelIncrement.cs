using BlazorGalaga.Models;
using BlazorGalaga.Static;
using BlazorGalaga.Static.GameServiceHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Services
{
    public partial class GameService
    {
        public async Task DoLevelIncrementAsync(List<Bug> bugs, float timestamp)
        {
            if (bugs.Count == 0 && !Ship.Disabled)
            {
                WaitManager.DoOnce(() =>
                {
                    EnemyGridManager.EnemyGridBreathing = false;
                }, WaitManager.WaitStep.enStep.CleanUp);

                //are we at a challenging stage?
                if ((IsChallengeLevel()) && !WaitManager.Steps.Any(a => a.Step == WaitManager.WaitStep.enStep.Pause1))
                {
                    if (hits == 40)
                        SoundManager.PlaySound(SoundManager.SoundManagerSounds.challengingstageperfect, true);
                    else
                        SoundManager.PlaySound(SoundManager.SoundManagerSounds.challengingstageover, true);
                    if (WaitManager.WaitFor(1500, timestamp, WaitManager.WaitStep.enStep.ShowNumberOfHitsLabel))
                    {
                        await ConsoleManager.DrawConsoleNumberOfHitsLabel(spriteService, hits);
                        if (WaitManager.WaitFor(1500, timestamp, WaitManager.WaitStep.enStep.ShowNumberOfHits))
                        {
                            await ConsoleManager.DrawConsoleNumberOfHits(spriteService, hits);
                            if (WaitManager.WaitFor(1500, timestamp, WaitManager.WaitStep.enStep.ShowBonusLabel))
                            {
                                await ConsoleManager.DrawConsoleBonusLabel(spriteService, hits);
                                if (WaitManager.WaitFor(1500, timestamp, WaitManager.WaitStep.enStep.ShowBonus))
                                {
                                    await ConsoleManager.DrawConsoleBonus(spriteService, hits);
                                    if (WaitManager.WaitFor(2000, timestamp, WaitManager.WaitStep.enStep.Pause3))
                                    {
                                        Score += hits == 40 ? 10000 : hits * 100;
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
        }

        public void MoveToNextLevel(float timestamp)
        {
            if (WaitManager.WaitFor(2000, timestamp, WaitManager.WaitStep.enStep.Pause1))
            {
                WaitManager.DoOnce(async () =>
                {
                    Level += 1;
                    if (Level == 12)
                    {
                        LevelOffset += 8;
                        Level = 4;
                    }
                    capturehappened = false;
                    hits = 0;
                    wave = 1;
                    GalagaCaptureManager.Reset();
                    await ConsoleManager.DrawConsole(Lives, spriteService, Ship, true, Level - 1 + LevelOffset,Score, HighScore);
                    await ConsoleManager.ClearConsoleLevelText(spriteService);
                    await ConsoleManager.DrawConsoleLevelText(spriteService, Level + LevelOffset, IsChallengeLevel());
                    SoundManager.StopAllSounds();
                    if (IsChallengeLevel())
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
    }
}
