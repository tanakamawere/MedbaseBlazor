using MedbaseLibrary.Services;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.Identity.Web;
using MedbaseLibrary.MsalClient;
using MedbaseBlazor;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Components.Authorization;
using MedbaseBlazor.Pages;
using Microsoft.AspNetCore.Authentication.Cookies;
using MedbaseLibrary.Auth;
using MedbaseLibrary.Essays;
using MedbaseLibrary.Notes;
using MedbaseComponents.Pages;
using MedbaseComponents.Shared;

var builder = WebApplication.CreateBuilder(args);

var environment = args.Contains("--local") ? Environments.Development : Environments.Production;

//Dependencies
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;


string apiString = "http://localhost:5249/";

if (environment == Environments.Development)
{
    apiString = "http://localhost:5249/";
}
else
{
    apiString = "http://localhost:5249/";
}

builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IPCAWrapper, PCAWrapper>();
builder.Services.AddSingleton<IPlatformInfoService, PlatformInfoService>();
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddTransient<ICheckForInternet, CheckForInternet>();
builder.Services.AddScoped<CommonAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CommonAuthStateProvider>());
builder.Services.AddScoped<IAuthMemory, AuthMemory>();

builder.Services.AddHttpClient<IApiRepository, ApiRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});
builder.Services.AddHttpClient<INotesRepository, NotesRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});
builder.Services.AddHttpClient<IEssaysRepository, EssaysRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});
builder.Services.AddHttpClient<IAuthService, AuthService>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "Settings");
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.UseAntiforgery();

app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");

app.Run();
