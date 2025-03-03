using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Configuration
{
    public interface ICompaniesHouseApiConfiguration : IApiConfiguration
    {
        string ApiKey { get; set; }
    }
}