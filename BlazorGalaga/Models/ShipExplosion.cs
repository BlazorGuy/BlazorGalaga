using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Models
{
    public class ShipExplosion : AnimatableBase
    {
        public ShipExplosion()
        {
            Sprite = new Sprite(Sprite.SpriteTypes.ShipExplosion1);
            SpriteBank = new List<Sprite>();
        }
    }
}
