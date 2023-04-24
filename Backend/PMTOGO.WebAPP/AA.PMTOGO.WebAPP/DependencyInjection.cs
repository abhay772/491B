using AA.PMTOGO.Authentication;
using AA.PMTOGO.Logging;
using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Libary;
using AA.PMTOGO.Managers;
using AA.PMTOGO.Managers.Interfaces;
using AA.PMTOGO.Services;
using AA.PMTOGO.Services.Interfaces;
using ILogger = AA.PMTOGO.Logging.ILogger;
using AA.PMTOGO.Models;
using AA.PMTOGO.WebAPP.Controllers;
using AA.PMTOGO.DAL.Interfaces;

namespace AA.PMTOGO.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<AA.PMTOGO.Models.Entities.CrimeAlert>();
            services.AddScoped<AA.PMTOGO.Models.Entities.Result>();
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IAccountManager, AccountManager>();
            services.AddTransient<IUserManagement, UserManagement>();
            services.AddTransient<IServiceManager, ServiceManager>();
            services.AddTransient<IServiceManagement, ServiceManagement>();
            services.AddTransient<IServiceRequestManagement, ServiceRequestManagement>();
            services.AddTransient<IServiceRequestManager, ServiceRequestManager>();
            services.AddTransient<IUserServiceManagement, UserServiceManagement>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddTransient<IAuthManager, AuthManager>();
            services.AddTransient<IDIYManager, DIYManager>();
            services.AddTransient<IDIYService, DIYService>();
            services.AddTransient<ICrimeMapDAO, CrimeMapDAO>();
            services.AddTransient<ICrimeMapManager, CrimeMapManager>();
            services.AddTransient<ICrimeMapService, CrimeMapService>();
            services.AddTransient<IHistoricalSalesDAO, HistoricalSalesDAO>();
            services.AddTransient<IPropertyEvaluator, PropertyEvaluator>();
            services.AddTransient<IPropEvalManager, PropEvalManager>();
            services.AddTransient<ISqlPropEvalDAO, SqlPropEvalDAO>();
            services.AddTransient<IUsageAnalysisManager, UsageAnalysisManager>();
            services.AddTransient<InputValidation>();
            services.AddTransient<ClaimValidation>();
            return services;
        }
    }
}
