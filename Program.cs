using MedbaseLibrary.Services;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.Identity.Web;
using MedbaseLibrary.MsalClient;
using MedbaseBlazor;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
//Dependencies
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddSingleton<IPCAWrapper, PCAWrapper>();
builder.Services.AddSingleton<IPlatformInfoService, PlatformInfoService>();
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddTransient<ICheckForInternet, CheckForInternet>();
builder.Services.AddScoped<AuthenticationStateProvider, MedbaseAuthenticationStateProvider>();

string apiString = "https://apimedbase.azurewebsites.net/";
//string apiString = "http://localhost:5249/";

builder.Services.AddHttpClient<IApiRepository, ApiRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
}); 
builder.Services.AddHttpClient<INotesRepository, NotesRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "Settings");
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().
    AddMicrosoftIdentityConsentHandler();

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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
