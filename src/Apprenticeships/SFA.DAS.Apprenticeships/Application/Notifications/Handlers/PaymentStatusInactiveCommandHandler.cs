using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;
using SFA.DAS.Employer.Shared.UI;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{

    public class PaymentStatusInactiveCommand : NotificationCommandBase, IRequest<NotificationResponse> { }

    public class PaymentsStatusInactiveCommandHandler : NotificationCommandHandlerBase<PaymentStatusInactiveCommand>
    {
        private readonly UrlBuilder _externalProviderUrlHelper;

        public PaymentsStatusInactiveCommandHandler(
            IExtendedNotificationService notificationService,
            UrlBuilder externalProviderUrlHelper)
            : base(notificationService)
        {
            _externalProviderUrlHelper = externalProviderUrlHelper;
        }

        public override async Task<NotificationResponse> Handle(PaymentStatusInactiveCommand request, CancellationToken cancellationToken)
        {
            return await SendToProvider(request, PaymentStatusInactiveToProvider.TemplateId, (_, apprenticeship) => GetProviderTokens(apprenticeship));
        }

        private Dictionary<string, string> GetProviderTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalProviderUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(PaymentStatusInactiveToProvider.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(PaymentStatusInactiveToProvider.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(PaymentStatusInactiveToProvider.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(PaymentStatusInactiveToProvider.ApprenticeshipDetailsUrl, linkUrl);
            tokens.Add(PaymentStatusInactiveToProvider.Date, DateTime.Now.ToString("d MMMM yyyy"));

            return tokens;
        }
    }
}
