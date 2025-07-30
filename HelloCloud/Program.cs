using HelloCloud.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;


var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Injection du DbContext
builder.Services.AddDbContext<TestProduitsContext>(options =>
    options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddAzureWebAppDiagnostics();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.Configure<AzureFileLoggerOptions>(options =>
{
    options.FileName = "azurewebapp-";
    options.FileSizeLimit = 10 * 1024 * 1024; // 10 MB
    options.RetainedFileCountLimit = 5;
});

builder.Services.Configure<AzureBlobLoggerOptions>(options =>
{
    options.BlobName = "logAppService.txt";
});
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches(); // facultatif mais recommandé

builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.ResponseType = "code"; // Forcer Authorization Code flow
    options.UsePkce = true;        // Activer PKCE (sécurisé)
    options.SaveTokens = true;

    options.TokenValidationParameters.RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groups";

    options.Events ??= new OpenIdConnectEvents();

    options.Events.OnRedirectToIdentityProvider = context =>
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Redirecting to Identity Provider with parameters: " + context.ProtocolMessage.Parameters);
        return Task.CompletedTask;
    };

    options.Events.OnTokenValidated = context =>
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Token validated. Claims:");
        var claims = context.Principal?.Claims;
        foreach (var claim in claims!)
        {
            logger.LogInformation($"{claim.Type}: {claim.Value}");
        }
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
         policy.RequireClaim("groups", "a15f4898-56f0-448b-8ce0-6b178c90f9aa"));
});


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogDebug("=== Test Log Debug : application démarre ===");
Console.WriteLine("=== Test Console.WriteLine : application démarre ===");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
