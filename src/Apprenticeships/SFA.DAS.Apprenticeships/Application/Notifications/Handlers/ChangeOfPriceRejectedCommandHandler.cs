using MediatR;
using SFA.DAS.Apprenticeships.Application.Notifications.Templates;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;

namespace SFA.DAS.Apprenticeships.Application.Notifications.Handlers
{

    public class ChangeOfPriceRejectedCommand : NotificationCommandBase, IRequest<NotificationResponse>
    {
        public string? Rejector { get; set; }
    }

    public class ChangeOfPriceRejectedCommandHandler : NotificationCommandHandlerBase<ChangeOfPriceRejectedCommand>
    {
        private readonly UrlBuilder _externalEmployerUrlHelper;

        public ChangeOfPriceRejectedCommandHandler(
            IExtendedNotificationService notificationService,
            UrlBuilder externalEmployerUrlHelper)
            : base(notificationService)
        {
            _externalEmployerUrlHelper = externalEmployerUrlHelper;
        }

        public override async Task<NotificationResponse> Handle(ChangeOfPriceRejectedCommand request, CancellationToken cancellationToken)
        {
            if (request.Rejector == RequestParty.Provider)
            {
                return await SendToEmployer(request, ProviderRejectedChangeOfPriceToEmployer.TemplateId, (_, apprenticeship) => GetEmployerTokens(apprenticeship));
            }
            else
            {
                return await SendToProvider(request, EmployerRejectedChangeOfPriceToProvider.TemplateId, (_, apprenticeship) => GetProviderTokens(apprenticeship));
            }

            return NotificationResponse.Ok();
        }

        private Dictionary<string, string> GetEmployerTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalEmployerUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(ProviderRejectedChangeOfPriceToEmployer.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(ProviderRejectedChangeOfPriceToEmployer.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(ProviderRejectedChangeOfPriceToEmployer.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(ProviderRejectedChangeOfPriceToEmployer.ApprenticeshipDetailsUrl, linkUrl);

            return tokens;
        }

        private Dictionary<string, string> GetProviderTokens(CommitmentsApprenticeshipDetails apprenticeshipDetails)
        {
            var linkUrl = _externalEmployerUrlHelper.CommitmentsV2Link("ApprenticeDetails", apprenticeshipDetails.EmployerAccountHashedId, apprenticeshipDetails.ApprenticeshipHashedId);

            var tokens = new Dictionary<string, string>();
            tokens.Add(EmployerRejectedChangeOfPriceToProvider.TrainingProvider, apprenticeshipDetails.ProviderName);
            tokens.Add(EmployerRejectedChangeOfPriceToProvider.Employer, apprenticeshipDetails.EmployerName);
            tokens.Add(EmployerRejectedChangeOfPriceToProvider.Apprentice, $"{apprenticeshipDetails.ApprenticeFirstName} {apprenticeshipDetails.ApprenticeLastName}");
            tokens.Add(EmployerRejectedChangeOfPriceToProvider.ApprenticeshipDetailsUrl, linkUrl);

            return tokens;
        }
    }
}
