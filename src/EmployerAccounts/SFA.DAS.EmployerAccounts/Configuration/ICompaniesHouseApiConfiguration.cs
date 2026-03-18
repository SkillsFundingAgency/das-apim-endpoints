using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerAccounts.Configuration
{
    public interface ICompaniesHouseApiConfiguration : IApiConfiguration
    {
        string ApiKey { get; set; }
    }
}