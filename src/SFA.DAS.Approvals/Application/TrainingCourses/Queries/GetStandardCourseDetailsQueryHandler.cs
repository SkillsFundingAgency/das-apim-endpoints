﻿using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries
{
    public class GetStandardCourseDetailsQueryHandler : IRequestHandler<GetStandardCourseDetailsQuery, GetStandardsListItem>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetStandardCourseDetailsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetStandardsListItem> Handle(GetStandardCourseDetailsQuery request, CancellationToken cancellationToken)
        {
            var course = await _coursesApiClient.Get<GetStandardsListItem>(
                new GetStandardDetailsByIdRequest(request.CourseCode));

            return course;
        }
    }
}
