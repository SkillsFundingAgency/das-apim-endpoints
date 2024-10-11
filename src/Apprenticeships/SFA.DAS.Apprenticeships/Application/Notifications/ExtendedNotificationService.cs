using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Apprenticeships.Application.Notifications
{
    public interface IExtendedNotificationService
    {
        Task<CommitmentsApprenticeshipDetails> GetApprenticeship(GetCurrentPartyIdsResponse currentPartyIds);
        Task<GetCurrentPartyIdsResponse> GetCurrentPartyIds(Guid apprenticeshipKey);
        Task<IEnumerable<Recipient>> GetEmployerRecipients(long accountId);
        Task<bool> Send(Recipient recipient, string templateId, Dictionary<string, string> tokens);
    }

    public class ExtendedNotificationService : IExtendedNotificationService
    {
        protected ILogger<ExtendedNotificationService> _logger;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;
        private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _apprenticeshipsApiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiCommitmentsClient;
        private readonly INotificationService _notificationService;

        public ExtendedNotificationService(
            ILogger<ExtendedNotificationService> logger,
            IAccountsApiClient<AccountsConfiguration> accountsApiClient,
            IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipsApiClient,
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiCommitmentsClient,
            INotificationService notificationService)
        {
            _logger = logger;
            _accountsApiClient = accountsApiClient;
            _apprenticeshipsApiClient = apprenticeshipsApiClient;
            _apiCommitmentsClient = apiCommitmentsClient;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Call to apprenticeship inner to get the apprenticeships current provider and employer ids
        /// </summary>
        public async Task<GetCurrentPartyIdsResponse> GetCurrentPartyIds(Guid apprenticeshipKey)
        {
            var ownerIds = await _apprenticeshipsApiClient.Get<GetCurrentPartyIdsResponse>(new GetCurrentPartyIdsRequest { ApprenticeshipKey = apprenticeshipKey });
            return ownerIds;
        }

        public async Task<IEnumerable<Recipient>> GetEmployerRecipients(long accountId)
        {
            var response = await _accountsApiClient.Get<GetAccountTeamMembersWhichReceiveNotificationsResponse>(new GetAccountTeamMembersWhichReceiveNotificationsRequest(accountId));

            if (response == null || !response.Any())
            {
                _logger.LogWarning($"No response returned from the shared outer api for account id {accountId}");
                return Enumerable.Empty<Recipient>();
            }

            return response.Where(x => x.CanReceiveNotifications).Select(x => new Recipient
            {
                UserRef = x.UserRef,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Name = x.Name,
                Email = x.Email,
                Role = x.Role
            });
        }

        public async Task<CommitmentsApprenticeshipDetails> GetApprenticeship(GetCurrentPartyIdsResponse currentPartyIds)
        {
            _logger.LogInformation("Getting apprenticeship details for apprenticeship id {apprenticeshipId}", currentPartyIds.ApprovalsApprenticeshipId);

            var innerApiRequest = new GetApprenticeshipRequest(currentPartyIds.ApprovalsApprenticeshipId);
            var innerApiResponse = await _apiCommitmentsClient.GetWithResponseCode<GetApprenticeshipResponse>(innerApiRequest);

            if (!ApiResponseErrorChecking.IsSuccessStatusCode(innerApiResponse.StatusCode))
            {
                throw new Exception($"Apprenticeship not found for id {currentPartyIds.ApprovalsApprenticeshipId}");
            }

            return new CommitmentsApprenticeshipDetails
            {
                ProviderName = innerApiResponse.Body.ProviderName,
                EmployerName = innerApiResponse.Body.EmployerName,
                ApprenticeFirstName = innerApiResponse.Body.FirstName,
                ApprenticeLastName = innerApiResponse.Body.LastName
            };
        }

        public async Task<bool> Send(Recipient recipient, string templateId, Dictionary<string,string> tokens)
        {
            try
            {
                var command = new SendEmailCommand(templateId, recipient.Email, tokens);
                await _notificationService.Send(command);
                _logger.LogInformation("Email sent to {userId}", recipient.UserRef);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {userId}", recipient.UserRef);
                return false;
            }

            return true;
        }
    }

    public class CommitmentsApprenticeshipDetails
    {
        public string ProviderName { get; set; } = string.Empty;
        public string EmployerName { get; set; } = string.Empty;
        public string ApprenticeFirstName { get; set; } = string.Empty;
        public string ApprenticeLastName { get; set; } = string.Empty;
    }
}
