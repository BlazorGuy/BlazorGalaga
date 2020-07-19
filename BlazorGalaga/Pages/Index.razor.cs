using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        public List<Canvas> BufferCanvases { get; set; }

        private Canvas2DContext DynamicCtx;
        private Canvas2DContext StaticCtx;
        private bool stopGameLoop;

        protected BECanvasComponent StaticCanvas;
        protected BECanvasComponent DynamicCanvas;
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

        protected override void OnInitialized()
        {
            BufferCanvases = new List<Canvas>();

            for (int i = 1; i <= Constants.SpriteBufferCount; i++)
                BufferCanvases.Add(new Canvas() { CanvasRef = new BECanvasComponent() });
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            DynamicCtx = await DynamicCanvas.CreateCanvas2DAsync();
            StaticCtx = await StaticCanvas.CreateCanvas2DAsync();

            foreach (var canvas in BufferCanvases)
               canvas.Context = await canvas.CanvasRef.CreateCanvas2DAsync();

            await JsRuntime.InvokeAsync<object>("initFromBlazor", DotNetObjectReference.Create(this));
        }

        [JSInvokable("SpriteSheetLoaded")]
        public void SpriteSheetLoaded()
        {

            spriteService.DynamicCtx = DynamicCtx;
            spriteService.StaticCtx = StaticCtx;
            spriteService.BufferCanvases = BufferCanvases;

            spriteService.SpriteSheet = spriteSheet;
            spriteService.Init();

            gameService.animationService = animationService;
            gameService.spriteService = spriteService;
            gameService.Init();

            ship = (Ship)animationService.Animatables.FirstOrDefault(a => a.Sprite.SpriteType == Sprite.SpriteTypes.Ship);
            ship.CurPathPointIndex = (int)ship.PathPoints.Count / 2;

            Utils.dOut("SpriteSheetLoaded", true);
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
            public bool spritesheetloaded { get; set; }
        }

        private long loopCount = 0;

        [JSInvokable]
        public async void GameLoop(GameLoopObject glo)
        {
            if (stopGameLoop || glo.pauseanimation)
            {
                lastTimeStamp = glo.timestamp;
                Utils.dOut("Exited GameLoop", true);
                return;
            }

            try
            {
                loopCount++;

                var timeStamp = glo.timestamp;

                Utils.dOut("GameLoop Running", "LC: " + loopCount + " , TS: " + glo.timestamp);

                if (gameService.animationService != null)
                    gameService.Process(ship);

                //Start Animation Logic
                delta += (int)(timeStamp - lastTimeStamp);
                lastTimeStamp = timeStamp;

                while (delta >= targetTicksPerFrame)
                {
                    animationService.Animate();
                    delta -= targetTicksPerFrame;
                }
                if(loopCount%2==0)
                    animationService.Draw();
                //End Animation Logic

                //Start Curve Editor Logic
                if (glo.editcurveschecked)
                    CurveEditorHelper.EditCurves(animationService, glo);
                else
                    CurveEditorHelper.DisableLines(animationService);
                if (glo.resetanimation) CurveEditorHelper.ResetAnimation(animationService);
                ////End Curve Editor Logic

                Utils.LogFPS();

                KeyBoardHelper.ControlShip(ship);

                await JsRuntime.InvokeAsync<object>("logDiagnosticInfo", Utils.DiagnosticInfo);
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
