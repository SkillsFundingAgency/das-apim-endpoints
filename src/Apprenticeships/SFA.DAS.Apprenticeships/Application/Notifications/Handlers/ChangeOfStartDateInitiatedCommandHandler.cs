using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{
    public class ChangeOfStartDateInitiatedCommand : NotificationCommandBase, IRequest<NotificationResponse>
    {
        public string? Initiator { get; set; }
    }

    public class ChangeOfStartDateInitiatedCommandHandler : NotificationCommandHandlerBase<ChangeOfStartDateInitiatedCommand>
    {
        private readonly UrlBuilder _externalEmployerUrlHelper;

        public ChangeOfStartDateInitiatedCommandHandler(
            IExtendedNotificationService notificationService,
            UrlBuilder externalEmployerUrlHelper)
            : base(notificationService)
        {
            _externalEmployerUrlHelper = externalEmployerUrlHelper;
        }

        public override async Task<NotificationResponse> Handle(ChangeOfStartDateInitiatedCommand request, CancellationToken cancellationToken)
        {
            if (request.Initiator == RequestParty.Provider)
            {
                return await SendToEmployer(request, ProviderInitiatedChangeOfStartDateToEmployer.TemplateId, (_, apprenticeship) => GetEmployerTokens( apprenticeship));
            }

            return NotificationResponse.Ok();
        }


        private Dictionary<string, string> GetEmployerTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalEmployerUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(ProviderInitiatedChangeOfStartDateToEmployer.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(ProviderInitiatedChangeOfStartDateToEmployer.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(ProviderInitiatedChangeOfStartDateToEmployer.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(ProviderInitiatedChangeOfStartDateToEmployer.ReviewChangesUrl, linkUrl);

            return tokens;
        }

    }
}