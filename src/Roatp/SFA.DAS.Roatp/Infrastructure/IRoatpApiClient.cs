using System.Threading.Tasks;
using RestEase;
using SFA.DAS.Roatp.Domain.Models;

namespace SFA.DAS.Roatp.Infrastructure;

public interface IRoatpApiClient
{
    [Get("Organisations")]
    [AllowAnyStatusCode]
    Task<GetOrganisationsQueryResult> GetOrganisations();

    [Put("Organisations/{ukprn}")]
    [AllowAnyStatusCode]
    Task PutOrganisation([Path] int ukprn, [Body] UpdateOrganisationModel model);
}
