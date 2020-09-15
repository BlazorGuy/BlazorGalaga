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
using Howler.Blazor.Components;

namespace BlazorGalaga
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<BezierCurveService>();
            builder.Services.AddSingleton<SpriteService>();
            builder.Services.AddSingleton(x =>
                new AnimationService(
                    x.GetRequiredService<BezierCurveService>(),
                    x.GetRequiredService<SpriteService>()
                    )
                );
            builder.Services.AddSingleton<GameService>();
            builder.Services.AddScoped<IHowl, Howl>();
            builder.Services.AddScoped<IHowlGlobal, HowlGlobal>();

            await builder.Build().RunAsync();
        }
    }
}
