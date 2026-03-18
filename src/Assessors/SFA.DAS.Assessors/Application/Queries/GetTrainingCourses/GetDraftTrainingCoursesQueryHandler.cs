using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.Assessors.Application.Queries.GetTrainingCourses
{
    public class GetDraftTrainingCoursesQueryHandler : IRequestHandler<GetDraftTrainingCoursesQuery, GetTrainingCoursesResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetDraftTrainingCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetTrainingCoursesResult> Handle(GetDraftTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetNotYetApprovedStandardsRequest());

            return new GetTrainingCoursesResult
            {
                TrainingCourses = courses.Standards
            };
        }
    }
}