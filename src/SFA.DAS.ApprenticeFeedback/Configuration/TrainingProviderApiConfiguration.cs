using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.Configuration
{
    public class TrainingProviderApiConfiguration : IInternalApiConfiguration
    {
        public string Tenant { get; set; }
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}