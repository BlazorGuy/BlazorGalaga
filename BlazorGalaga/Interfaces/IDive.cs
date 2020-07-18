using System;
using System.Collections.Generic;
using BlazorGalaga.Models;

namespace BlazorGalaga.Interfaces
{
    public interface IDive
    {
        public List<BezierCurve> GetPaths(IAnimatable animatable, Ship ship);
    }
}
