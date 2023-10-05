using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsQueryHandler : IRequestHandler<SearchApprenticeshipsQuery, SearchApprenticeshipsResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;

        public SearchApprenticeshipsQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
        }
        
        public async Task<SearchApprenticeshipsResult> Handle(SearchApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var result = await _findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
                    new GetApprenticeshipCountRequest());

            return new SearchApprenticeshipsResult
            {
                TotalApprenticeshipCount = result.TotalVacancies
            };
        }
    }
}