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
        public List<Canvas> BigBufferCanvases { get; set; }

        private Canvas2DContext DynamicCtx1;
        private Canvas2DContext DynamicCtx2;
        private Canvas2DContext StaticCtx;
        private bool stopGameLoop;
        private readonly int targetTicksPerFrame = (1000 / 60);
        private float delta;
        private float lastTimeStamp;
        private int drawmod = 2;
        private long loopCount = 0;

        protected BECanvasComponent StaticCanvas;
        protected BECanvasComponent DynamicCanvas1;
        protected BECanvasComponent DynamicCanvas2;
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
                BufferCanvases.Add(new Canvas() {
                    CanvasRef = new BECanvasComponent(),
                    Width = Constants.SpriteDestSize.Width,
                    Height = Constants.SpriteDestSize.Height
                });

            BigBufferCanvases = new List<Canvas>();

            for (int i = 1; i <= Constants.BigSpriteBufferCount; i++)
                BigBufferCanvases.Add(new Canvas()
                {
                    CanvasRef = new BECanvasComponent(),
                    Width = Constants.BigSpriteDestSize.Width,
                    Height = Constants.BigSpriteDestSize.Height
                });
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            DynamicCtx1 = await DynamicCanvas1.CreateCanvas2DAsync();
            DynamicCtx2 = await DynamicCanvas2.CreateCanvas2DAsync();
            StaticCtx = await StaticCanvas.CreateCanvas2DAsync();

            foreach (var canvas in BufferCanvases)
               canvas.Context = await canvas.CanvasRef.CreateCanvas2DAsync();

            foreach (var canvas in BigBufferCanvases)
                canvas.Context = await canvas.CanvasRef.CreateCanvas2DAsync();

            await JsRuntime.InvokeAsync<object>("initFromBlazor", DotNetObjectReference.Create(this));
        }

        [JSInvokable("SpriteSheetLoaded")]
        public void SpriteSheetLoaded()
        {

            spriteService.DynamicCtx1 = DynamicCtx1;
            spriteService.DynamicCtx2 = DynamicCtx2;
            spriteService.StaticCtx = StaticCtx;
            spriteService.BufferCanvases = BufferCanvases;
            spriteService.BigBufferCanvases = BigBufferCanvases;

            spriteService.SpriteSheet = spriteSheet;
            spriteService.Init();

            gameService.animationService = animationService;
            gameService.spriteService = spriteService;
            gameService.Init();

            ship = (Ship)animationService.Animatables.FirstOrDefault(a => a.Sprite.SpriteType == Sprite.SpriteTypes.Ship);
        }

        public class GameLoopObject
        {
            public float timestamp { get; set; }
            public bool editcurveschecked { get; set; }
            public bool pauseanimation { get; set; }
            public bool killbugs { get; set; }
            public bool resetanimation { get; set; }
            public bool spritesheetloaded { get; set; }
        }

        Stopwatch sw = new Stopwatch();
        long totaldraw = 0;
        long maxdraw = 0;

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

                //Start Animation Logic
                delta += (int)(timeStamp - lastTimeStamp);
                lastTimeStamp = timeStamp;

                while (delta >= targetTicksPerFrame)
                {
                    sw.Restart();
                    animationService.Animate();
                    Utils.dOut("animationService.Animate()", sw.ElapsedMilliseconds);
                    delta -= targetTicksPerFrame;
                }

                if (Utils.FPS > 50 && Utils.FPS <= 55)
                    drawmod = 3;
                else if (Utils.FPS > 45 && Utils.FPS <= 50)
                    drawmod = 4;
                else if (Utils.FPS <= 45)
                    drawmod = 5;
                else
                    drawmod = 2;

                if (loopCount % drawmod == 0)
                {
                    sw.Restart();
                    animationService.Draw();
                    if (sw.ElapsedMilliseconds >=8)
                        totaldraw += sw.ElapsedMilliseconds;
                    if (sw.ElapsedMilliseconds > maxdraw && sw.ElapsedMilliseconds < 50) maxdraw = sw.ElapsedMilliseconds;
                    Utils.dOut("animationService.Draw()", sw.ElapsedMilliseconds + " Avg: " + ((int)totaldraw/(loopCount/drawmod)) + " Max: " + maxdraw);

                    sw.Restart();
                    if (gameService.animationService != null)
                    {
                        if (gameService.Ship == null) gameService.Ship = ship;
                        gameService.Process(timeStamp);
                        Utils.dOut("gameService.Process()", sw.ElapsedMilliseconds);
                    }
                }
                //End Animation Logic

                //Start Curve Editor Logic
                if (glo.editcurveschecked)
                    CurveEditorHelper.EditCurves(animationService, glo);
                else
                    CurveEditorHelper.DisableLines(animationService);
                if (glo.resetanimation) CurveEditorHelper.ResetAnimation(animationService);
                ////End Curve Editor Logic

                if (glo.killbugs)
                {
                    animationService.Animatables.RemoveAll(a =>
                    a.Sprite.SpriteType == Sprite.SpriteTypes.BlueBug
                    || a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug
                    || a.Sprite.SpriteType == Sprite.SpriteTypes.RedBug
                    || a.Sprite.SpriteType == Sprite.SpriteTypes.GreenBug_Blue);
                }

                Utils.LogFPS();

                KeyBoardHelper.ControlShip(ship,animationService);

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
