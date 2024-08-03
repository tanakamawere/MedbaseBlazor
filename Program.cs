using MedbaseLibrary.Services;
using MudBlazor;
using MudBlazor.Services;
using MedbaseBlazor;
using System.IdentityModel.Tokens.Jwt;
using MedbaseBlazor.Pages;
using MedbaseLibrary.Essays;
using MedbaseLibrary.Notes;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MedbaseLibrary.Store;
using MedbaseLibrary.Questions;
using MedbaseLibrary.CoursesAndTopics;

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
    //apiString = "https://apimedbase.azurewebsites.net/";
}
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddSingleton<IPlatformInfoService, PlatformInfoService>();
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddTransient<ICheckForInternet, CheckForInternet>();

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
builder.Services.AddHttpClient<IStore, StoreRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});
builder.Services.AddHttpClient<IQuestions, QuestionRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});
builder.Services.AddHttpClient<ICoursesAndTopics, CoursesAndTopicsRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});

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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// new code
app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext) =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri("/")
            .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});
// new code

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");

app.Run();
