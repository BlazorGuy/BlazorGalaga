using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Ship : Animatable, IAnimatable
    {
        public Ship() 
        {
            Sprite = new Sprite(Sprite.SpriteTypes.Ship);
        }
    }
}
