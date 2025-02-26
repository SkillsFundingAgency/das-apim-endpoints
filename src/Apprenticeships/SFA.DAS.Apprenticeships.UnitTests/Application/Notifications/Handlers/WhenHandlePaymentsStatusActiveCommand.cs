using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications.Handlers;

internal class WhenHandlePaymentsStatusActiveCommand : BaseHandlerTestHelper
{
    [SetUp]
    public void SetUp()
    {
        Reset();
    }

    [Test]
    public async Task ShouldSendToProviderWithCorrectTokens()
    {
        // Arrange
        var command = new PaymentStatusActiveCommand
        {
            ApprenticeshipKey = Guid.NewGuid()
        };
        var handler = new PaymentsStatusActiveCommandHandler(GetExtendedNotificationService());

        // Act
        var response = await handler.Handle(command, new CancellationToken());

        // Assert
        VerifySentToProvider("PaymentStatusActiveToProvider", new Dictionary<string, string>
        {
            { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
            { "Employer", ExpectedApprenticeshipDetails.EmployerName },
            { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" },
            { "date", DateTime.Now.ToString("d MMMM yyyy") }
        });
    }
}