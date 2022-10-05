using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.ApprenticeFeedback.Models;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetExitSurvey
{
    public class GetExitSurveyForApprenticeFeedbackTargetQueryHandler : IRequestHandler<GetExitSurveyForApprenticeFeedbackTargetQuery, GetExitSurveyForApprenticeFeedbackTargetResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;

        public GetExitSurveyForApprenticeFeedbackTargetQueryHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
        }

        public async Task<GetExitSurveyForApprenticeFeedbackTargetResult> Handle(GetExitSurveyForApprenticeFeedbackTargetQuery request, CancellationToken cancellationToken)
        {
            var apiResponse = await _apprenticeFeedbackApiClient.Get<ApprenticeExitSurvey>(new GetExitSurveyRequest(request.ApprenticeFeedbackTargetId));

            return new GetExitSurveyForApprenticeFeedbackTargetResult
            {
                ExitSurvey = apiResponse
            };

        }
    }
}
