using MediatR;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackForAcademicYear
{
    public class GetProviderFeedbackForAcademicYearQueryHandler : IRequestHandler<GetProviderFeedbackForAcademicYearQuery, GetProviderFeedbackForAcademicYearResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;

        public GetProviderFeedbackForAcademicYearQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
        }

        public async Task<GetProviderFeedbackForAcademicYearResult> Handle(GetProviderFeedbackForAcademicYearQuery request, CancellationToken cancellationToken)
        {
            var apprenticeFeedbackTask = _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackForAcademicYearResponse>(new GetApprenticeFeedbackForAcademicYearRequest(request.ProviderId, request.Year));
            var employerFeedbackTask = _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackForAcademicYearResponse>(new GetEmployerFeedbackForAcademicYearRequest(request.ProviderId, request.Year));


            await Task.WhenAll(apprenticeFeedbackTask, employerFeedbackTask);

            GetProviderStandardForAcademicYearItem providerDetails = new GetProviderStandardForAcademicYearItem();

            providerDetails.Ukprn = request.ProviderId;

            if (apprenticeFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && apprenticeFeedbackTask.Result.Body != null)
            {
                providerDetails.ApprenticeFeedback = apprenticeFeedbackTask.Result.Body;
            }

            if (employerFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && employerFeedbackTask.Result.Body != null)
            {
                providerDetails.EmployerFeedback = employerFeedbackTask.Result.Body;
            }


            return new GetProviderFeedbackForAcademicYearResult
            {
                ProviderStandard = providerDetails,
            };
        }
    }
}