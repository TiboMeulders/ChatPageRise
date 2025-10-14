using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Persistence.Triggers;
using Rise.Server.Identity;
using Rise.Services;
using Rise.Services.Identity;
using Serilog.Events;
using MongoDB.Driver;
using Rise.Server.Endpoints.ChatMessages;
using Rise.Server.Hubs;
using Rise.Server.Processors;
using ServerVersion = Microsoft.EntityFrameworkCore.ServerVersion;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger(); // Initial log setup, will be overwritten by Serilog, but we need a logger before Dependency Injection is activated.

try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);

    // Add CORS services
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowBlazorClient", policy =>
        {
            policy.WithOrigins(
                    "https://localhost:7214",
                    "http://localhost:5000",
                    "https://localhost:5000",
                    "http://127.0.0.1:5000",
                    "https://127.0.0.1:5000",
                    "https://localhost:5001",
                    "https://127.0.0.1:5001"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });

    });

    builder.Services
        .AddSerilog((_, lc) => lc.ReadFrom.Configuration(builder.Configuration)) // Configuration in AppSettings.json
        .AddDbContext<ApplicationDbContext>(o =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection") ??
                                   throw new InvalidOperationException("Connection string 'DatabaseConnection' not found.");
            o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); 
            o.EnableDetailedErrors();
            if (builder.Environment.IsDevelopment())
            {
                o.EnableSensitiveDataLogging(); // only enabled in development.
            }
            o.UseTriggers(options => options.AddTrigger<EntityBeforeSaveTrigger>()); // Handles all UpdatedAt, CreatedAt stuff.
        })
        .AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .Services
        // Configure authentication cookie for cross-origin usage
        .ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddHttpContextAccessor()
        .AddScoped<ISessionContextProvider, HttpContextSessionProvider>() // Provides the current user from the HttpContext to the session provider.
        .AddApplicationServices() // You'll need to add your own services in this function call.
        .AddAuthorization()
        .AddFastEndpoints(o =>
        {
            o.IncludeAbstractValidators = true;
            o.Assemblies = new[] 
            { 
                typeof(Create).Assembly, // ChatMessages endpoint
                typeof(Rise.Shared.Products.ProductRequest).Assembly  // Optional: other assemblies
            };
        })
        .SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "RISE API";
            };
        });
    
    builder.Services.AddSignalR(); 
    
    builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            [ "application/octet-stream" ]);
    });

    var mongoConnection = builder.Configuration.GetConnectionString("ChatDatabaseConnection"); 
    var mongoDatabaseName = builder.Configuration["DatabaseName"]; 
 
    // Register MongoDB client and database 
    builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnection)); 
    builder.Services.AddSingleton<IMongoDatabase>(sp => 
    { 
        var client = sp.GetRequiredService<IMongoClient>(); 
        return client.GetDatabase(mongoDatabaseName); 
    });
    
    builder.Services.AddScoped<MongoDbSeeder>();
    
    var app = builder.Build();
    app.UseResponseCompression();
    // apply Database migraticons on startup, not so wise in production (Use Generated SQL Scripts) 
    // See: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli
     
    
    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
            
            // Delete and recreate the database
            //await dbContext.Database.EnsureDeletedAsync();
            //await dbContext.Database.EnsureCreatedAsync();
            
            // Seed the database with test data
            // dbContext.Database.EnsureDeleted(); // Delete the database if it exists to clean it up if needed.

            dbContext.Database.Migrate(); // Creates the database if it doesn't exist and applies all migrations. See Readme.md for more info.
            await dbSeeder.SeedAsync(); // Seeds the database with some test data.
            
            var mongoSeeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
            await mongoSeeder.SeedAsync();
        }
        
    }
    // Theses middlewares are strict in order of calling!
    app.UseHttpsRedirection()
        .UseCors("AllowBlazorClient"); // Add CORS middleware
    
    app.UseBlazorFrameworkFiles() // Blazor is also served from the API. 
        .UseStaticFiles()
        .UseDefaultExceptionHandler()
        .UseAuthentication()
        .UseAuthorization()
        .UseFastEndpoints(
         o =>
        {
            o.Endpoints.Configurator = ep =>
            {
                ep.DontAutoSendResponse();
                ep.PreProcessor<GlobalRequestLogger>(Order.Before);
                ep.PostProcessor<GlobalResponseSender>(Order.Before);
                ep.PostProcessor<GlobalResponseLogger>(Order.Before);
            };
        }
            )
        .UseSwaggerGen();
    app.MapHub<PrivateChatHub>("/chathub");
    app.MapFallbackToFile("index.html"); // Serves the Blazor app from the API, when no routes match.
    app.Run();
    
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}
