using System;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using SFA.DAS.Roatp.Domain.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

namespace SFA.DAS.Roatp.Infrastructure;

public interface IRoatpApiClient : IHealthChecker
{
    [Get("Organisations")]
    [AllowAnyStatusCode]
    Task<GetOrganisationsResponse> GetOrganisations(CancellationToken cancellationToken);

    [Put("Organisations/{ukprn}")]
    [AllowAnyStatusCode]
    Task PutOrganisation([Path] int ukprn, [Body] UpdateOrganisationModel model, CancellationToken cancellationToken);

    [Get("ukrlp/providers")]
    Task<UkrlpProvidersResponse> GetProvidersDataFromUkrlp([Query] DateTime? updatedSince, [Query] int[] ukprns, CancellationToken cancellationToken);
}
