using MedbaseLibrary.Services;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
//builder.Services.AddControllersWithViews()
//    .AddMicrosoftIdentityUI();
//Dependencies
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddHttpClient<IApiRepository, ApiRepository>(client =>
{
    client.BaseAddress = new Uri("http://apimedbase.azurewebsites.net/");
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
