using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System;
using System.Drawing;

namespace BlazorGalaga.Models
{
    public class Sprite
    {
        public enum SpriteTypes
        {
            Ship,
            BlueBug
        }

        public RectangleF SpriteDestRect { get; set; }
        public SpriteTypes SpriteType { get; set; }
        public bool IsInitialized { get; set; }
        public float InitialRotationOffset { get; set; }
        public Canvas2DContext BufferCanvas { get; set; }

        public Sprite(SpriteTypes spritetype)
        {
            SpriteType = spritetype;
        }
    }
}
