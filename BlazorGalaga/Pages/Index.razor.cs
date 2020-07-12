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
        public GameService gameService { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            ctx = await _canvasReference.CreateCanvas2DAsync();

            spriteService.CanvasCtx = ctx;
            spriteService.SpriteSheet = spriteSheet;
            spriteService.Init();

            gameService.animationService = animationService;
            gameService.Init();

            ship = (Ship)animationService.Animatables.FirstOrDefault(a => a.Sprite.SpriteType == Sprite.SpriteTypes.Ship);
            ship.CurPathPointIndex = (int)ship.PathPoints.Count / 2;

            await JsRuntime.InvokeAsync<object>("initFromBlazor", DotNetObjectReference.Create(this));

        }

        private int targetTicksPerFrame = (1000 / 60);
        private float delta;
        private float lastTimeStamp;

        public class GameLoopObject
        {
            public float timestamp { get; set; }
            public bool editcurveschecked { get; set; }
            public bool pauseanimation { get; set; }
            public bool addpath { get; set; }
            public bool resetanimation { get; set; }
        }

        [JSInvokable]
        public async void GameLoop(GameLoopObject glo)
        {
            if (stopGameLoop || glo.pauseanimation)
            {
                lastTimeStamp = glo.timestamp;
                return;
            }

            try
            {
                await JsRuntime.InvokeAsync<object>("logDiagnosticInfo", Utils.DiagnosticInfo);

                var timeStamp = glo.timestamp;

                gameService.Process();

                //Start Animation Logic
                delta += (int)(timeStamp - lastTimeStamp);
                lastTimeStamp = timeStamp;

                Utils.dOut("delta", delta);
                while (delta >= targetTicksPerFrame)
                {
                    animationService.Animate();
                    delta -= targetTicksPerFrame;
                }
                animationService.Draw();
                //End Animation Logic

                //Start Curve Editor Logic
                if (glo.editcurveschecked)
                    CurveEditorHelper.EditCurves(animationService,glo);
                else
                    CurveEditorHelper.DisableLines(animationService);
                if (glo.resetanimation) CurveEditorHelper.ResetAnimation(animationService);
                //End Curve Editor Logic

                Utils.LogFPS();

                KeyBoardHelper.ControlShip(ship);
            }
            catch (Exception ex)
            {
                stopGameLoop = true;
                Utils.dOut("<p style=\"color: red\">Exception", ex.Message + "</p><br/>" + ex.StackTrace);
                await JsRuntime.InvokeAsync<object>("logDiagnosticInfo", Utils.DiagnosticInfo);
                throw ex;
            }
        }

    }
}
