using MediatR;

namespace SFA.DAS.Learning.Application.Notification;

public abstract class NotificationCommandBase : IRequest<NotificationResponse>
{
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
        var recipients = await _notificationService.GetEmployerRecipients(currentParties.EmployerAccountId);
        var apprenticeship = await _notificationService.GetApprenticeship(currentParties);

        foreach (var recipient in recipients)
        {
            var tokens = getTokensFunction(recipient, apprenticeship);
            await _notificationService.Send(recipient, templateId, tokens);
        }

        return notificationResponse;
    }

    protected async Task<NotificationResponse> SendToProvider(T request, string templateId, Func<Recipient, CommitmentsApprenticeshipDetails, Dictionary<string, string>> getTokensFunction)
    {
        var notificationResponse = new NotificationResponse { Success = true };
        var currentParties = await _notificationService.GetCurrentPartyIds(request.ApprenticeshipKey);
        var recipients = await _notificationService.GetProviderRecipients(currentParties.Ukprn);
        var apprenticeship = await _notificationService.GetApprenticeship(currentParties);

        foreach (var recipient in recipients)
        {
            var tokens = getTokensFunction(recipient, apprenticeship);
            await _notificationService.Send(recipient, templateId, tokens);
        }

        return notificationResponse;
    }
}
