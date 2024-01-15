using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Configuration
{
    public class FindAnApprenticeshipConfiguration
    {
        public double LocationsApiMinMatch { get; set; }
        public string ApimEndpointsRedisConnectionString { get; set; }
        public CandidateApiConfiguration CandidateApi { get; set; }
    }

    public class CandidateApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}