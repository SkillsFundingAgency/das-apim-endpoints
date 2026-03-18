using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerIncentives.Configuration
{
    public class CommitmentsConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}