using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Models;
using BlazorGalaga.Services;
using BlazorGalaga.Static;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGalaga.Pages
{
    public partial class Index: ComponentBase
    {
        private Canvas2DContext ctx;
        private static bool stopAnimating = false;

        protected BECanvasComponent _canvasReference;
        protected ElementReference spriteSheet;

        private static Animation shipAnimation;

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

            animationService.CanvasCtx = ctx;

            animationService.InitAnimations();

            shipAnimation = animationService.Animations.FirstOrDefault(
                a=>a.Animatables.Any(
                        b=>b.Sprite.SpriteType == Sprite.SpriteTypes.Ship
                    )
                );

            await browserService.ResizeCanvas();

            await SetInterval(() => Animate(), TimeSpan.FromMilliseconds(1000 / Constants.FPS));

        }

        public static async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);

            action();

            if(!stopAnimating)
                await SetInterval(action, timeout);
        }

        async void Animate()
        {
            try
            {
                await animationService.ResetCanvas();

                KeyBoardHelper.ControlShip(shipAnimation);

                foreach (Animation a in animationService.Animations)
                {
                    animationService.Animate(a);
                    animationService.Draw(a);
                }
            }
            catch(Exception ex)
            {
                stopAnimating = true;
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }
    }
}
