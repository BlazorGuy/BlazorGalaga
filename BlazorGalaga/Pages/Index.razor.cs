using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using BlazorGalaga.Interfaces;
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
        private long framesRendered = 0;
        private Stopwatch timer = new Stopwatch();

        protected BECanvasComponent _canvasReference;
        protected ElementReference spriteSheet;

        private static Ship ship;

        [Inject]
        public BezierCurveService bezierCurveService { get; set; }
        [Inject]
        public AnimationService animationService { get; set; }
        [Inject]
        public SpriteService spriteService { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            ctx = await _canvasReference.CreateCanvas2DAsync();

            spriteService.CanvasCtx = ctx;
            spriteService.SpriteSheet = spriteSheet;

            animationService.CanvasCtx = ctx;

            animationService.InitAnimations();
            animationService.ComputePathPoints();

            ship = (Ship)animationService.Animatables.FirstOrDefault(a => a.Sprite.SpriteType == Sprite.SpriteTypes.Ship);

            await JsRuntime.InvokeAsync<object>("initFromBlazor", DotNetObjectReference.Create(this));

        }

        [JSInvokable]
        public async ValueTask GameLoop()
        {
            framesRendered += 1;
            if (!timer.IsRunning) timer.Start();

            if (timer.ElapsedMilliseconds>=1000)
            {
                Console.WriteLine("FPS: " + framesRendered);
                framesRendered = 0;
                timer.Restart();
            }

            //await animationService.ResetCanvas();

            //KeyBoardHelper.ControlShip(ship);

            //foreach (IAnimatable a in animationService.Animatables)
            //{
            //    animationService.Animate(a);
            //    animationService.Draw(a);
            //}
        }

    }
}
