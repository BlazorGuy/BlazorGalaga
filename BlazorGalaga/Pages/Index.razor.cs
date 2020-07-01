using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Models;
using BlazorGalaga.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGalaga.Pages
{
    public partial class Index: ComponentBase
    {
        private Canvas2DContext ctx;

        protected BECanvasComponent _canvasReference;
        protected ElementReference spriteSheet;

        private int fps = 60;
        private CanvasDimension dimensions = new CanvasDimension();

        [Inject]
        public BezierCurveService bezierCurveService { get; set; }
        [Inject]
        public AnimationService animationService { get; set; }
        [Inject]
        public BrowserService browserService { get; set; }
        [Inject]
        public SpriteService spriteService { get; set; }

        [JSInvokable("OnKeyDown")]
        public static void OnKeyDown(string keycode)
        {
            Console.WriteLine(keycode);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            ctx = await _canvasReference.CreateCanvas2DAsync();

            spriteService.CanvasCtx = ctx;
            spriteService.SpriteSheet = spriteSheet;

            animationService.CanvasCtx = ctx;

            dimensions = await browserService.ResizeCanvas();


            animationService.InitSprites(dimensions);

           

            await SetInterval(() => Animate(), TimeSpan.FromMilliseconds(1000 / fps));

        }

        public static async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);

            action();

            await SetInterval(action, timeout);
        }

        async void Animate()
        {
            dimensions = await browserService.ResizeCanvas();

            await animationService.ResetCanvas(dimensions);

            foreach (Animation a in animationService.Animations)
            {
                animationService.Animate(a, true);
                animationService.Draw(a);
            }
        }
    }
}
