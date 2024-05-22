using MediatR;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback
{
    public class GetProviderFeedbackQueryHandler : IRequestHandler<GetProviderFeedbackQuery, GetProviderFeedbackResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;

        public GetProviderFeedbackQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
        }

        public async Task<GetProviderFeedbackResult> Handle(GetProviderFeedbackQuery request, CancellationToken cancellationToken)
        {
            var apprenticeFeedbackTask = _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackResponse>(new GetApprenticeFeedbackRequest(request.ProviderId));
            var employerFeedbackTask = _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackResponse>(new GetEmployerFeedbackRequest(request.ProviderId));


            await Task.WhenAll(apprenticeFeedbackTask, employerFeedbackTask);

            GetProviderStandardItem providerDetails = new GetProviderStandardItem();

            providerDetails.Ukprn = request.ProviderId;

            if (apprenticeFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && apprenticeFeedbackTask.Result.Body != null)
            {
                providerDetails.ApprenticeFeedback = apprenticeFeedbackTask.Result.Body;
            }

            if (employerFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && employerFeedbackTask.Result.Body != null)
            {
                providerDetails.EmployerFeedback = employerFeedbackTask.Result.Body;
            }


            return new GetProviderFeedbackResult
            {
                ProviderStandard = providerDetails,
            };
        }
    }
}