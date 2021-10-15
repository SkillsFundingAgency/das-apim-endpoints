using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnmetCourseDemands
{
    public class GetUnmetCourseDemandsQueryHandler : IRequestHandler<GetUnmetCourseDemandsQuery, GetUnmetCourseDemandsQueryResult>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _apiClient;

        public GetUnmetCourseDemandsQueryHandler (IEmployerDemandApiClient<EmployerDemandApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetUnmetCourseDemandsQueryResult> Handle(GetUnmetCourseDemandsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetUnmetCourseDemandsResponse>(
                    new GetUnmetEmployerDemandsRequest(request.AgeOfDemandInDays));

            return new GetUnmetCourseDemandsQueryResult
            {
                EmployerDemandIds = result.UnmetCourseDemands.Select(demand => demand.Id).ToList()
            };
        }
    }
}