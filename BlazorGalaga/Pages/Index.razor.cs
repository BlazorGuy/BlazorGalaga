using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private static string keydown;
        private static bool ignorenextkeyup;
        private static Animation shipAnimation;

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
            Console.WriteLine("KeyDown: " + keycode);

            if ((keydown == "ArrowLeft" && keycode == "ArrowRight") ||
                (keydown == "ArrowRight" && keycode == "ArrowLeft")) ignorenextkeyup = true;

            keydown = keycode;
        }

        [JSInvokable("OnKeyUp")]
        public static void OnKeyUp(string keycode)
        {
            Console.WriteLine("KeyUp: " + keycode);
            if (ignorenextkeyup)
                ignorenextkeyup = false;
            else
                keydown = "";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            ctx = await _canvasReference.CreateCanvas2DAsync();

            spriteService.CanvasCtx = ctx;
            spriteService.SpriteSheet = spriteSheet;

            animationService.CanvasCtx = ctx;

            animationService.InitAnimations();

            shipAnimation = animationService.Animations.FirstOrDefault(
                a=>a.Animatables.Any(
                        b=>b.Sprite.SpriteType== Sprite.SpriteTypes.Ship
                    )
                );

            await browserService.ResizeCanvas();

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
            await animationService.ResetCanvas();

            if (keydown == "ArrowLeft")
            {
                shipAnimation.Speed = -2;
            }
            if (keydown == "ArrowRight")
            {
                shipAnimation.Speed = 2;
            }
            if (keydown == "")
            {
                shipAnimation.Speed = 0;
            }

            foreach (Animation a in animationService.Animations)
            {
                animationService.Animate(a);
                animationService.Draw(a);
            }
        }
    }
}
