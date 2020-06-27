using System;
using System.Collections.Generic;

namespace BlazorGalaga.Models
{
    public class Animation
    {

        public float Percent { get; set; }
        public float Speed { get; set; }

        public List<Word> Words { get; set; }

        public Animation()
        {
            Speed = 1;
            Percent = 0;
            Words = new List<Word>();
        }
    }
}
