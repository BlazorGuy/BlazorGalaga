using System;
using System.Drawing;

namespace BlazorGalaga.Models
{
    public class Sprite
    {
        public enum SpriteTypes
        {
            Ship
        }

        public RectangleF SpriteSheetRect { get; set; }
        public RectangleF SpriteDestRect { get; set; }
        public SpriteTypes SpriteType { get; set; }

        public Sprite(SpriteTypes spritetype)
        {
            SpriteType = spritetype;
        }
    }
}
