﻿using Contracts.Logging;
using Contracts.Managers;
using LoggerService.NLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Repositories.Contexts;
using Repositories.Managers;
using Services;
using Services.Contracts;

namespace API.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
        => services.AddCors(opt =>
           {
               opt.AddPolicy("CorsPolicy",
                   builder => builder.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader()
                                     .WithExposedHeaders("X-Pagination"));

           });

    public static void ConfigureISSIntegration(this IServiceCollection services)
        => services.Configure<IISOptions>(opt => { });

    public static void ConfigureLoggerService(this IServiceCollection services)
        => services.AddSingleton<ILoggerManager, NLogLoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection servies)
        => servies.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection servies)
      => servies.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddSqlServer<RepositoryContext>((configuration
        .GetConnectionString("sqlConnection")));

    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

            if (systemTextJsonOutputFormatter !=null)
                systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.hateoas+json");


            var xmlOutputFormatter = config.OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

            if (xmlOutputFormatter != null)
                xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.codemaze.hateoas+json");
        });
    }
}

