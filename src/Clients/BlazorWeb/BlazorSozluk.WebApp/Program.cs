using Blazored.LocalStorage;
using BlazorSozluk.WebApp;
using BlazorSozluk.WebApp.Infrastructure.Auth;
using BlazorSozluk.WebApp.Infrastructure.Services.Interfaces;
using BlazorSozluk.WebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WebApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7226");
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("WebApiClient");
});

builder.Services.AddScoped<AuthTokenHandler>();

builder.Services.AddTransient<IEntryService, EntryService>();
builder.Services.AddTransient<IVoteService, VoteService>();
builder.Services.AddTransient<IFavService, FavService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();