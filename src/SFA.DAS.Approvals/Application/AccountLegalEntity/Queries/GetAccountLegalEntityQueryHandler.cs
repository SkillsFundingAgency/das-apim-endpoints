using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.AccountLegalEntity
{
    public class GetAccountLegalEntityQueryHandler : IRequestHandler<GetAccountLegalEntityQuery, GetAccountLegalEntityQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetAccountLegalEntityQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async  Task<GetAccountLegalEntityQueryResult> Handle(GetAccountLegalEntityQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(request.AccountLegalEntityId));

            if (result == null)
                return null;

            return (GetAccountLegalEntityQueryResult)result;
        }
    }
}
