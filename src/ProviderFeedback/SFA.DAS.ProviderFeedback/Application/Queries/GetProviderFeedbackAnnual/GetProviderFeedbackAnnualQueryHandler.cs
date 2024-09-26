using MediatR;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;
using SFA.DAS.ProviderFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService.TrainingProviderResponse;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual
{
    public class GetProviderFeedbackAnnualQueryHandler : IRequestHandler<GetProviderFeedbackAnnualQuery, GetProviderFeedbackAnnualResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly IProviderService _providerService;

        public GetProviderFeedbackAnnualQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            IProviderService providerService)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _providerService = providerService;
        }

        public async Task<GetProviderFeedbackAnnualResult> Handle(GetProviderFeedbackAnnualQuery request, CancellationToken cancellationToken)
        {
            var providerStandardsData = await _providerService.GetTrainingProviderDetails(request.ProviderId);
            bool isEmployerProvider = providerStandardsData.ProviderType.Id == (short)ProviderTypeIdentifier.EmployerProvider;

            var apprenticeFeedbackTask = _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackAnnualResponse>(
                new GetApprenticeFeedbackAnnualRequest(request.ProviderId));

            var employerFeedbackTask = isEmployerProvider
                ? Task.FromResult<ApiResponse<GetEmployerFeedbackAnnualResponse>>(null)
                : _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackAnnualResponse>(
                    new GetEmployerFeedbackAnnualRequest(request.ProviderId));

            await Task.WhenAll(apprenticeFeedbackTask, employerFeedbackTask);

            GetProviderStandardAnnualItem providerDetails = new GetProviderStandardAnnualItem();

            providerDetails.Ukprn = request.ProviderId;
            providerDetails.IsEmployerProvider = isEmployerProvider;

            if (apprenticeFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && apprenticeFeedbackTask.Result.Body != null)
            {
                providerDetails.ApprenticeFeedback = apprenticeFeedbackTask.Result.Body;
            }

            if (employerFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && employerFeedbackTask.Result.Body != null)
            {
                providerDetails.EmployerFeedback = employerFeedbackTask.Result.Body;
            }

            return new GetProviderFeedbackAnnualResult
            {
                ProviderStandard = providerDetails,
            };
        }
    }
}