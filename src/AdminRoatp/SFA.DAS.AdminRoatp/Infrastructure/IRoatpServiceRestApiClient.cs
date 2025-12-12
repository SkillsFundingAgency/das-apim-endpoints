using Microsoft.AspNetCore.JsonPatch;
using RestEase;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;

namespace SFA.DAS.AdminRoatp.Infrastructure;

public interface IRoatpServiceRestApiClient
{
    [Patch("/organisations/{ukprn}")]
    Task PatchOrganisation([Path] int ukprn, [Header(Constants.RequestingUserIdHeader)] string userId, [Body] JsonPatchDocument<PatchOrganisationModel> patchDoc, CancellationToken cancellationToken);

    [Put("/organisations/{ukprn}/course-types")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> PutCourseTypes([Path] int ukprn, [Body] UpdateCourseTypesModel command, CancellationToken cancellationToken);

    [Post("/organisations")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> PostOrganisation([Body] PostOrganisationRequest request, CancellationToken cancellationToken);

    [Delete("/organisations/{ukprn}/short-courses")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> DeleteShortCourseTypes([Path] int ukprn, [Header(Constants.RequestingUserIdHeader)] string userId, CancellationToken cancellationToken);
}