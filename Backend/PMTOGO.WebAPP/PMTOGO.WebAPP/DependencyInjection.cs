﻿using PMTOGO.Infrastructure.Services;
using ILogger = PMTOGO.Infrastructure.Interfaces.ILogger;

namespace PMTOGO.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();

            return services;
        }
    }
}
