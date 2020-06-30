using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Models;
using BlazorGalaga.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorGalaga.Pages
{
    public partial class Index: ComponentBase
    {
        private Canvas2DContext ctx;
        private List<Animation> Animations = new List<Animation>();

        protected BECanvasComponent _canvasReference;
        protected ElementReference spriteSheet;

        private int fps = 60;
        private string backgroundcolor = "#454545";
        private CansvasDimension dimensions = new CansvasDimension();

        [Inject]
        public BezierCurveService bezierCurveService { get; set; }
        [Inject]
        public AnimationService animationService { get; set; }
        [Inject]
        public BrowserService browserService { get; set; }
        [Inject]
        public SpriteService spriteService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            ctx = await _canvasReference.CreateCanvas2DAsync();

            spriteService.CanvasCtx = ctx;
            spriteService.SpriteSheet = spriteSheet;

            var shipAnimation = new Animation();
            shipAnimation.Animatables.Add(new Ship());


            Animations.Add(shipAnimation);

            await SetInterval(() => Animate(), TimeSpan.FromMilliseconds(1000 / fps));

        }

        public static async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);

            action();

            //await SetInterval(action, timeout);
        }

        async void Animate()
        {
            dimensions = await browserService.ResizeCanvas();
            // redraw path

            await ctx.ClearRectAsync(0, 0, (int)dimensions.Width, (int)dimensions.Height);
            await ctx.SetFillStyleAsync(backgroundcolor);
            await ctx.FillRectAsync(0, 0, (int)dimensions.Width, (int)dimensions.Height);
            await ctx.SetLineWidthAsync(5);

            foreach (Animation a in Animations)
            {
                animationService.Animate(a, false);
                animationService.Draw(a);
            }
        }
    }
}
