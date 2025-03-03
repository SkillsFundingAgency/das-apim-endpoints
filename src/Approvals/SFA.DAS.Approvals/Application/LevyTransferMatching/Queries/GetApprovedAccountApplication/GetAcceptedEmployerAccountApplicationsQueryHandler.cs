using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.LevyTransferMatching.Queries.GetApprovedAccountApplication
{
    public class GetAcceptedEmployerAccountApplicationsQueryHandler : IRequestHandler<GetAcceptedEmployerAccountApplicationsQuery, GetAcceptedEmployerAccountApplicationsQueryResult>
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _apiClient;

        public GetAcceptedEmployerAccountApplicationsQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetAcceptedEmployerAccountApplicationsQueryResult> Handle(GetAcceptedEmployerAccountApplicationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetApplicationsResponse>(new GetAcceptedEmployerAccountPledgeApplicationsRequest(request.EmployerAccountId));

            if (result == null) 
                return null;

            return new GetAcceptedEmployerAccountApplicationsQueryResult
            {
                Applications = result.Applications
            };
        }
    }
}