using FastEndpoints;
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
    .CreateBootstrapLogger();

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
        .AddSerilog((_, lc) => lc.ReadFrom.Configuration(builder.Configuration))
        .AddDbContext<ApplicationDbContext>(o =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection") ??
                                   throw new InvalidOperationException("Connection string 'DatabaseConnection' not found.");
            o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            o.EnableDetailedErrors();
            if (builder.Environment.IsDevelopment())
            {
                o.EnableSensitiveDataLogging();
            }
            o.UseTriggers(options => options.AddTrigger<EntityBeforeSaveTrigger>());
        })
        .AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .Services
        .ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddHttpContextAccessor()
        .AddScoped<ISessionContextProvider, HttpContextSessionProvider>()
        .AddApplicationServices()
        .AddAuthorization()
        .AddFastEndpoints(o =>
        {
            o.IncludeAbstractValidators = true;
            o.Assemblies = new[]
            {
                typeof(Rise.Shared.Products.ProductRequest).Assembly,
                typeof(Rise.Server.Endpoints.Identity.Accounts.GetById).Assembly
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
    
    if (app.Environment.IsDevelopment())
    {
        using (var scope = app.Services.CreateScope())
        {
            
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync(); 
            await dbContext.Database.EnsureCreatedAsync(); 
            var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
            
            dbContext.Database.Migrate();
            await dbSeeder.SeedAsync();
            
            var mongoSeeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
            await mongoSeeder.SeedAsync();
        }
    }
    
    app.UseHttpsRedirection()
        .UseCors("AllowBlazorClient");
    
    app.UseBlazorFrameworkFiles()
        .UseStaticFiles()
        .UseDefaultExceptionHandler()
        .UseAuthentication()
        .UseAuthorization()
        .UseFastEndpoints(o =>
        {
            o.Endpoints.Configurator = ep =>
            {
                ep.DontAutoSendResponse();
                ep.PreProcessor<GlobalRequestLogger>(Order.Before);
                ep.PostProcessor<GlobalResponseSender>(Order.Before);
                ep.PostProcessor<GlobalResponseLogger>(Order.Before);
            };
        })
        .UseSwaggerGen();
    
    app.MapHub<PrivateChatHub>("/chathub");
    app.MapFallbackToFile("index.html");
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