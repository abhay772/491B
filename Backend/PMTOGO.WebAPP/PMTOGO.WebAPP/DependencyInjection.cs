using AA.PMTOGO.Services;
using ILogger = AA.PMTOGO.Infrastructure.Interfaces.ILogger;

namespace PMTOGO.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ILogger, Logger>();

            return services;
        }
    }
}
