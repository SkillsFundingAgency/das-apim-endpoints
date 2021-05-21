using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCourseDemand
{
    public class GetCourseDemandQueryHandler : IRequestHandler<GetCourseDemandQuery, GetCourseDemandQueryResult>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _apiClient;

        public GetCourseDemandQueryHandler (IEmployerDemandApiClient<EmployerDemandApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetCourseDemandQueryResult> Handle(GetCourseDemandQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetEmployerDemandResponse>(new GetEmployerDemandRequest(request.Id));

            return new GetCourseDemandQueryResult
            {
                EmployerDemand = result
            };
        }
    }
}