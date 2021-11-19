using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EventHorizon.Blazor.Mascot.Sample.Wasm;
using BlazorPro.BlazorSize;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMediaQueryService();
builder.Services.AddScoped<IResizeListener, ResizeListener>();

await builder.Build().RunAsync();
