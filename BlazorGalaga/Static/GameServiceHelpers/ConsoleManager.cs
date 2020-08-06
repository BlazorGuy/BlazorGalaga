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
        public static async Task DrawConsole(int lives, SpriteService spriteService, Ship ship)
        {
            //draw top black block
            await spriteService.StaticCtx.SetFillStyleAsync("Black");
            await spriteService.StaticCtx.FillRectAsync(0, 0, Constants.CanvasSize.Width, 65);

            //draw bottom black block
            await spriteService.StaticCtx.FillRectAsync(0, Constants.CanvasSize.Height - 65, Constants.CanvasSize.Width, 65);

            //draw the ships
            int left = 5;
            for (var i = 1; i <= lives; i++)
            {
                await spriteService.StaticCtx.DrawImageAsync(
                    ship.Sprite.BufferCanvas.Canvas,
                    left,
                    Constants.CanvasSize.Height - ship.Sprite.SpriteDestRect.Height - 5
                );
                left += (int)ship.Sprite.SpriteDestRect.Width + 10;
            }

            //draw the badges
            await spriteService.StaticCtx.DrawImageAsync(
                spriteService.SpriteSheet,
                305,
                175,
                10,
                16,
                Constants.CanvasSize.Width - 30,
                Constants.CanvasSize.Height - 45,
                28,
                45
            );

            //await spriteService.StaticCtx.SetFillStyleAsync("Red");
            //await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");

            //await spriteService.StaticCtx.FillTextAsync("1UP", 50, 30);
            //await spriteService.StaticCtx.FillTextAsync("HIGH SCORE", 220, 30);

            //await spriteService.StaticCtx.SetFillStyleAsync("White");
            //await spriteService.StaticCtx.FillTextAsync("00", 50, 60);
            //await spriteService.StaticCtx.FillTextAsync("20000", 300, 60);
        }
        public static async Task ClearConsoleLevelText(SpriteService spriteService)
        {
            await spriteService.StaticCtx.ClearRectAsync(250, Constants.CanvasSize.Height / 2, 200, 50);
        }
        public static async Task DrawConsoleLevelText(SpriteService spriteService, int level)
        {
            await spriteService.StaticCtx.SetFillStyleAsync("rgba(152, 249, 255, 1)");
            await spriteService.StaticCtx.SetFontAsync("26px PressStart2P");
            await spriteService.StaticCtx.FillTextAsync("STAGE " + level, 250, Constants.CanvasSize.Height / 2);
        }
    }
}
