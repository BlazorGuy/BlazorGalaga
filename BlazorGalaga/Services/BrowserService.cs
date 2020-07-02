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


        public async Task ResizeCanvas()
        {
            await _js.InvokeAsync<object>("resizeCanvas");
        }
        
    }
}