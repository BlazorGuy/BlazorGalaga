using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Models
{
    public class PathCache
    {
        public BezierCurve Path { get; set; }
        public List<PointF> PathPoints { get; set; }
    }
}
