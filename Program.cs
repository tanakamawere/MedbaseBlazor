using MedbaseLibrary.Services;
using MudBlazor;
using MudBlazor.Services;
using MedbaseBlazor;
using System.IdentityModel.Tokens.Jwt;
using MedbaseLibrary.Notes;
using MedbaseBlazor.Pages;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using MedbaseLibrary.CoursesAndTopics;
using MedbaseLibrary.Essays;

var builder = WebApplication.CreateBuilder(args);

var environment = args.Contains("--local") ? Environments.Development : Environments.Production;

//Write to console which environment is being used
Console.WriteLine($"Medbase API Environment: {environment}");

//Dependencies
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddRazorComponents().AddInteractiveServerComponents();


string apiString = "https://apimedbase.azurewebsites.net/";

if (environment == Environments.Development)
{
    apiString = "http://localhost:5249/";
}
else
{
    //apiString = "http://localhost:5249/";
    apiString = "https://apimedbase.azurewebsites.net/";
}
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.CallbackPath = "/callback";
});

builder.Services.AddHttpClient("ApiData", client =>
{
    client.BaseAddress = new Uri(apiString);
});

//Registering services
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddTransient<IEssaysRepository, EssaysRepository>();
builder.Services.AddSingleton<IPlatformInfoService, PlatformInfoService>();
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddTransient<ICheckForInternet, CheckForInternet>();
builder.Services.AddScoped<ICoursesAndTopics, CoursesAndTopicsRepository>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();

var app = builder.Build();

//Auth0 Fix Code

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto |
            ForwardedHeaders.XForwardedHost
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Auth Code
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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

//Status code
app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(MedbaseComponents._Imports).Assembly);

app.Run();
