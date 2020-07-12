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
        public Canvas2DContext CanvasCtx { get; set; }
        public Canvas2DContext BufferCanvasCtx { get; set; }
        public ElementReference SpriteSheet { get; set; }
        public List<Sprite> Sprites = new List<Sprite>();


        public void Init()
        {
            CanvasCtx.SetStrokeStyleAsync("white");
            CanvasCtx.SetFillStyleAsync("yellow");
            CanvasCtx.SetLineWidthAsync(2);
        }

        public async void DrawSprite(Sprite sprite, PointF location, float rotationangle)
        {
            if (!sprite.IsInitialized)
                SetSpriteInfoBySpriteType(sprite);

            await CanvasCtx.SaveAsync();
            await CanvasCtx.TranslateAsync(location.X, location.Y);
            await CanvasCtx.RotateAsync((float)((rotationangle + sprite.InitialRotationOffset) * Math.PI / 180));

            await CanvasCtx.DrawImageAsync(
                SpriteSheet,
                sprite.SpriteSheetRect.X, //source x
                sprite.SpriteSheetRect.Y, //source y
                sprite.SpriteSheetRect.Width, //source width
                sprite.SpriteSheetRect.Height, //source height
                (int)sprite.SpriteDestRect.Width / 2 * -1, //dest x
                (int)sprite.SpriteDestRect.Height / 2 * -1, //dest y
                sprite.SpriteDestRect.Width,//dest width
                sprite.SpriteDestRect.Height //dest height
            );

            await CanvasCtx.RestoreAsync();
        }

        private void SetSpriteInfoBySpriteType(Sprite sprite)
        {

            switch (sprite.SpriteType)
            {
                case Sprite.SpriteTypes.Ship:
                    sprite.SpriteSheetRect = new System.Drawing.RectangleF(109,1, 16, 16);
                    sprite.SpriteDestRect = new System.Drawing.RectangleF(0, 0, Constants.SpriteDestSize.Width, Constants.SpriteDestSize.Height);
                    break;
                case Sprite.SpriteTypes.BlueBug:
                    sprite.SpriteSheetRect = new System.Drawing.RectangleF(109, 91, 16, 16);
                    sprite.SpriteDestRect = new System.Drawing.RectangleF(0, 0, Constants.SpriteDestSize.Width, Constants.SpriteDestSize.Height);
                    sprite.InitialRotationOffset = -90;
                    break;
            }

            sprite.IsInitialized = true;
        }

    }
}
