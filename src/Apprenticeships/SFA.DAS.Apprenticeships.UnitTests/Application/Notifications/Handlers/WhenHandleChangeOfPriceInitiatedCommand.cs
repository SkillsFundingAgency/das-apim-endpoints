using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications.Handlers
{
    internal class WhenHandleChangeOfPriceInitiatedCommand : BaseHandlerTestHelper
    {
        private UrlBuilder _externalEmployerUrlHelper;

        public WhenHandleChangeOfPriceInitiatedCommand()
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
            var command = new ChangeOfPriceInitiatedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Initiator = RequestParty.Provider,
                PriceChangeStatus = ChangeRequestStatus.Created
            };
            var handler = new ChangeOfPriceInitiatedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifySentToEmployer("ProviderInitiatedChangeOfPriceToEmployer", new Dictionary<string, string>
            {
                { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
                { "Employer", ExpectedApprenticeshipDetails.EmployerName },
                { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" },
                { "review changes URL", $"https://approvals.at-eas.apprenticeships.education.gov.uk/{ExpectedApprenticeshipDetails.EmployerAccountHashedId}/apprentices/{ExpectedApprenticeshipDetails.ApprenticeshipHashedId}/details" }
            });
        }

        [TestCase(RequestParty.Provider, "Invalid")]
        [TestCase("Invalid", ChangeRequestStatus.Created)]
        public async Task AndInitiatorIsNotProvider_ShouldNotSendToEmployer(string initiator, string changeStatus)
        {
            // Arrange
            var command = new ChangeOfPriceInitiatedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Initiator = initiator,
                PriceChangeStatus = changeStatus
            };
            var handler = new ChangeOfPriceInitiatedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifyNoMessageSent();
        }
    }
}
