using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlazorGalaga.Services;

namespace BlazorGalaga
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<BezierCurveService>();
            builder.Services.AddScoped(sp => new AnimationService(builder.Services.BuildServiceProvider()));
            builder.Services.AddScoped<BrowserService>();
            builder.Services.AddScoped<SpriteService>();

            await builder.Build().RunAsync();
        }
    }
}
