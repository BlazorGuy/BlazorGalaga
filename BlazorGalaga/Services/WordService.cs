using System;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorAnimate.Models;

namespace BlazorAnimate.Services
{
    public class WordService
    {
        public async void DrawText(Canvas2DContext ctx, Word word)
        {
            await ctx.SetFillStyleAsync("#ffffff");
            await ctx.SetFontAsync("70px Arial");
            await ctx.FillTextAsync(word.TextString, word.Location.X, word.Location.Y);
        }
    }
}
