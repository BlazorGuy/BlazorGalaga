using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Models
{
    public class BugMissle : AnimatableBase
    {
        public BugMissle()
        {
            Sprite = new Sprite(Sprite.SpriteTypes.BugMissle);
        }
    }
}
