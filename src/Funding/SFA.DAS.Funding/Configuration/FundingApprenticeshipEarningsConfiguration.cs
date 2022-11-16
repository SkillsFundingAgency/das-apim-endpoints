using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.Configuration
{
    public class FundingApprenticeshipEarningsConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}