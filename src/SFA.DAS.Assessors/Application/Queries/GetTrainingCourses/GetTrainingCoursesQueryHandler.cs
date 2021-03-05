using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Assessors.Application.Queries.GetTrainingCourses
{
    public class GetTrainingCoursesQueryHandler : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetTrainingCoursesQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetTrainingCoursesResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var standardsList = await _coursesApiClient.Get<GetStandardsListResponse>(new GetAllStandardsListRequest());

            return new GetTrainingCoursesResult
            {
                TrainingCourses = standardsList.Standards
            };
        }
    }
}
