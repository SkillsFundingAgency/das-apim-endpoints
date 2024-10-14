using MediatR;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{
    public class ChangeOfPriceInitiatedCommand : NotificationCommandBase, IRequest<NotificationResponse>
    {
        public string? Initiator { get; set; }
    }

    public class ChangeOfPriceInitiatedCommandHandler : NotificationCommandHandlerBase<ChangeOfPriceInitiatedCommand>
    {
        private const string ToEmployerTemplateId = "EmployerChangeOfPriceInitiated";//CODE REVIEW TALKING POINT: Do we want to discuss a naming convention for these?
        
        public ChangeOfPriceInitiatedCommandHandler(IExtendedNotificationService notificationService) 
            : base(notificationService)
        {

        }

        public override async Task<NotificationResponse> Handle(ChangeOfPriceInitiatedCommand request, CancellationToken cancellationToken)
        {
            if (request.Initiator == "Provider")
            {
                return await SendToEmployer(request, ToEmployerTemplateId, (_, apprenticeship) => GetEmployerTokens(request, apprenticeship));
            }

            return NotificationResponse.Ok();//Send to provider to be implemented in FLP-406
        }


        private Dictionary<string, string> GetEmployerTokens(ChangeOfPriceInitiatedCommand request, CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var tokens = new Dictionary<string, string>();
            tokens.Add("Training provider", apprenticeshipDetails.ProviderName);
            tokens.Add("Employer", apprenticeshipDetails.EmployerName);
            tokens.Add("apprentice", $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            return tokens;
        }

    }

}
