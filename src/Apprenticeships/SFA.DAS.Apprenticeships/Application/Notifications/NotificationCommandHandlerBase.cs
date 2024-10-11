using MediatR;
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
    public abstract class NotificationCommandBase : IRequest<NotificationResponse>
    {
        public string? Initiator { get; set; }
        public Guid ApprenticeshipKey { get; set; }
    }

    public abstract class NotificationCommandHandlerBase<T> : IRequestHandler<T, NotificationResponse> where T : NotificationCommandBase
    {
        private readonly IExtendedNotificationService _notificationService;
        
        protected NotificationCommandHandlerBase(IExtendedNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public abstract Task<NotificationResponse> Handle(T request, CancellationToken cancellationToken);

        protected async Task<NotificationResponse> SendToEmployer(T request, string templateId, Func<Recipient, CommitmentsApprenticeshipDetails, Dictionary<string, string>> getTokensFunction)
        {
            var notificationResponse = new NotificationResponse { Success = true };
            var currentParties = await _notificationService.GetCurrentPartyIds(request.ApprenticeshipKey);
            var recepients = await _notificationService.GetEmployerRecipients(currentParties.EmployerAccountId);
            var apprenticeship = await _notificationService.GetApprenticeship(currentParties);

            foreach (var recipient in recepients)
            {
                var tokens = getTokensFunction(recipient, apprenticeship);
                await _notificationService.Send(recipient, templateId, tokens);
            }

            return notificationResponse;
        }

        protected Task<NotificationResponse> SendToProvider(T request)
        {
            throw new NotImplementedException($"No override has been implemented for {nameof(SendToProvider)} on this commandhandler");
        }
    }
}