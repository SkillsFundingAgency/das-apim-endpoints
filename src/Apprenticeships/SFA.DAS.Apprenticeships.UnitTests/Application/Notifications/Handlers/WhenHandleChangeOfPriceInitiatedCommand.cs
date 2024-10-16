using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.Apprenticeships.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications.Handlers
{
    internal class WhenHandleChangeOfPriceInitiatedCommand : BaseHandlerTestHelper
    {
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
                Initiator = RequestInitiator.Provider,
                PriceChangeStatus = ChangeRequestStatus.Created
            };
            var handler = new ChangeOfPriceInitiatedCommandHandler(GetExtendedNotificationService());

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifySentToEmployer("ProviderInitiatedChangeOfPriceToEmployer", new Dictionary<string, string>
            {
                { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
                { "Employer", ExpectedApprenticeshipDetails.EmployerName },
                { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" }
            });
        }

        [TestCase(RequestInitiator.Provider, "Invalid")]
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
            var handler = new ChangeOfPriceInitiatedCommandHandler(GetExtendedNotificationService());

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifyNoMessageSent();
        }
    }
}
