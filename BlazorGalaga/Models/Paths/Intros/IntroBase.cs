using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BlazorGalaga.Interfaces;

namespace BlazorGalaga.Models.Paths.Intros
{
    public class IntroBase : IIntro
    {
        public int Offset { get; set; }
        public bool IsChallenge { get; set; }
        public IntroLocation IntroLocation { get; set; }
        public virtual List<BezierCurve> GetPaths() { return null; }
    }
}
