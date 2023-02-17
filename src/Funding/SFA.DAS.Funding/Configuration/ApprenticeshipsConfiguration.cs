using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.Configuration
{
    public class ApprenticeshipsConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}