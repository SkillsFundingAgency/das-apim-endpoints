using MediatR;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual
{
    public class GetProviderFeedbackAnnualQueryHandler : IRequestHandler<GetProviderFeedbackAnnualQuery, GetProviderFeedbackAnnualResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;

        public GetProviderFeedbackAnnualQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
        }

        public async Task<GetProviderFeedbackAnnualResult> Handle(GetProviderFeedbackAnnualQuery request, CancellationToken cancellationToken)
        {
            var apprenticeFeedbackTask = _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackAnnualResponse>(new GetApprenticeFeedbackAnnualRequest(request.ProviderId));
            var employerFeedbackTask = _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackAnnualResponse>(new GetEmployerFeedbackAnnualRequest(request.ProviderId));

            await Task.WhenAll(apprenticeFeedbackTask, employerFeedbackTask);

            await Task.WhenAll(apprenticeFeedbackTask);

            GetProviderStandardAnnualItem providerDetails = new GetProviderStandardAnnualItem();

            providerDetails.Ukprn = request.ProviderId;

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