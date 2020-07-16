using System;
using BlazorGalaga.Models;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Static;
using System.Linq;

namespace BlazorGalaga.Services
{
    public class SpriteService
    {
        public Canvas2DContext DynamicCtx { get; set; }
        public Canvas2DContext StaticCtx { get; set; }
        public Canvas2DContext BufferCtx { get; set; }
        public List<Canvas2DContext> BufferCanvases { get; set; }
        public ElementReference SpriteSheet { get; set; }
        public List<Sprite> Sprites = new List<Sprite>();


        public async void Init()
        {
            await DynamicCtx.SetStrokeStyleAsync("white");
            await DynamicCtx.SetFillStyleAsync("yellow");
            await DynamicCtx.SetLineWidthAsync(2);

            await BufferCanvases.FirstOrDefault().DrawImageAsync(
                SpriteSheet, 109,91,16,16,0,0,45,45
            );

            //await StaticCtx.SetFillStyleAsync("yellow");
            //await StaticCtx.FillRectAsync(150, 250, 100, 100);

        }

        public async void DrawSprite(Sprite sprite, PointF location, float rotationangle)
        {

            if (!sprite.IsInitialized)
                SetSpriteInfoBySpriteType(sprite);

            if (rotationangle != 0)
            {
                await DynamicCtx.SaveAsync();
                await DynamicCtx.TranslateAsync(location.X, location.Y);
                await DynamicCtx.RotateAsync((float)((rotationangle + sprite.InitialRotationOffset) * Math.PI / 180));
            }

            await DynamicCtx.DrawImageAsync(
                sprite.BufferCanvas.Canvas,
                rotationangle == 0 ? (int)location.X - (int)sprite.SpriteDestRect.Width / 2 : (int)sprite.SpriteDestRect.Width / 2 * -1, //dest x
                rotationangle == 0 ? (int)location.Y - (int)sprite.SpriteDestRect.Height / 2 : (int)sprite.SpriteDestRect.Height / 2 * -1 //dest y,
            );

            if (rotationangle!=0)
                await DynamicCtx.RestoreAsync();
        }

        private void SetSpriteInfoBySpriteType(Sprite sprite)
        {

            switch (sprite.SpriteType)
            {
                case Sprite.SpriteTypes.Ship:
                    sprite.SpriteSheetRect = new RectangleF(109,1, 16, 16);
                    sprite.SpriteDestRect = new  RectangleF(0, 0, Constants.SpriteDestSize.Width, Constants.SpriteDestSize.Height);
                    sprite.BufferCanvas = BufferCanvases.FirstOrDefault();
                    break;
                case Sprite.SpriteTypes.BlueBug:
                    sprite.SpriteSheetRect = new RectangleF(109, 91, 16, 16);
                    sprite.SpriteDestRect = new RectangleF(0, 0, Constants.SpriteDestSize.Width, Constants.SpriteDestSize.Height);
                    sprite.InitialRotationOffset = -90;
                    sprite.BufferCanvas = BufferCanvases.FirstOrDefault();
                    break;
            }

            sprite.IsInitialized = true;
        }

    }
}
