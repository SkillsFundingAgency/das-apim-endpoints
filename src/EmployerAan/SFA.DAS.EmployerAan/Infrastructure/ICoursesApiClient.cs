using RestEase;
using SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

namespace SFA.DAS.EmployerAan.Infrastructure;

public interface ICoursesApiClient : IHealthChecker
{
    [Get("api/courses/Standards/{standardUid}")]
    [AllowAnyStatusCode]
    Task<Response<GetStandardResponse>> GetStandard([Path] string standardUid, CancellationToken cancellationToken);

    [Get("api/courses/Frameworks/{trainingCode}")]
    [AllowAnyStatusCode]
    Task<Response<GetFrameworkResponse>> GetFramework([Path] string trainingCode, CancellationToken cancellationToken);
}