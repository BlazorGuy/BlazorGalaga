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

            //await spriteService.StaticCtx.SetStrokeStyleAsync("red");
            //await spriteService.StaticCtx.SetLineWidthAsync(1);
            await spriteService.StaticCtx.SetFillStyleAsync("Red");
            await spriteService.StaticCtx.SetFontAsync("32px Sarif");

            //await spriteService.StaticCtx.StrokeTextAsync("This Is A Test", 50, 25);
            await spriteService.StaticCtx.FillTextAsync("1UP", 50, 25);
            await spriteService.StaticCtx.FillTextAsync("HIGH SCORE", 250, 25);

            await spriteService.StaticCtx.SetFillStyleAsync("White");
            await spriteService.StaticCtx.FillTextAsync("00", 50, 50);
            await spriteService.StaticCtx.FillTextAsync("20000", 300, 50);
        }
    }
}
