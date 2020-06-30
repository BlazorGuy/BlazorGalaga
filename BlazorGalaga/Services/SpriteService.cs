using System;
using BlazorGalaga.Models;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

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

        public async void DrawSprite(Sprite sprite)
        {
            SetSpriteInfoBySpriteType(sprite);

            await CanvasCtx.DrawImageAsync(
                SpriteSheet,
                sprite.SpriteSheetRect.X, //source x
                sprite.SpriteSheetRect.Y, //source y
                sprite.SpriteSheetRect.Width, //source width
                sprite.SpriteSheetRect.Height, //source height
                sprite.SpriteDestRect.X, //dest x
                sprite.SpriteDestRect.Y, //dest y
                sprite.SpriteDestRect.Width,//dest width
                sprite.SpriteDestRect.Height //dest height
            );
        }

        private void SetSpriteInfoBySpriteType(Sprite sprite)
        {
            //standard sprite constants
            const int SPRITE_DEST_WIDTH = 45;
            const int SPRITE_DEST_HEIGHT = 32;

            switch (sprite.SpriteType)
            {
                case Sprite.SpriteTypes.Ship:
                    sprite.SpriteSheetRect = new System.Drawing.RectangleF(107.7F,0, 17, 17);
                    sprite.SpriteDestRect = new System.Drawing.RectangleF(0, 0, SPRITE_DEST_WIDTH, SPRITE_DEST_HEIGHT);
                    break;
            }
        }

    }
}
