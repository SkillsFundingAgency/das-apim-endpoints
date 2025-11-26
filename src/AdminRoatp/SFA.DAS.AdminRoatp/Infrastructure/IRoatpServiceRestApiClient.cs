using Microsoft.AspNetCore.JsonPatch;
using RestEase;
using SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.Infrastructure;

public interface IRoatpServiceRestApiClient
{
    [Patch("/organisations/{ukprn}")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> PatchOrganisation([Path] int ukprn, [Header(Constants.RequestingUserIdHeader)] string userId, [Body] JsonPatchDocument<PatchOrganisationModel> patchDoc, CancellationToken cancellationToken);

    [Put("/organisations/{ukprn}/course-types")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> PutCourseTypes([Path] int ukprn, [Body] PutCourseTypesModel command, CancellationToken cancellationToken);

    [Post("/organisations")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> PostOrganisation([Body] PostOrganisationModel model, CancellationToken cancellationToken);

    [Delete("/organisations/{ukprn}/short-courses")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> DeleteShortCourseTypes([Path] int ukprn, [Header(Constants.RequestingUserIdHeader)] string userId, CancellationToken cancellationToken);
}