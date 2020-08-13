using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models
{
    public class Bug : AnimatableBase
    {
        public Point HomePoint { get; set; }
        public bool IsDiving { get; set; }
        public bool IsExploding { get; set; }
        public List<Bug> ChildBugs { get; set; }
        public bool IsDiveBomber { get; set; }
        public Vector2 DiveBombLocation { get; set; }
        public int Wave { get; set; }
        public Point ChildBugOffset { get; set; }
        public string Tag { get; set; }
        public Bug(Sprite.SpriteTypes spritetype)
        {
            Sprite = new Sprite(spritetype);
            SpriteBank = new List<Sprite>();
            ChildBugs = new List<Bug>();
        }
    }
}
