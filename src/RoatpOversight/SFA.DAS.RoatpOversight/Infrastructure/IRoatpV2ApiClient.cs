using RestEase;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;
using SFA.DAS.RoatpOversight.InnerApi.Models;

namespace SFA.DAS.RoatpOversight.Infrastructure;

public interface IRoatpV2ApiClient : IHealthChecker
{
    [Post("Providers?userId={userId}&userDisplayName={userDisplayName}")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> CreateProvider([Path] string userId, [Path] string userDisplayName, [Body] CreateProviderCommand command, CancellationToken cancellationToken);

    [Get("providers/{ukprn}")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetProvider([Path] int ukprn);

    [Post("providers/{ukprn}/course-types")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> AddCourseTypes([Path] int ukprn, [Body] AddCourseTypesModel command, CancellationToken cancellationToken);
}