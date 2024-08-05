using Bodatero.FeatureManagementMinimalAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bodatero.FeatureManagementMinimalAPI
{
    public static class FeatureManagementMinimalApiExtensions
    {
        public static IServiceCollection AddFeatureGateForMinimalAPI(this IServiceCollection services)
        {
            services.AddSingleton<FeatureGateForMinimalApiMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseMinimalApiFeatureGate(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<FeatureGateForMinimalApiMiddleware>();
            return appBuilder;
        }
    }
}
