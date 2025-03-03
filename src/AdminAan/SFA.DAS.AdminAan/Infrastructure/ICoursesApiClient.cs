﻿using RestEase;
using SFA.DAS.AdminAan.Domain.Courses;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface ICoursesApiClient : IHealthChecker
{
    [Get("api/courses/Standards/{standardUId}")]
    [AllowAnyStatusCode]
    Task<Response<GetStandardResponse>> GetStandard([Path] string standardUId, CancellationToken cancellationToken);

    [Get("api/courses/Frameworks/{trainingCode}")]
    [AllowAnyStatusCode]
    Task<Response<GetFrameworkResponse>> GetFramework([Path] string trainingCode, CancellationToken cancellationToken);
}
