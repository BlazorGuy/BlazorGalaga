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
        public Canvas2DContext DynamicCtx1 { get; set; }
        public Canvas2DContext DynamicCtx2 { get; set; }
        public Canvas2DContext StaticCtx { get; set; }
        public Canvas2DContext BufferCtx { get; set; }
        public List<Canvas> BufferCanvases { get; set; }
        public ElementReference SpriteSheet { get; set; }
        public List<Sprite> Sprites = new List<Sprite>();


        public async void Init()
        {
            await DynamicCtx1.SetStrokeStyleAsync("white");
            await DynamicCtx1.SetFillStyleAsync("yellow");
            await DynamicCtx1.SetFontAsync("48px serif");
            await DynamicCtx1.SetLineWidthAsync(2);

            await DynamicCtx2.SetStrokeStyleAsync("white");
            await DynamicCtx2.SetFillStyleAsync("yellow");
            await DynamicCtx2.SetFontAsync("48px serif");
            await DynamicCtx2.SetLineWidthAsync(2);

        }

        private bool isrotated = false;

        public async void DrawSprite(Sprite sprite, PointF location, float rotationangle)
        {
            //rotationangle = 0;

            if (!sprite.IsInitialized)
                SetSpriteInfoBySpriteType(sprite);

            if (sprite.DynamicCanvas == null) return;

            if (rotationangle != 0)
            {
                isrotated = true;

                var rotation = (float)((rotationangle + sprite.InitialRotationOffset) * Math.PI / 180);
                var x = Math.Cos(rotation);
                var y = Math.Sin(rotation);

                await sprite.DynamicCanvas.SetTransformAsync(x, y, -y, x, location.X, location.Y);
            }
            else if (isrotated)
            {
                await sprite.DynamicCanvas.SetTransformAsync(1, 0, 0, 1, 0, 0);
                isrotated = false;
            }

            if (sprite.BufferCanvas != null)
            {
                await sprite.DynamicCanvas.DrawImageAsync(
                    sprite.BufferCanvas.Canvas,
                    rotationangle == 0 ? (int)location.X - sprite.SpriteDestRect.Width * .5 : (int)sprite.SpriteDestRect.Width * .5 * -1, //dest x
                    rotationangle == 0 ? (int)location.Y - sprite.SpriteDestRect.Height * .5 : (int)sprite.SpriteDestRect.Height * .5 * -1 //dest y,
                );
            }

        }

        private void SetSpriteInfoBySpriteType(Sprite sprite)
        {

            switch (sprite.SpriteType)
            {
                case Sprite.SpriteTypes.Ship:
                    SetUpSprite(sprite, 0, 109, 1, 0, 0);
                    break;
                case Sprite.SpriteTypes.BlueBug:
                    SetUpSprite(sprite, 1, 109, 91, -90, 0);
                    break;
                case Sprite.SpriteTypes.RedBug:
                    SetUpSprite(sprite, 2, 109, 73, -90, 0);
                    break;
                case Sprite.SpriteTypes.GreenBug:
                    SetUpSprite(sprite, 3, 109, 37, -90, 0);
                    break;
                case Sprite.SpriteTypes.ShipMissle:
                    SetUpSprite(sprite, 4, 310, 120, 0, 0);
                    break;
                case Sprite.SpriteTypes.BlueBug_DownFlap:
                    SetUpSprite(sprite, 5, 127, 91, -90, 0);
                    break;
                case Sprite.SpriteTypes.RedBug_DownFlap:
                    SetUpSprite(sprite, 6, 127, 73, -90, 0);
                    break;
                case Sprite.SpriteTypes.GreenBug_DownFlap:
                    SetUpSprite(sprite, 7, 127, 37, -90, 0);
                    break;
                case Sprite.SpriteTypes.BugMissle:
                    SetUpSprite(sprite, 8, 310, 135, 0, 0);
                    break;
                case Sprite.SpriteTypes.EnemyExplosion1:
                    SetUpSprite(sprite, 9, 292, 1, 0, 0, 32, 32, 90, 90);
                    break;
                case Sprite.SpriteTypes.EnemyExplosion2:
                    SetUpSprite(sprite, 9, 324, 1, 0, 0, 32, 32, 90, 90);
                    break;
                case Sprite.SpriteTypes.EnemyExplosion3:
                    SetUpSprite(sprite, 9, 356, 1, 0, 0, 32, 32, 90, 90);
                    break;
                case Sprite.SpriteTypes.EnemyExplosion4:
                    SetUpSprite(sprite, 9, 388, 1, 0, 0, 32, 32, 90, 90);
                    break;
                case Sprite.SpriteTypes.EnemyExplosion5:
                    SetUpSprite(sprite, 9, 420, 1, 0, 0, 32, 32, 90, 90);
                    break;
            }

            sprite.IsInitialized = true;
        }

        private async void SetUpSprite(Sprite sprite,int bufferindex, int sx, int sy, int rotationoffset,int dynamiccanvasindex, int swidth=0, int sheight=0, int dwidth= 0, int dheight = 0)
        {
            await BufferCanvases[bufferindex].Context.DrawImageAsync(
                SpriteSheet,
                sx,
                sy,
                swidth==0 ? Constants.SpriteSourceSize : swidth,
                sheight==0 ? Constants.SpriteSourceSize: sheight,
                0,
                0,
                dwidth==0 ? Constants.SpriteDestSize.Width : dwidth,
                dheight==0 ? Constants.SpriteDestSize.Height : dheight
            );

            sprite.BufferCanvas = BufferCanvases[bufferindex].Context;
            sprite.DynamicCanvas = dynamiccanvasindex == 0 ? DynamicCtx1 : DynamicCtx2;
            sprite.SpriteDestRect = new RectangleF(0, 0, dwidth==0 ? Constants.SpriteDestSize.Width : dwidth, dheight==0 ? Constants.SpriteDestSize.Height: dheight);
            sprite.InitialRotationOffset = rotationoffset;
        }

    }
}
