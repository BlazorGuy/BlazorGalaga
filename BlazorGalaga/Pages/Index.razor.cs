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
        private Canvas2DContext bufferctx;
        private bool stopGameLoop;

        protected BECanvasComponent _canvasReference;
        protected BECanvasComponent _buffercanvasReference;
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
            bufferctx = await _canvasReference.CreateCanvas2DAsync();

            spriteService.CanvasCtx = ctx;
            spriteService.SpriteSheet = spriteSheet;

            animationService.CanvasCtx = ctx;

            animationService.InitAnimations();
            animationService.ComputePathPoints();

            ship = (Ship)animationService.Animatables.FirstOrDefault(a => a.Sprite.SpriteType == Sprite.SpriteTypes.Ship);

            await JsRuntime.InvokeAsync<object>("initFromBlazor", DotNetObjectReference.Create(this));

        }

        private int targetTicksPerFrame = (1000 / 60);
        private float delta;
        private float lastTimeStamp;

        [JSInvokable]
        public async void GameLoop(float timeStamp)
        {
            if (stopGameLoop) return;

            try
            {
                await JsRuntime.InvokeAsync<object>("logDiagnosticInfo", Utils.DiagnosticInfo);

                //Start Animation Logic
                delta += (int)(timeStamp - lastTimeStamp);
                lastTimeStamp = timeStamp;

                Utils.dOut("delta", delta);
                while(delta >= targetTicksPerFrame)
                {
                    await animationService.ResetCanvas();
                    animationService.Animate();
                    animationService.Draw();
                    delta -= targetTicksPerFrame;
                }
                //End Animation Logic

                Utils.LogFPS();

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
