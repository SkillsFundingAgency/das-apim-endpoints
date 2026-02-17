using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetApprenticeshipIdsByCommitmentId
{
    public class GetApprenticeshipIdsByCommitmentIdQueryHandler : IRequestHandler<GetApprenticeshipIdsByCommitmentIdQuery, GetApprenticeshipIdsByCommitmentIdQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;

        public GetApprenticeshipIdsByCommitmentIdQueryHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<GetApprenticeshipIdsByCommitmentIdQueryResult> Handle(
            GetApprenticeshipIdsByCommitmentIdQuery request,
            CancellationToken cancellationToken)
        {
            var apprenticeshipIds = await _commitmentsApiClient.Get<GetApprenticeshipIdsByCommitmentIdQueryResult>
                (new GetApprenticeshipIdsByCommitmentIdRequest(request.CommitmentId));            

            return apprenticeshipIds;
        }
    }    
}
