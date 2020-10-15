using BlazorGalaga.Models;
using BlazorGalaga.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Static.GameServiceHelpers
{
    public static class ConsoleManager
    {
        public static async Task DrawConsole(int lives, SpriteService spriteService, Ship ship, bool drawships, int level)
        {
            //draw top black block
            await spriteService.StaticCtx.SetFillStyleAsync("Black");
            await spriteService.StaticCtx.FillRectAsync(0, 0, Constants.CanvasSize.Width, 65);

            //draw bottom black block
            await spriteService.StaticCtx.FillRectAsync(0, Constants.CanvasSize.Height - 65, Constants.CanvasSize.Width, 65);

            if (drawships)
            {
                //draw the ships
                int left = 5;
                for (var i = 1; i <= lives; i++)
                {
                    await spriteService.StaticCtx.DrawImageAsync(
                        ship.Sprite.BufferCanvas.Canvas,
                        left,
                        Constants.CanvasSize.Height - ship.Sprite.SpriteDestRect.Height - 3
                    );
                    left += (int)ship.Sprite.SpriteDestRect.Width + 3;
                }

                level += 1;

                int badgeleftoffset = 0;

                //draw the 10 badges
                if (level >= 10)
                {
                    for (int i = 1; i <= (int)(level / 10); i++)
                    {
                        await spriteService.StaticCtx.DrawImageAsync(
                            spriteService.SpriteSheet,
                            329,
                            175,
                            10,
                            16,
                            Constants.CanvasSize.Width - 30 - badgeleftoffset,
                            Constants.CanvasSize.Height - 45,
                            28,
                            45
                        );
                        badgeleftoffset += 30;
                    }
                }
                
                if (level >= 10) level = level - ((int)(level / 10) * 10);

                //draw the 5 badges
                if (level >= 5)
                {
                    for (int i = 1; i <= (int)(level / 5); i++)
                    {
                        await spriteService.StaticCtx.DrawImageAsync(
                            spriteService.SpriteSheet,
                            315,
                            175,
                            10,
                            16,
                            Constants.CanvasSize.Width - 30 - badgeleftoffset,
                            Constants.CanvasSize.Height - 45,
                            28,
                            45
                        );
                        badgeleftoffset += 30;
                    }
                }

                if (level >= 5) level = level - ((int)(level / 5) * 5);

                //draw 1 the badges
                if (level <= 4)
                {
                    for (int i = 1; i <= level; i++)
                    {
                        await spriteService.StaticCtx.DrawImageAsync(
                            spriteService.SpriteSheet,
                            305,
                            175,
                            10,
                            16,
                            Constants.CanvasSize.Width - 30 - badgeleftoffset,
                            Constants.CanvasSize.Height - 45,
                            28,
                            45
                        );
                        badgeleftoffset += 30;
                    }
                }
            }

            await spriteService.StaticCtx.SetFillStyleAsync("Red");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");

            await spriteService.StaticCtx.FillTextAsync("1UP", 50, 30);
            await spriteService.StaticCtx.FillTextAsync("HIGH SCORE", 220, 30);

            await spriteService.StaticCtx.SetFillStyleAsync("White");
            await spriteService.StaticCtx.FillTextAsync("00", 50, 60);
            await spriteService.StaticCtx.FillTextAsync("20000", 300, 60);
        }

        public static async Task DrawIntroScreen(SpriteService spriteService, Ship ship)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("24px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("PUSH START BUTTON", 140, Constants.CanvasSize.Height / 3);

            await spriteService.StaticCtx.SetFillStyleAsync("Yellow");
            await spriteService.StaticCtx.FillTextAsync("1ST BONUS FOR 30000 PTS", 90, Constants.CanvasSize.Height / 2);
            await spriteService.StaticCtx.FillTextAsync("2ND BONUS FOR 120000 PTS", 90, Constants.CanvasSize.Height / 2 + 75);
            await spriteService.StaticCtx.FillTextAsync("AND FOR EVERY 120000 PTS", 90, Constants.CanvasSize.Height / 2 + 150);

            await spriteService.StaticCtx.SetFillStyleAsync("White");
            await spriteService.StaticCtx.FillTextAsync("©1981  1995  NAMCO LTD.", 70, 750);
            await spriteService.StaticCtx.FillTextAsync("ALL RIGHTS RESERVED", 110, 780);

            await spriteService.StaticCtx.SetFillStyleAsync("Red");
            await spriteService.StaticCtx.SetFontAsync("38px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("namco", 250, 830);
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("®", 440, 827);

            spriteService.SetSpriteInfoBySpriteType(ship.Sprite);

            await spriteService.StaticCtx.DrawImageAsync(
                ship.Sprite.BufferCanvas.Canvas,
                25,
                430
            );

            await spriteService.StaticCtx.DrawImageAsync(
                ship.Sprite.BufferCanvas.Canvas,
                25,
                500
            );

            await spriteService.StaticCtx.DrawImageAsync(
                ship.Sprite.BufferCanvas.Canvas,
                25,
                580
            );

            await spriteService.StaticCtx.SetFillStyleAsync("White");
            await spriteService.StaticCtx.SetFontAsync("24px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("CREDITS 0", 20, 940);
        }


        public static async Task DrawScore(SpriteService spriteService, int score)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("White");
            await spriteService.StaticCtx.ClearRectAsync(40, 30, 150, 50);
            await spriteService.StaticCtx.FillTextAsync(score == 0 ? "00" : score.ToString(), 50, 60);
        }

        public static async Task ClearConsole(SpriteService spriteService)
        {
            await spriteService.StaticCtx.ClearRectAsync(0,0, Constants.CanvasSize.Width, Constants.CanvasSize.Height);
        }

        public static async Task ClearConsoleLevelText(SpriteService spriteService)
        {
            await spriteService.StaticCtx.ClearRectAsync(0, (Constants.CanvasSize.Height / 2)-100, Constants.CanvasSize.Width, 300);
        }

        public static async Task DrawConsoleLevelText(SpriteService spriteService, int level)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");

            if (level==3 || level==8)
                await spriteService.StaticCtx.FillTextAsync("CHALLENGING STAGE ", 120, Constants.CanvasSize.Height / 2);
            else
                await spriteService.StaticCtx.FillTextAsync("STAGE " + level, 250, Constants.CanvasSize.Height / 2);
        }

        public static async Task DrawConsoleNumberOfHitsLabel(SpriteService spriteService)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("NUMBER OF HITS", 100, Constants.CanvasSize.Height / 2);
        }

        public static async Task DrawConsoleNumberOfHits(SpriteService spriteService, int numberofhits)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync(numberofhits.ToString(), 490, Constants.CanvasSize.Height / 2);
        }

        public static async Task DrawConsoleBonusLabel(SpriteService spriteService)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("BONUS", 150, (Constants.CanvasSize.Height / 2) + 100);
        }


        public static async Task DrawConsoleBonus(SpriteService spriteService, int bonus)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync(bonus.ToString(), 400, (Constants.CanvasSize.Height / 2) + 100);
        }

        public static async Task DrawConsoleFighterCaptured(SpriteService spriteService)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("Red");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("FIGHTER CAPTURED", 125, (Constants.CanvasSize.Height / 2) + 100);
        }
        public static async Task DrawConsolePlayer1(SpriteService spriteService)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("PLAYER 1", 250, Constants.CanvasSize.Height / 2);
        }

        public static async Task DrawConsoleReady(SpriteService spriteService)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("READY", 250, Constants.CanvasSize.Height / 2);
        }

        public static async Task DrawConsoleGameOver(SpriteService spriteService)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("GAME OVER", 210, Constants.CanvasSize.Height / 2);
        }
    }
}
