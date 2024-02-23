using SFA.DAS.EarlyConnect.Configuration.FeatureToggle;

namespace SFA.DAS.EarlyConnect.Api.Extensions
{
    public static class FeatureExtensions
    {
        public static void AddFeatureToggle(this IServiceCollection services)
        {
            services.AddSingleton<IFeature, Feature>();
        }
    }
}
