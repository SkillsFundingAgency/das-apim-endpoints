using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{
    public class ChangeOfPriceInitiatedCommand : NotificationCommandBase, IRequest<NotificationResponse>
    {
        public string? Initiator { get; set; }
    }

    public class ChangeOfPriceInitiatedCommandHandler : NotificationCommandHandlerBase<ChangeOfPriceInitiatedCommand>
    {
        public ChangeOfPriceInitiatedCommandHandler(IExtendedNotificationService notificationService) 
            : base(notificationService)
        {

        }

        public override async Task<NotificationResponse> Handle(ChangeOfPriceInitiatedCommand request, CancellationToken cancellationToken)
        {
            if (request.Initiator == "Provider")
            {
                return await SendToEmployer(request, ProviderInitiatedChangeOfPriceToEmployer.TemplateId, (_, apprenticeship) => GetEmployerTokens(request, apprenticeship));
            }

            return NotificationResponse.Ok();//Send to provider to be implemented in FLP-406
        }


        private Dictionary<string, string> GetEmployerTokens(ChangeOfPriceInitiatedCommand request, CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var tokens = new Dictionary<string, string>();
            tokens.Add(ProviderInitiatedChangeOfPriceToEmployer.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(ProviderInitiatedChangeOfPriceToEmployer.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(ProviderInitiatedChangeOfPriceToEmployer.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            return tokens;
        }

    }

}
