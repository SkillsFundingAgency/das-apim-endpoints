using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System.Net;
using SFA.DAS.ApprenticePortal.InnerApi.CommitmentsV2.Requests;
using SFA.DAS.ApprenticePortal.Services;
using SFA.DAS.ApprenticePortal.InnerApi.CommitmentsV2.Responses;
using SFA.DAS.ApprenticePortal.InnerApi.ProviderAccounts.Responses;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Commands
{
    public class AddOrUpdateMyApprenticeshipCommandHandler : IRequestHandler<AddOrUpdateMyApprenticeshipCommand>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;
        private readonly TrainingProviderService _trainingProviderService;
        private readonly ILogger<AddOrUpdateMyApprenticeshipCommandHandler> _logger;

        public AddOrUpdateMyApprenticeshipCommandHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient,
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
            TrainingProviderService trainingProviderService,
            ILogger<AddOrUpdateMyApprenticeshipCommandHandler> logger)
        {
            _accountsApiClient = accountsApiClient;
            _commitmentsApiClient = commitmentsApiClient;
            _trainingProviderService = trainingProviderService;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddOrUpdateMyApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "AddOrUpdateMyApprenticeshipCommand Apprentice:{ApprenticeId} CommitmentsApprenticeshipId : {CommitmentsApprenticeshipId}",
                request.ApprenticeId, request.CommitmentsApprenticeshipId);

            var commitmentsApprenticeship = await GetCommitmentsApprenticeshipDetails(request.CommitmentsApprenticeshipId);
            var myNewApprenticeship = await BuildMyApprenticeship(commitmentsApprenticeship);
            
            // Internally the Inner API will either create a new MyApprenticeship or update the existing one
            await _accountsApiClient.Post(new PostApprenticeshipRequest(request.ApprenticeId) { Data = myNewApprenticeship });

            return Unit.Value;
        }

        private async Task<MyApprenticeshipData> BuildMyApprenticeship(ApprenticeshipDetailsResponse commitmentsApprenticeship)
        {
            var provider = await _trainingProviderService.GetTrainingProviderDetails(commitmentsApprenticeship.ProviderId);
            return new MyApprenticeshipData
            {
                ApprenticeshipId = commitmentsApprenticeship.Id,
                Uln = commitmentsApprenticeship.Uln,
                EmployerName = commitmentsApprenticeship.EmployerName,
                StartDate = commitmentsApprenticeship.StartDate,
                EndDate = commitmentsApprenticeship.EndDate,
                TrainingCode = commitmentsApprenticeship.CourseCode,
                StandardUId = commitmentsApprenticeship.StandardUId,
                TrainingProviderId = commitmentsApprenticeship.ProviderId,
                TrainingProviderName = ProviderName(provider)
            };
        }

        private static string ProviderName(TrainingProviderResponse trainingProvider)
            => string.IsNullOrWhiteSpace(trainingProvider.TradingName)
                ? trainingProvider.LegalName
                : trainingProvider.TradingName;

        public async Task<ApprenticeshipDetailsResponse> GetCommitmentsApprenticeshipDetails(long apprenticeshipId)
        {
            var response = await _commitmentsApiClient.GetWithResponseCode<ApprenticeshipDetailsResponse>(new GetApprenticeshipDetailsRequest(apprenticeshipId));

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestContentException(response.ErrorContent, response.StatusCode);

            return response.Body;
        }
    }
}
