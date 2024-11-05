using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications.Handlers
{
    internal class WhenHandleChangeOfStartDateInitiatedCommand : BaseHandlerTestHelper
    {
        private UrlBuilder _externalEmployerUrlHelper;

        public WhenHandleChangeOfStartDateInitiatedCommand()
        {
            _externalEmployerUrlHelper = new UrlBuilder("AT");
        }

        [SetUp]
        public void SetUp()
        {
            Reset();
        }

        [Test]
        public async Task AndInitiatorIsProvider_ShouldSendToEmployer()
        {
            // Arrange
            var command = new ChangeOfStartDateInitiatedCommand()
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Initiator = RequestParty.Provider
            };
            var handler = new ChangeOfStartDateInitiatedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifySentToEmployer("ProviderInitiatedChangeOfStartDateToEmployer", new Dictionary<string, string>
            {
                { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
                { "Employer", ExpectedApprenticeshipDetails.EmployerName },
                { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" },
                { "review changes URL", $"https://approvals.at-eas.apprenticeships.education.gov.uk/{ExpectedApprenticeshipDetails.EmployerAccountHashedId}/apprentices/{ExpectedApprenticeshipDetails.ApprenticeshipHashedId}/details" }
            });
        }


        [Test]
        public async Task AndInitiatorIsNotProvider_ShouldNotSendToEmployer()
        {
            // Arrange
            var command = new ChangeOfStartDateInitiatedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Initiator = "Invalid",
            };
            var handler = new ChangeOfStartDateInitiatedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifyNoMessageSent();
        }
    }
}