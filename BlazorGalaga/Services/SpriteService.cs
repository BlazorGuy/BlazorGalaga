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
        public ElementReference SpriteSheet { get; set; }
        public List<Sprite> Sprites = new List<Sprite>();


        public SpriteService()
        {
            
        }

        public async void DrawSprite(Sprite sprite, PointF location)
        {
            if (!sprite.IsInitialized)
                SetSpriteInfoBySpriteType(sprite);

            await CanvasCtx.DrawImageAsync(
                SpriteSheet,
                sprite.SpriteSheetRect.X, //source x
                sprite.SpriteSheetRect.Y, //source y
                sprite.SpriteSheetRect.Width, //source width
                sprite.SpriteSheetRect.Height, //source height
                (int)location.X, //dest x convert to int to avoid weird clipping of drawings
                (int)location.Y, //dest y
                sprite.SpriteDestRect.Width,//dest width
                sprite.SpriteDestRect.Height //dest height
            );
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
                    sprite.SpriteSheetRect = new System.Drawing.RectangleF(109, 92, 16, 16);
                    sprite.SpriteDestRect = new System.Drawing.RectangleF(0, 0, Constants.SpriteDestSize.Width, Constants.SpriteDestSize.Height);
                    break;
            }

            sprite.IsInitialized = true;
        }

    }
}
