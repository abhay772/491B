using AA.PMTOGO.Authentication;
using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Libary;
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
            services.AddTransient<IRequestManager, RequestManager>();
            services.AddTransient<IServiceManagement, ServiceManagement>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddTransient<IAuthManager, AuthManager>();
            services.AddTransient<IHistoricalSalesDAO, HistoricalSalesDAO>();
            services.AddTransient<IPropertyEvaluator, PropertyEvaluator>();
            services.AddTransient<IPropEvalManager, PropEvalManager>();
            services.AddTransient<ISqlPropEvalDAO, SqlPropEvalDAO>();
            services.AddTransient<InputValidation>();
            return services;
        }
    }
}
