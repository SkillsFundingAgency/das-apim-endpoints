using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{
    public class ChangeOfDateApprovedCommand : NotificationCommandBase, IRequest<NotificationResponse>
    {
    }

    public class ChangeOfDateApprovedCommandHandler : NotificationCommandHandlerBase<ChangeOfDateApprovedCommand>
    {
        private readonly UrlBuilder _externalProviderUrlHelper;

        public ChangeOfDateApprovedCommandHandler(
            IExtendedNotificationService notificationService,
            UrlBuilder externalEmployerUrlHelper)
            : base(notificationService)
        {
            _externalProviderUrlHelper = externalEmployerUrlHelper;
        }

        public override async Task<NotificationResponse> Handle(ChangeOfDateApprovedCommand request, CancellationToken cancellationToken)
        {            
            return await SendToProvider(request, EmployerApprovedChangeOfDateToProvider.TemplateId, (_, apprenticeship) => GetEmployerTokens(apprenticeship));            
        }       

        private Dictionary<string, string> GetEmployerTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalProviderUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(EmployerApprovedChangeOfDateToProvider.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(EmployerApprovedChangeOfDateToProvider.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(EmployerApprovedChangeOfDateToProvider.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(EmployerApprovedChangeOfDateToProvider.ApprenticeDetailsUrl, linkUrl);

            return tokens;
        }
    }
}
