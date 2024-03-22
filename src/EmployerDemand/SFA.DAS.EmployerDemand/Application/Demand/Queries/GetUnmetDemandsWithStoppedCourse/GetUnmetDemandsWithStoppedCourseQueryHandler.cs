using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnmetDemandsWithStoppedCourse
{
    public class GetUnmetDemandsWithStoppedCourseQueryHandler : IRequestHandler<GetUnmetDemandsWithStoppedCourseQuery, GetUnmetDemandsWithStoppedCourseResult> 
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _employerDemandApiClient;

        public GetUnmetDemandsWithStoppedCourseQueryHandler(
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> employerDemandApiClient)
        {
            _coursesApiClient = coursesApiClient;
            _employerDemandApiClient = employerDemandApiClient;
        }

        public async Task<GetUnmetDemandsWithStoppedCourseResult> Handle(GetUnmetDemandsWithStoppedCourseQuery request, CancellationToken cancellationToken)
        {
            var stoppedCoursesTask = _coursesApiClient.Get<GetStandardsListResponse>(new GetStandardsClosedToNewStartsRequest());
            var unmetDemandsResponseTask = _employerDemandApiClient.Get<GetUnmetCourseDemandsResponse>(new GetUnmetEmployerDemandsRequest(0));
            await Task.WhenAll(stoppedCoursesTask, unmetDemandsResponseTask);

            var stoppedCourseIds = stoppedCoursesTask.Result.Standards
                .Select(item => item.LarsCode);
            var unmetEmployerDemandIds = unmetDemandsResponseTask.Result.UnmetCourseDemands
                .Where(demand => stoppedCourseIds.Contains(demand.CourseId))
                .Select(demand => demand.Id);

            return new GetUnmetDemandsWithStoppedCourseResult
            {
                EmployerDemandIds = unmetEmployerDemandIds
            };
        }
    }
}