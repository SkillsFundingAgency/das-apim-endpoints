using Microsoft.AspNetCore.JsonPatch;
using RestEase;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.Infrastructure;

public interface IRoatpServiceRestApiClient
{
    [Patch("/organisations/{ukprn}")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> PatchOrganisation([Path] int ukprn, [Header(Constants.RequestingUserIdHeader)] string userId, [Body] JsonPatchDocument<PatchOrganisationModel> patchDoc, CancellationToken cancellationToken);
}
