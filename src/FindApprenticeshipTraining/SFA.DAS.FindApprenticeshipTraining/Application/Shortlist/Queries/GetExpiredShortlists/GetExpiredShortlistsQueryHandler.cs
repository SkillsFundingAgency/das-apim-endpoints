using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetExpiredShortlists
{
    public class GetExpiredShortlistsQueryHandler : IRequestHandler<GetExpiredShortlistsQuery, GetExpiredShortlistsQueryResult>
    {
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;

        public GetExpiredShortlistsQueryHandler (IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient)
        {
            _shortlistApiClient = shortlistApiClient;
        }
        public async Task<GetExpiredShortlistsQueryResult> Handle(GetExpiredShortlistsQuery request, CancellationToken cancellationToken)
        {
            var apiResult =
                await _shortlistApiClient.Get<GetExpiredShortlistsResponse>(
                    new GetExpiredShortlistsRequest(request.ExpiryInDays));

            return new GetExpiredShortlistsQueryResult
            {
                UserIds = apiResult.UserIds
            };
        }
    }
}