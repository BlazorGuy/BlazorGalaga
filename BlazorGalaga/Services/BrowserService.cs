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

        public async Task<CansvasDimension> ResizeCanvas()
        {
            return await _js.InvokeAsync<CansvasDimension>("resizeCanvas");
        }
    }

    public class CansvasDimension
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Ratio { get; set; }
        public double ViewportRatio { get; set; }
    }
}