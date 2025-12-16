using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerFeedback.Configuration;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Commands.UpsertFeedbackTransaction
{
    public class UpsertFeedbackTransactionCommandHandler : IRequestHandler<UpsertFeedbackTransactionCommand>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly ILogger<UpsertFeedbackTransactionCommandHandler> _logger;
        private readonly EmployerFeedbackConfiguration _settings;

        public UpsertFeedbackTransactionCommandHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            ILogger<UpsertFeedbackTransactionCommandHandler> logger,
            IOptions<EmployerFeedbackConfiguration> settings)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task Handle(UpsertFeedbackTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var accountProvidersCourseStatusResponse = await _commitmentsApiClient.GetWithResponseCode<GetAccountProvidersCourseStatusResponse>(
                        new GetAccountProvidersCourseStatusRequest(
                        request.AccountId,
                        _settings.AccountProvidersCourseStatusCompletionLag,
                        _settings.AccountProvidersCourseStatusStartLag,
                        _settings.AccountProvidersCourseStatusNewStartWindow));

                accountProvidersCourseStatusResponse.EnsureSuccessStatusCode();

                var responseBody = accountProvidersCourseStatusResponse.Body;
                if (responseBody == null)
                {
                    var message = $"No account providers course status data returned for account {request.AccountId}";
                    _logger.LogError(message);
                    throw new InvalidOperationException(message);
                }

                var feedbackTransactionRequest = new UpsertFeedbackTransactionRequest(
                    request.AccountId,
                    new UpsertFeedbackTransactionData
                    {
                        Active = responseBody.Active?.ConvertAll(x => new ApprenticeshipStatusItem { Ukprn = x.Ukprn, CourseCode = x.CourseCode }) ?? new(),
                        Completed = responseBody.Completed?.ConvertAll(x => new ApprenticeshipStatusItem { Ukprn = x.Ukprn, CourseCode = x.CourseCode }) ?? new(),
                        NewStart = responseBody.NewStart?.ConvertAll(x => new ApprenticeshipStatusItem { Ukprn = x.Ukprn, CourseCode = x.CourseCode }) ?? new()
                    });

                var feedbackResponse = await _employerFeedbackApiClient.PostWithResponseCode<UpsertFeedbackTransactionData, object>(
                    feedbackTransactionRequest, false);

                feedbackResponse.EnsureSuccessStatusCode();

                _logger.LogInformation("Successfully upserted feedback transaction for account {AccountId}", request.AccountId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error upserting feedback transaction for account {AccountId}", request.AccountId);
                throw;
            }
        }
    }
}