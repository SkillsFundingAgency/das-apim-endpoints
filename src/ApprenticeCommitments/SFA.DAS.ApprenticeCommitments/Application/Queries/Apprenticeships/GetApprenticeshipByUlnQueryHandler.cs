using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Apprenticeships
{
    public class GetApprenticeshipByUlnQueryHandler : IRequestHandler<GetApprenticeshipByUlnQuery, GetApprenticeshipByUlnQueryResponse>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;

        public GetApprenticeshipByUlnQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient)
            => _apprenticeAccountsApiClient = apprenticeAccountsApiClient;

        public async Task<GetApprenticeshipByUlnQueryResponse> Handle(GetApprenticeshipByUlnQuery request, CancellationToken cancellationToken)
        {
            var response = await _apprenticeAccountsApiClient.GetWithResponseCode<ApprenticeshipBase>(
                new GetApprenticeshipByUlnRequest(request.Uln));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }           

            return new GetApprenticeshipByUlnQueryResponse 
            { 
                MyApprenticeship = response.Body
            };
        }
    }
}
