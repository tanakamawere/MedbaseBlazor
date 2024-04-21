using MedbaseLibrary.Services;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.Identity.Web;
using MedbaseLibrary.MsalClient;
using MedbaseBlazor;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
//Dependencies
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddSingleton<IPCAWrapper, PCAWrapper>();
builder.Services.AddSingleton<AuthenticationStateProvider, MedbaseAuthStateProvider>();

builder.Services.AddHttpClient<IApiRepository, ApiRepository>("ApiData", client =>
{
    client.BaseAddress = new Uri("https://apimedbase.azurewebsites.net/");
    //client.BaseAddress = new Uri("https://localhost:5249/");
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
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
