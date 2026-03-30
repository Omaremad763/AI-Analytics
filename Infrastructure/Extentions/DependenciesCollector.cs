using Application;
using Application.Contracts;
using Application.Contracts.Ai_Insights;
using Application.Contracts.Data_Ingestion;

using FluentValidation;

using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Contracts_Implementation;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Extentions;
public static class DependenciesCollector
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(IApplicationHandlerMarker).Assembly;
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>
         (options =>
         {
             options.UseNpgsql((connectionString));
         });

        #region Hangifre

        services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(connectionString);
    }));
        services.AddHangfireServer();
        #endregion
        services.AddScoped<IAIAnalyticsServices, AIAnalyticsServices>();
        services.AddHttpClient<IAIInsightService, AiInsightService>();
       services.AddScoped<IUnitofWork, UnitofWork>();
        services.AddScoped<IDataInegstionService, DataInegstionService>();
        #region Mediator
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly);
        #endregion

        return services;
    }
}