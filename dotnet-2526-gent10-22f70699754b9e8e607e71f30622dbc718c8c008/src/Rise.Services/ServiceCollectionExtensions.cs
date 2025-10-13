using Microsoft.Extensions.DependencyInjection;
using Rise.Persistence.Repositories;
using Rise.Persistence;
using Rise.Services.Facilities;
using Rise.Services.Identity;
using Rise.Services.ChatMessages;
using Rise.Services.Products;
using Rise.Services.Projects;
using Rise.Shared.Facility;
using Rise.Shared.Identity.Accounts;
using Rise.Shared.ChatMessages;
using Rise.Shared.Products;
using Rise.Shared.Projects;


namespace Rise.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IFacilityService, FacilityService>();
        services.AddScoped<IProductService, ProductService>();        
        services.AddScoped<IProjectService, ProjectService>();    
        services.AddScoped<IChatMessageService, ChatMessageService>(); 
        services.AddScoped<IFriendRepository, FriendRepository>();
        services.AddScoped<IFriendService, FriendService>();    
        services.AddTransient<DbSeeder>();       
        
        // Add other application services here.
        return services;
    }
}