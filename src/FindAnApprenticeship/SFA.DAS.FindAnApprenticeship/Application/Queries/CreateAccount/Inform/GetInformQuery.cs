using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.Inform
{
    public class GetInformQuery : IRequest<GetInformQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetInformQueryResult
    {
        public bool ShowAccountRecoveryBanner { get; set; }
    }

    public class GetInformQueryHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient) : IRequestHandler<GetInformQuery, GetInformQueryResult>
    {
        public async Task<GetInformQueryResult> Handle(GetInformQuery request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await apiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(request.CandidateId.ToString()));

            return new GetInformQueryResult
            {
                ShowAccountRecoveryBanner = string.IsNullOrWhiteSpace(apiResponse.MigratedEmail)
            };
        }
    }
}
