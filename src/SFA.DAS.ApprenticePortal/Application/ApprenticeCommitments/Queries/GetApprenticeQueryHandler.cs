using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeCommitments.Queries
{
    public class GetApprenticeQueryHandler : IRequestHandler<GetApprenticeQuery, GetApprenticeQueryResult>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _apiClient;

        public GetApprenticeQueryHandler(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apiClient)
            => _apiClient = apiClient;

        public async Task<GetApprenticeQueryResult> Handle(GetApprenticeQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<Apprentice>(new GetApprenticeRequest(request.ApprenticeId));

            return new GetApprenticeQueryResult { apprentice = result };
        }
    }
}
