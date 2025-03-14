using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{

    public class PaymentStatusActiveCommand : NotificationCommandBase, IRequest<NotificationResponse> { }

    public class PaymentsStatusActiveCommandHandler : NotificationCommandHandlerBase<PaymentStatusActiveCommand>
    {
        public PaymentsStatusActiveCommandHandler(
            IExtendedNotificationService notificationService)
            : base(notificationService) { }

        public override async Task<NotificationResponse> Handle(PaymentStatusActiveCommand request, CancellationToken cancellationToken)
        {
            return await SendToProvider(request, PaymentStatusActiveToProvider.TemplateId, (_, apprenticeship) => GetProviderTokens(apprenticeship));
        }

        private Dictionary<string, string> GetProviderTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var tokens = new Dictionary<string, string>();
            tokens.Add(PaymentStatusActiveToProvider.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(PaymentStatusActiveToProvider.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(PaymentStatusActiveToProvider.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(PaymentStatusActiveToProvider.Date, DateTime.Now.ToString("d MMMM yyyy"));

            return tokens;
        }
    }
}
