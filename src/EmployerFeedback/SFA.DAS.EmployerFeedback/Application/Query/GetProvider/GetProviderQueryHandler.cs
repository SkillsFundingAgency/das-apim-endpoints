using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetProviderQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }
        public async Task<GetProviderQueryResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {

            var response = await _commitmentsV2ApiClient.Get<GetProviderQueryResult>(new GetProviderRequest(request.ProviderId));
            return response;
        }
    }
}