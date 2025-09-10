using MediatR;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Learning.Application.Notification.Templates;

namespace SFA.DAS.Learning.Application.Notification.Handlers;

public class ApprenticeshipWithdrawnCommand : NotificationCommandBase, IRequest<NotificationResponse>
{
    public DateTime LastDayOfLearning { get; set; }
    public string Reason { get; set; }
}

public class ApprenticeshipWithdrawnCommandHandler : NotificationCommandHandlerBase<ApprenticeshipWithdrawnCommand>
{
    private readonly UrlBuilder _externalEmployerUrlHelper;
    private const string WithdrawFromBeta = "WithdrawFromBeta";

    public ApprenticeshipWithdrawnCommandHandler(
        IExtendedNotificationService notificationService,
        UrlBuilder externalEmployerUrlHelper)
        : base(notificationService)
    {
        _externalEmployerUrlHelper = externalEmployerUrlHelper;
    }

    public override async Task<NotificationResponse> Handle(ApprenticeshipWithdrawnCommand request, CancellationToken cancellationToken)
    {
        if (request.Reason == WithdrawFromBeta)
            return new NotificationResponse { Success = true };

        return await SendToEmployer(request, ApprenticeshipStatusWithdrawnToEmployer.TemplateId, (_, apprenticeship) => GetEmployerTokens(apprenticeship, request.LastDayOfLearning));
    }

    private Dictionary<string, string> GetEmployerTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails, DateTime lastDayOfLearning)
    {
        var linkUrl = _externalEmployerUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

        var tokens = new Dictionary<string, string>();
        tokens.Add(ApprenticeshipStatusWithdrawnToEmployer.TrainingProvider, apprenticeshipDetails.ProviderName);
        tokens.Add(ApprenticeshipStatusWithdrawnToEmployer.Employer, apprenticeshipDetails.EmployerName);
        tokens.Add(ApprenticeshipStatusWithdrawnToEmployer.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
        tokens.Add(ApprenticeshipStatusWithdrawnToEmployer.Date, lastDayOfLearning.ToString("d MMMM yyyy"));
        tokens.Add(ApprenticeshipStatusWithdrawnToEmployer.ApprenticeDetailsUrl, linkUrl);


        return tokens;
    }
}
