﻿using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleStore.Infrastructure.Common.Extensions;
using SimpleStore.Infrastructure.EfCore;
using SimpleStore.Infrastructure.EfCore.Persistence;
using SimpleStore.Inventory.Infrastructure.EfCore.Persistence;

namespace SimpleStore.Inventory.Infrastructure.EfCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomInfrastructure(this IServiceCollection services)
        {
            services
                .AddEfCore()
                .AddDbContext<InventoryDbContext>((serviceProvider, dbContextOptionBuilder) =>
                {
                    var extendOptionsBuilder = serviceProvider.GetRequiredService<IExtendDbContextOptionsBuilder>();
                    var connStringFactory = serviceProvider.GetRequiredService<IConnectionStringFactory>();
                    extendOptionsBuilder.Extend(dbContextOptionBuilder, connStringFactory, Assembly.GetExecutingAssembly().GetName().Name);
                })
                .AddScoped<DbContext>(serviceProvider => serviceProvider.GetRequiredService<InventoryDbContext>())
                .AddCustomHostedServices();

            services
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddCustomRequestValidation();

            return services;
        }
    }
}
