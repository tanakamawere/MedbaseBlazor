using MedbaseLibrary.Services;
using MudBlazor;
using MudBlazor.Services;
using MedbaseBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseSentry(dsn: "https://33e9efbdf42e02a857a7e54fde618aab@o4505919081873408.ingest.us.sentry.io/4506879230935040");

// Add services to the container.
//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
//builder.Services.AddControllersWithViews()
//    .AddMicrosoftIdentityUI();
//Dependencies
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddMemoryCache();

//How to remember the user that has been logged in
builder.Services.AddScoped<IAuthMemory, JwtCache>();
builder.Services.AddScoped<AuthenticationStateProvider, MedbaseAuthStateProvider>();
builder.Services.AddHttpClient<IApiRepository, ApiRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri("https://apimedbase.azurewebsites.net/");
});
builder.Services.AddHttpClient<IAuthService, AuthService>("AuthAPI", client =>
{
    client.BaseAddress = new Uri("https://apimedbase.azurewebsites.net/");
});
builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    //options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//.AddMicrosoftIdentityConsentHandler();

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
