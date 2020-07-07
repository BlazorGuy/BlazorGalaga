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
        public string DiagnosticInfo = "";

        private Canvas2DContext ctx;
        private bool stopGameLoop;

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

        private DateTime lastTime = DateTime.Now;
        private int updateInterval = 600;

        [JSInvokable]
        public async void GameLoop()
        {
            if (stopGameLoop) return;

            try
            {
                await JsRuntime.InvokeAsync<object>("logDiagnosticInfo", Utils.DiagnosticInfo);



                var currentTime = DateTime.Now;

                if ((currentTime - lastTime).TotalMilliseconds > 16)
                {
                    lastTime = currentTime; //we're too far behind, catch up
                }
                int updatesNeeded = (int)(currentTime - lastTime).TotalMilliseconds / updateInterval;
                Utils.dOut("updatesNeeded", updatesNeeded);
                for (int i = 0; i < updatesNeeded; i++)
                {
                    animationService.Animate();
                    lastTime.AddMilliseconds(updateInterval);
                }
                animationService.Draw();




                Utils.LogFPS();

                await animationService.ResetCanvas();

                KeyBoardHelper.ControlShip(ship);
            }
            catch (Exception ex)
            {
                stopGameLoop = true;
                Utils.dOut("Exception", ex.Message + "<br/>" + ex.StackTrace);
                await JsRuntime.InvokeAsync<object>("logDiagnosticInfo", Utils.DiagnosticInfo);
                throw ex;
            }
        }

    }
}
