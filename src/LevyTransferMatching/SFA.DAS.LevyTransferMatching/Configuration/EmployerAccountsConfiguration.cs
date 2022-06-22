using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Configuration
{
    public class EmployerAccountsConfiguration : IInternalApiConfiguration
    {
        public string Identifier { get; set; }
        public string Url { get; set; }
    }
}