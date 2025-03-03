using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.CommitmentsV2.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipDetails
{
    public class GetApprenticeshipQueryHandler : IRequestHandler<GetApprenticeshipQuery, GetApprenticeshipQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        
        public GetApprenticeshipQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetApprenticeshipQueryResult> Handle(GetApprenticeshipQuery request, CancellationToken cancellationToken)
        {
            var task = await _commitmentsV2ApiClient.Get<GetApprenticeshipQueryResult>(new GetApprenticeshipDetailsRequest(request.ApprenticeshipId));

            return new GetApprenticeshipQueryResult
            {
                Option = task.Option
            };
        }
    }
}