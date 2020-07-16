using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGalaga.Models
{
    public class Canvas
    {
        public BECanvasComponent CanvasRef { get; set; }
        public Canvas2DContext Context { get; set; }
    }
}
