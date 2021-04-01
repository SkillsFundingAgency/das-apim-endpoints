using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetAggregatedCourseDemandList
{
    public class GetAggregatedCourseDemandListQueryHandler : IRequestHandler<GetAggregatedCourseDemandListQuery, GetAggregatedCourseDemandListResult> 
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _demandApiClient;

        public GetAggregatedCourseDemandListQueryHandler(
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> demandApiClient)
        {
            _coursesApiClient = coursesApiClient;
            _demandApiClient = demandApiClient;
        }

        public async Task<GetAggregatedCourseDemandListResult> Handle(GetAggregatedCourseDemandListQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetAllStandardsListRequest());
            var aggregatedDemands = await _demandApiClient.Get<GetAggregatedCourseDemandListResponse>(new GetAggregatedCourseDemandListRequest(request.Ukprn, request.CourseId));


            return new GetAggregatedCourseDemandListResult
            {
                Courses = courses.Courses,
                AggregatedCourseDemands = aggregatedDemands.AggregatedCourseDemandList,
                Total = aggregatedDemands.Total,
                TotalFiltered = aggregatedDemands.TotalFiltered
            };
        }
    }
}
