using System;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Bug : Animatable, IAnimatable
    {
        public Bug()
        {
            Sprite = new Sprite(Sprite.SpriteTypes.BlueBug);
        }
    }
}
