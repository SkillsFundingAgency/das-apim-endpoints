using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Configuration
{
    public class EmployerAccountsConfiguration : IInternalApiConfiguration
    {
        public string Identifier { get; set; }
        public string Url { get; set; }
    }
}