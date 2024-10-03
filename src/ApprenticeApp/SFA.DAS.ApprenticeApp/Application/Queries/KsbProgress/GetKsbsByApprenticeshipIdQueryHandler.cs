using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbsByApprenticeshipIdQueryHandler : IRequestHandler<GetKsbsByApprenticeshipIdQuery, GetKsbsByApprenticeshipIdQueryResult>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApiClient;

        public GetKsbsByApprenticeshipIdQueryHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApiClient)
        {
            _progressApiClient = progressApiClient;
        }

        public async Task<GetKsbsByApprenticeshipIdQueryResult> Handle(GetKsbsByApprenticeshipIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _progressApiClient.Get<GetKsbsByApprenticeshipIdQueryResult>(new GetKsbsByApprenticeshipIdQueryRequest(request.ApprenticeshipId));

            return new GetKsbsByApprenticeshipIdQueryResult
            {
                KSBProgresses = task.KSBProgresses
            };
        }
    }
}
