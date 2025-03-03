using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{
    public class ChangeOfPriceApprovedCommand : NotificationCommandBase, IRequest<NotificationResponse>
    {
        public string? Approver { get; set; }
    }

    public class ChangeOfPriceApprovedCommandHandler : NotificationCommandHandlerBase<ChangeOfPriceApprovedCommand>
    {
        private readonly UrlBuilder _externalEmployerUrlHelper;

        public ChangeOfPriceApprovedCommandHandler(
            IExtendedNotificationService notificationService,
            UrlBuilder externalEmployerUrlHelper)
            : base(notificationService)
        {
            _externalEmployerUrlHelper = externalEmployerUrlHelper;
        }

        public override async Task<NotificationResponse> Handle(ChangeOfPriceApprovedCommand request, CancellationToken cancellationToken)
        {
            if(ShouldNotifyEmployer(request))
            {
                return await SendToEmployer(request, ProviderApprovedChangeOfPriceToEmployer.TemplateId, (_, apprenticeship) => GetEmployerTokens(apprenticeship));
            }
            
            if(ShouldNotifyProvider(request))
            {
                return await SendToProvider(request, EmployerApprovedChangeOfPriceToProvider.TemplateId, (_, apprenticeship) => GetProviderTokens(apprenticeship));
            }

            return NotificationResponse.Ok();
        }

        private static bool ShouldNotifyEmployer(ChangeOfPriceApprovedCommand request)
        {
            return request.Approver == RequestParty.Provider;
        }

        private static bool ShouldNotifyProvider(ChangeOfPriceApprovedCommand request)
        {
            return request.Approver == RequestParty.Employer;
        }

        private Dictionary<string, string> GetProviderTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalEmployerUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(EmployerApprovedChangeOfPriceToProvider.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(EmployerApprovedChangeOfPriceToProvider.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(EmployerApprovedChangeOfPriceToProvider.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(EmployerApprovedChangeOfPriceToProvider.ApprenticeDetailsUrl, linkUrl);

            return tokens;
        }

        private Dictionary<string, string> GetEmployerTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalEmployerUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(ProviderApprovedChangeOfPriceToEmployer.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(ProviderApprovedChangeOfPriceToEmployer.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(ProviderApprovedChangeOfPriceToEmployer.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(ProviderApprovedChangeOfPriceToEmployer.ReviewChangesUrl, linkUrl);

            return tokens;
        }
    }
}
