using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Strategies
{
    public interface IOrganisationApiStrategy
    {
        Task<GetLatestDetailsResult> GetOrganisationDetails(string identifier, OrganisationType orgType);
    }
}
