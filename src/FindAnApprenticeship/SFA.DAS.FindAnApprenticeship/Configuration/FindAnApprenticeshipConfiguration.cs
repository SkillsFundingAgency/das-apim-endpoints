using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Configuration
{
    public class FindAnApprenticeshipConfiguration
    {
        public double LocationsApiMinMatch { get; set; }
        public string ApimEndpointsRedisConnectionString { get; set; }
    }
}