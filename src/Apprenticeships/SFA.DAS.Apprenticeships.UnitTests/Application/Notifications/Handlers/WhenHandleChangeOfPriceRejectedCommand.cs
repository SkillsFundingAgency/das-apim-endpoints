using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications.Handlers
{
    internal class WhenHandleChangeOfPriceRejectedCommand : BaseHandlerTestHelper
    {
        private UrlBuilder _externalEmployerUrlHelper;

        public WhenHandleChangeOfPriceRejectedCommand()
        {
            _externalEmployerUrlHelper = new UrlBuilder("AT");
        }

        [SetUp]
        public void SetUp()
        {
            Reset();
        }

        [Test]
        public async Task AndApproverIsProvider_ShouldSendToEmployer()
        {
            // Arrange
            var command = new ChangeOfPriceRejectedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Rejector = RequestParty.Provider
            };
            var handler = new ChangeOfPriceRejectedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifySentToEmployer("ProviderRejectedChangeOfPriceToEmployer", new Dictionary<string, string>
            {
                { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
                { "Employer", ExpectedApprenticeshipDetails.EmployerName },
                { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" },
                { "Apprentice details URL", $"https://approvals.at-eas.apprenticeships.education.gov.uk/{ExpectedApprenticeshipDetails.EmployerAccountHashedId}/apprentices/{ExpectedApprenticeshipDetails.ApprenticeshipHashedId}/details" }
            });
        }

        [Test]
        public async Task AndApproverIsEmployer_ShouldSendToProvider()
        {
            // Arrange
            var command = new ChangeOfPriceRejectedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Rejector = RequestParty.Employer
            };
            var handler = new ChangeOfPriceRejectedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifySentToProvider("EmployerRejectedChangeOfPriceToProvider", new Dictionary<string, string>
            {
                { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
                { "Employer", ExpectedApprenticeshipDetails.EmployerName },
                { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" },
                { "Apprentice details URL", $"https://approvals.at-eas.apprenticeships.education.gov.uk/{ExpectedApprenticeshipDetails.EmployerAccountHashedId}/apprentices/{ExpectedApprenticeshipDetails.ApprenticeshipHashedId}/details" }
            });
        }

        [Test]
        public async Task AndInitiatorIsNotProvider_ShouldNotSendToEmployer()
        {
            // Arrange
            var command = new ChangeOfPriceRejectedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Rejector = RequestParty.Employer
            };
            var handler = new ChangeOfPriceRejectedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifyNoMessageSentToEmployer();
        }
    }
}
