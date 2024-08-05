using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Bodatero.FeatureManagementMinimalAPI.Middlewares
{
    internal class FeatureGateForMinimalApiMiddleware : IMiddleware
    {
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<FeatureGateForMinimalApiMiddleware> _logger;

        public FeatureGateForMinimalApiMiddleware(IFeatureManager featureManager, ILogger<FeatureGateForMinimalApiMiddleware> logger)
        {
            _featureManager = featureManager;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Endpoint? endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null)
            {
                FeatureGateAttribute? featureGateAttribute = endpoint.Metadata.GetMetadata<FeatureGateAttribute>();
                if (featureGateAttribute is not null)
                {
                    bool[] results = await Task.WhenAll(featureGateAttribute.Features.Select(f => _featureManager.IsEnabledAsync(f)));
                    if (!results.All(b => b))
                    {
                        _logger.LogInformation("Feature gate check failed for {Endpoint}. Required features: {Features}",
                            endpoint.DisplayName, string.Join(", ", featureGateAttribute.Features));

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Feature not enabled.");
                        return;
                    }
                }
            }

            await next(context);
        }
    }
}