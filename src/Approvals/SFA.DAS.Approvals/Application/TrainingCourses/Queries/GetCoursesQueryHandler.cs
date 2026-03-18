using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;
public class GetCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IRequestHandler<GetCoursesQuery, GetCoursesResult>
{
    public async Task<GetCoursesResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var response = await coursesApiClient.Get<GetCoursesListResponse>(new GetCoursesExportRequest());        

        return new GetCoursesResult
        {
            Courses = response.Courses
        };
    }
}