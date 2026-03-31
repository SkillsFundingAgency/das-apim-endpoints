using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetCommitmentsProviders
{
    public class GetCommitmentsProvidersQueryHandler : IRequestHandler<GetCommitmentsProvidersQuery, GetCommitmentsProvidersResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetCommitmentsProvidersQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetCommitmentsProvidersResult> Handle(GetCommitmentsProvidersQuery request, CancellationToken cancellationToken)
        {
            var result = await _commitmentsV2ApiClient.GetWithResponseCode<GetProvidersResponse>(new GetProvidersRequest());

            var providers = result.Body.Providers;            

            return new GetCommitmentsProvidersResult
            {
                Providers = providers.ToList()
            };
        }
    }
}
