using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class TractorBeam : AnimatableBase
    {
        public TractorBeam()
        {
            Sprite = new Sprite(Sprite.SpriteTypes.TractorBeam);
            SpriteBank = new List<Sprite>();
        }
    }
}
