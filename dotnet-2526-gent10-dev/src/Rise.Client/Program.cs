using System;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Rise.Client;
using Rise.Client.ChatMessages;
using Rise.Client.Identity;
using Rise.Client.Products;
using Rise.Client.Services;
using Rise.Client.Services.ChatMessages;
using Rise.Shared.ChatMessages;
using Rise.Shared.Facility;
using Rise.Shared.Products;
using Serilog;

try
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.BrowserConsole(outputTemplate: "[{Timestamp:HH:mm:ss}{Level:u3}]{Message:lj} {NewLine}{Exception}")
        .CreateLogger();

    Log.Information("Starting web application");

    // Add authorization services
    builder.Services.AddAuthorizationCore();

    // Cookie handler for managing authentication cookies
    builder.Services.AddTransient<CookieHandler>();

    // Real authentication state provider that communicates with the backend server
    builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

    // Register the account management interface so it can be injected into components
    builder.Services.AddScoped(sp => (IAccountManager)sp.GetRequiredService<AuthenticationStateProvider>());
    builder.Services.AddScoped<AccountService>();
    
    // register PageTitleService
    builder.Services.AddScoped<PageTitleService>();
    
    // ✅ Register NavBarVisibilityService
    builder.Services.AddScoped<NavBarVisibilityService>();
    
    builder.Services.AddScoped<IChatConnectionService, ChatConnectionService>();

    // Configure HttpClient for secure API calls to the backend
    builder.Services.AddHttpClient("SecureApi",
            opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001"))
        .AddHttpMessageHandler<CookieHandler>();

    builder.Services.AddHttpClient<IFacilityService, FacilityService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001");
    });

    builder.Services.AddHttpClient<IChatMessageService, ChatMessageService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001");
    });
    
    builder.Services.AddHttpClient<IChatContactService, ChatContactService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001");
    }).AddHttpMessageHandler<CookieHandler>();  // Belangrijk voor authenticatie!
    
    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An exception occurred while creating the WASM host");
    throw;
}