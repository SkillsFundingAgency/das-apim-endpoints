using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;
using SFA.DAS.Apprenticeships.Constants;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{
    public class ChangeOfPriceInitiatedCommand : NotificationCommandBase, IRequest<NotificationResponse>
    {
        public string? Initiator { get; set; }
        public string? PriceChangeStatus { get; set; }
    }

    public class ChangeOfPriceInitiatedCommandHandler : NotificationCommandHandlerBase<ChangeOfPriceInitiatedCommand>
    {
        public ChangeOfPriceInitiatedCommandHandler(IExtendedNotificationService notificationService) 
            : base(notificationService)
        {

        }

        public override async Task<NotificationResponse> Handle(ChangeOfPriceInitiatedCommand request, CancellationToken cancellationToken)
        {
            if (request.Initiator == RequestInitiator.Provider && request.PriceChangeStatus == ChangeRequestStatus.Created)
            {
                return await SendToEmployer(request, ProviderInitiatedChangeOfPriceToEmployer.TemplateId, (_, apprenticeship) => GetEmployerTokens(request, apprenticeship));
            }

            //Send to employer on price reduction to be implemented in FLP-916
            //Send to provider to be implemented in FLP-406
            return NotificationResponse.Ok();
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
