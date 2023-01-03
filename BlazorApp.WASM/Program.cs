
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Main>("#main");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddSingleton<IContentAccess,ContentAccess>()
    .AddBlazorApp() // See BlazorApp/Config/CofigureBlazorApp.cs
    .AddSingleton<IContentAccess, ContentAccess>();
;

await builder.Build().RunAsync();
