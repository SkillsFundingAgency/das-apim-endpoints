using RestEase;
using SFA.DAS.AdminAan.Domain.Courses;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface ICoursesApiClient
{
    [Get("api/courses/Standards/{StandardUId}")]
    [AllowAnyStatusCode]
    Task<Response<GetStandardResponse>> GetStandard([Path] string standardUId, CancellationToken cancellationToken);

    [Get("api/courses/Frameworks/{TrainingCode}")]
    [AllowAnyStatusCode]
    Task<Response<GetFrameworkResponse>> GetFramework([Path] string trainingCode, CancellationToken cancellationToken);
}
