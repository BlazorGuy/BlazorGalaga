using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class ShipMissle : AnimatableBase
    {
        public bool IsFiring { get; set; }

        public ShipMissle() 
        {
            Sprite = new Sprite(Sprite.SpriteTypes.ShipMissle);
        }
    }
}
