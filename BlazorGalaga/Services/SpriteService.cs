using System;
using BlazorGalaga.Models;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Static;

namespace BlazorGalaga.Services
{
    public class SpriteService
    {
        public Canvas2DContext DynamicCtx { get; set; }
        public Canvas2DContext StaticCtx { get; set; }
        public Canvas2DContext BufferCtx { get; set; }
        public ElementReference BufferCtxCanvas { get; set; }
        public ElementReference SpriteSheet { get; set; }
        public List<Sprite> Sprites = new List<Sprite>();


        public async void Init()
        {
            await DynamicCtx.SetStrokeStyleAsync("white");
            await DynamicCtx.SetFillStyleAsync("yellow");
            await DynamicCtx.SetLineWidthAsync(2);

            BufferCtxCanvas = BufferCtx.Canvas;

            await BufferCtx.DrawImageAsync(
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
                BufferCtxCanvas,
                rotationangle == 0 ? (int)location.X - (int)sprite.SpriteDestRect.Width / 2 : (int)sprite.SpriteDestRect.Width / 2 * -1, //dest x
                rotationangle == 0 ? (int)location.Y - (int)sprite.SpriteDestRect.Height / 2 : (int)sprite.SpriteDestRect.Height / 2 * -1 //dest y,
            );

            //await DynamicCtx.DrawImageAsync(
            //    BufferCtx.Canvas,
            //    sprite.SpriteSheetRect.X, //source x
            //    sprite.SpriteSheetRect.Y, //source y
            //    sprite.SpriteSheetRect.Width, //source width
            //    sprite.SpriteSheetRect.Height, //source height
            //    rotationangle == 0 ? (int)location.X - (int)sprite.SpriteDestRect.Width / 2 : (int)sprite.SpriteDestRect.Width / 2 * -1, //dest x
            //    rotationangle == 0 ? (int)location.Y - (int)sprite.SpriteDestRect.Height / 2 : (int)sprite.SpriteDestRect.Height / 2 * -1, //dest y,
            //    sprite.SpriteDestRect.Width,//dest width
            //    sprite.SpriteDestRect.Height //dest height
            //);

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
                    break;
                case Sprite.SpriteTypes.BlueBug:
                    sprite.SpriteSheetRect = new RectangleF(109, 91, 16, 16);
                    sprite.SpriteDestRect = new RectangleF(0, 0, Constants.SpriteDestSize.Width, Constants.SpriteDestSize.Height);
                    sprite.InitialRotationOffset = -90;
                    break;
            }

            sprite.IsInitialized = true;
        }

    }
}
