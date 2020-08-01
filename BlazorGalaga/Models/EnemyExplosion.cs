using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using BlazorGalaga.Static;

namespace BlazorGalaga.Models
{
    public class EnemyExplosion : AnimatableBase
    {
        public EnemyExplosion()
        {
            Sprite = new Sprite(Sprite.SpriteTypes.EnemyExplosion1);
            SpriteBank = new List<Sprite>();
        }
    }
}
