using MedbaseLibrary.Services;
using MudBlazor;
using MudBlazor.Services;
using MedbaseBlazor;
using System.IdentityModel.Tokens.Jwt;
using MedbaseBlazor.Pages;
using MedbaseLibrary.CoursesAndTopics;
using MedbaseLibrary.Essays;
using MedbaseLibrary.Notes;
using Medbase.Application.Clients;
using Medbase.Application.Services;
using Medbase.Application;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var environment = args.Contains("--local") ? Environments.Development : Environments.Production;


// Api client registration
builder.Services.AddHttpClient<IApiClient, ApiClient>("MedbaseApiClient", client =>
{
    string? baseAddress = builder.Configuration["Endpoints:Local"];
    if (environment == Environments.Development)
    {
        baseAddress = builder.Configuration["Endpoints:Local"];
    }

    if (string.IsNullOrEmpty(baseAddress))
    {
        throw new ArgumentNullException(nameof(baseAddress), "Medbase endpoint configuration is missing or empty.");
    }
    client.BaseAddress = new Uri(baseAddress);
});

//builder.Services.AddHttpClient<IApiRepository, ApiRepository>("ApiData", client =>
//{
//    string apiString = "http://localhost:5249/";

//    if (environment == Environments.Development)
//        apiString = "http://localhost:5249/";
//    client.BaseAddress = new Uri(apiString);
//});


//Client registration
builder.Services.AddScoped<CoursesClient>();
builder.Services.AddScoped<TopicsClient>();
builder.Services.AddScoped<NotesClient>();
builder.Services.AddScoped<EssaysClient>();
builder.Services.AddScoped<Question2Client>();
builder.Services.AddScoped<QuestionClient>();
builder.Services.AddScoped<TopicsClient>();
builder.Services.AddScoped<UserCoursePreferencesClient>();
builder.Services.AddScoped<UserQuizInteractionClient>();
builder.Services.AddScoped<UserClient>();

builder.Services.AddScoped<UserService>();


//Registering services
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddTransient<IEssaysRepository, EssaysRepository>();
builder.Services.AddSingleton<IPlatformInfoService, PlatformInfoService>();
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddTransient<ICheckForInternet, CheckForInternet>();
builder.Services.AddScoped<ICoursesAndTopics, CoursesAndTopicsRepository>();

builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = "dev-nema48ewf82jkozq.us.auth0.com";
    options.ClientId = "tQHTAlr5YIfEyJjgzz7L0j4vAuEYEKjg";
});
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

app.UseAntiforgery();


app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

    await httpContext.ChallengeAsync("Auth0", authenticationProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext) =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri("/")
            .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");

app.Run();
