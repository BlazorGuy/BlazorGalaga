using System;
using System.Collections.Generic;
using BlazorGalaga.Models;

namespace BlazorGalaga.Interfaces
{
    public interface IIntro
    {
        public List<BezierCurve> GetPaths();
    }
}
