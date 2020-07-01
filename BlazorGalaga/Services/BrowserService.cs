using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorGalaga.Services
{

    public class BrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<CanvasDimension> ResizeCanvas()
        {
            return await _js.InvokeAsync<CanvasDimension>("resizeCanvas");
        }
        public async Task<CanvasDimension> KeyPress()
        {
            return await _js.InvokeAsync<CanvasDimension>("keyPress");
        }
    }

    public class CanvasDimension
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Ratio { get; set; }
        public double ViewportRatio { get; set; }
    }
}