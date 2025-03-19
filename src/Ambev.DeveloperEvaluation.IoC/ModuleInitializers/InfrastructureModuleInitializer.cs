using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MongoDB;
using Ambev.DeveloperEvaluation.MongoDB.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        Initialize(builder.Services, builder.Configuration);
    }

    public void Initialize(IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL with EF Core
        services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnection")));

        // MongoDB
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<SaleEventRepository>();

        // Repositories - PostgreSQL
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBranchRepository, Ambev.DeveloperEvaluation.ORM.Repositories.BranchRepository>();
        services.AddScoped<ICustomerRepository, Ambev.DeveloperEvaluation.ORM.Repositories.CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
    }
}
