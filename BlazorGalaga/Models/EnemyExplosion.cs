using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Models
{
    public class EnemyExplosion : AnimatableBase
    {
        public EnemyExplosion()
        {
            Sprite = new Sprite(Sprite.SpriteTypes.EnemyExplosion1);
        }
    }
}
