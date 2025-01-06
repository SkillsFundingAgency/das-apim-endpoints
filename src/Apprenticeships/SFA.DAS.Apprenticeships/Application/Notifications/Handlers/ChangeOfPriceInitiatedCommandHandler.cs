using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{
    public class ChangeOfPriceInitiatedCommand : NotificationCommandBase, IRequest<NotificationResponse>
    {
        public string? Initiator { get; set; }
        public string? PriceChangeStatus { get; set; }
    }

    public class ChangeOfPriceInitiatedCommandHandler : NotificationCommandHandlerBase<ChangeOfPriceInitiatedCommand>
    {
        private readonly UrlBuilder _externalEmployerUrlHelper;

        public ChangeOfPriceInitiatedCommandHandler(
            IExtendedNotificationService notificationService, 
            UrlBuilder externalEmployerUrlHelper) 
            : base(notificationService)
        {
            _externalEmployerUrlHelper = externalEmployerUrlHelper;
        }

        public override async Task<NotificationResponse> Handle(ChangeOfPriceInitiatedCommand request, CancellationToken cancellationToken)
        {
            if (ShouldNotifyEmployer(request))
            {
                return await SendToEmployer(request, ProviderInitiatedChangeOfPriceToEmployer.TemplateId, (_, apprenticeship) => GetEmployerTokens(request, apprenticeship));
            } 
            
            if (ShouldNotifyProvider(request))
            {
                return await SendToProvider(request, EmployerInitiatedChangeOfPriceToProvider.TemplateId, (_, apprenticeship) => GetProviderTokens(request, apprenticeship));
            }

            //Send to employer on price reduction to be implemented in FLP-916
            return NotificationResponse.Ok();
        }

        private static bool ShouldNotifyProvider(ChangeOfPriceInitiatedCommand request)
        {
            return request.Initiator == RequestParty.Employer;
        }

        private static bool ShouldNotifyEmployer(ChangeOfPriceInitiatedCommand request)
        {
            return request.Initiator == RequestParty.Provider && request.PriceChangeStatus == ChangeRequestStatus.Created;
        }


        private Dictionary<string, string> GetEmployerTokens(ChangeOfPriceInitiatedCommand request, CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalEmployerUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(ProviderInitiatedChangeOfPriceToEmployer.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(ProviderInitiatedChangeOfPriceToEmployer.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(ProviderInitiatedChangeOfPriceToEmployer.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(ProviderInitiatedChangeOfPriceToEmployer.ReviewChangesUrl, linkUrl);
            
            return tokens;
        }

        private Dictionary<string, string> GetProviderTokens(ChangeOfPriceInitiatedCommand request, CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalEmployerUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(EmployerInitiatedChangeOfPriceToProvider.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(EmployerInitiatedChangeOfPriceToProvider.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(EmployerInitiatedChangeOfPriceToProvider.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(EmployerInitiatedChangeOfPriceToProvider.ReviewChangesUrl, linkUrl);

            return tokens;
        }
    }

}
