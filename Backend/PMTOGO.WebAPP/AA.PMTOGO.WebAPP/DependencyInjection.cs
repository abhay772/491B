﻿using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Managers;
using AA.PMTOGO.Services;
using ILogger = AA.PMTOGO.Infrastructure.Interfaces.ILogger;

namespace AA.PMTOGO.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IAccountManager, AccountManager>();
            services.AddTransient<IUserManagement, UserManagement>();
            services.AddTransient<IServiceManager, ServiceManager>();
            services.AddTransient<IRequestManagement, RequestManagement>();
            //services.AddTransient<IAuthManager, AuthManager>();
            //services.AddTransient<IAuthenticator, Authenticator>();
            
            return services;
        }
    }
}
