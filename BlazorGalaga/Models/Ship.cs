using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Ship : AnimatableBase
    {
        public Ship() 
        {
            Sprite = new Sprite(Sprite.SpriteTypes.Ship);
        }
    }
}
