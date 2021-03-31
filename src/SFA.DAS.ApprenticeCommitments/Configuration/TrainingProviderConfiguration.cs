using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Configuration
{
    public class TrainingProviderConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}