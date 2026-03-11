using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetCommitmentsApprenticeshipById
{
    public class GetCommitmentsApprenticeshipByIdQueryHandler : IRequestHandler<GetCommitmentsApprenticeshipByIdQuery, GetApprenticeshipResponse>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;

        public GetCommitmentsApprenticeshipByIdQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<GetApprenticeshipResponse> Handle(GetCommitmentsApprenticeshipByIdQuery request, CancellationToken cancellationToken)
        {
            var apprenticeship = await _commitmentsApiClient.Get<GetApprenticeshipResponse>
                (new GetCommitmentsApprenticeshipByIdRequest(request.ApprenticeshipId));

            return apprenticeship;
        }

    }
}
