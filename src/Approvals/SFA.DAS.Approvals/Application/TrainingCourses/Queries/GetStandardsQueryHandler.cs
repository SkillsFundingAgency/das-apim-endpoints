using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries
{
    public class GetStandardsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        : IRequestHandler<GetStandardsQuery, GetStandardsResult>
    {
        public async Task<GetStandardsResult> Handle(GetStandardsQuery request, CancellationToken cancellationToken)
        {
            var standards = await coursesApiClient.Get<GetStandardsListResponse>(new GetStandardsExportRequest());
            
            return new GetStandardsResult
            {
                Standards = standards.Standards
            };
        }
    }
}