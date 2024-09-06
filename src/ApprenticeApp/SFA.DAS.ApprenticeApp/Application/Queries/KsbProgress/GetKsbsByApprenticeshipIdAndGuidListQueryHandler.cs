using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbsByApprenticeshipIdAndGuidListQueryHandler : IRequestHandler<GetKsbsByApprenticeshipIdAndGuidListQuery, GetKsbsByApprenticeshipIdAndGuidListQueryResult>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApiClient;
        public GetKsbsByApprenticeshipIdAndGuidListQueryHandler(
         IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApiClient)
            {
                _progressApiClient = progressApiClient;
            }
        public async Task<GetKsbsByApprenticeshipIdAndGuidListQueryResult> Handle(GetKsbsByApprenticeshipIdAndGuidListQuery request, CancellationToken cancellationToken)
        {
            var task = await _progressApiClient.Get<GetKsbsByApprenticeshipIdAndGuidListQueryResult>(new GetKsbsByApprenticeshipIdAndGuidListRequest(request.ApprenticeshipId, request.Guids));

            return new GetKsbsByApprenticeshipIdAndGuidListQueryResult
            {
                KSBProgresses = task.KSBProgresses
            };
        }

    }
}
