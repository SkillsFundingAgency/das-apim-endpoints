using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.Employer.Shared.UI;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications.Handlers;

internal class WhenHandlePaymentsStatusInactiveCommand : BaseHandlerTestHelper
{
    private UrlBuilder _externalProviderUrlHelper;

    public WhenHandlePaymentsStatusInactiveCommand()
    {
        _externalProviderUrlHelper = new UrlBuilder("AT");
    }

    [SetUp]
    public void SetUp()
    {
        Reset();
    }

    [Test]
    public async Task ShouldSendToProviderWithCorrectTokens()
    {
        // Arrange
        var command = new PaymentStatusInactiveCommand
        {
            ApprenticeshipKey = Guid.NewGuid()
        };
        var handler = new PaymentsStatusInactiveCommandHandler(GetExtendedNotificationService(), _externalProviderUrlHelper);

        // Act
        var response = await handler.Handle(command, new CancellationToken());

        // Assert
        VerifySentToProvider("PaymentStatusInactiveToProvider", new Dictionary<string, string>
        {
            { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
            { "Employer", ExpectedApprenticeshipDetails.EmployerName },
            { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" },
            { "Apprentice details URL", $"https://approvals.at-eas.apprenticeships.education.gov.uk/{ExpectedApprenticeshipDetails.EmployerAccountHashedId}/apprentices/{ExpectedApprenticeshipDetails.ApprenticeshipHashedId}/details" },
            { "date", DateTime.Now.ToString("d MMMM yyyy") }
        });
    }
}