using RestEase;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Responses;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;

public interface ICoursesApiClient : IHealthChecker
{
    [Get("api/courses/Standards/{standardUid}")]
    [AllowAnyStatusCode]
    Task<Response<GetStandardResponse>> GetStandard([Path] string standardUid, CancellationToken cancellationToken);


    [Get("api/courses/Frameworks/{trainingCode}")]
    [AllowAnyStatusCode]
    Task<Response<GetFrameworkResponse>> GetFramework([Path] string trainingCode, CancellationToken cancellationToken);

    [Get("/ping")]
    Task GetPing(CancellationToken cancellationToken);
}