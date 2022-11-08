using MediatR;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeFeedbackTargets
{
    public class GetApprenticeFeedbackTargetsQueryHandler : IRequestHandler<GetApprenticeFeedbackTargetsQuery, GetApprenticeFeedbackTargetsResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;

        public GetApprenticeFeedbackTargetsQueryHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
        }

        public async Task<GetApprenticeFeedbackTargetsResult> Handle(GetApprenticeFeedbackTargetsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeFeedbackTargets = await _apprenticeFeedbackApiClient.
                GetAll<ApprenticeFeedbackTarget>(new GetAllApprenticeFeedbackTargetsRequest { ApprenticeId = request.ApprenticeId });

            return new GetApprenticeFeedbackTargetsResult
            {
                FeedbackTargets = apprenticeFeedbackTargets
            };
        }
    }
}
