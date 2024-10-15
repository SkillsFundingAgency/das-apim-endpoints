using FluentAssertions.Common;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Initiator = "Provider"
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

        [Test]
        public async Task AndInitiatorIsNotProvider_ShouldNotSendToEmployer()
        {
            // Arrange
            var command = new ChangeOfPriceInitiatedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Initiator = "Invalid"
            };
            var handler = new ChangeOfPriceInitiatedCommandHandler(GetExtendedNotificationService());

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifyNoMessageSent();
        }
    }
}
