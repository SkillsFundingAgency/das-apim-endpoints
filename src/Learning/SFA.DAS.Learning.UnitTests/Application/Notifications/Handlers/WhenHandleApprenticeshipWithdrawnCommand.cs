using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Learning.Application.Notification.Handlers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Learning.UnitTests.Application.Notifications.Handlers;

[TestFixture]
internal class WhenHandleApprenticeshipWithdrawnCommand : BaseHandlerTestHelper
{
    private UrlBuilder _externalEmployerUrlHelper;

    public WhenHandleApprenticeshipWithdrawnCommand()
    {
        _externalEmployerUrlHelper = new UrlBuilder("AT");
    }

    [SetUp]
    public void SetUp()
    {
        Reset();
    }

    [Test]
    public async Task ShouldSendToEmployer()
    {
        // Arrange
        var command = new ApprenticeshipWithdrawnCommand
        {
            ApprenticeshipKey = Guid.NewGuid(),
            LastDayOfLearning = new DateTime(2024, 05, 01)
        };
        var handler = new ApprenticeshipWithdrawnCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        VerifySentToEmployer("ApprenticeshipStatusWithdrawnToEmployer", new Dictionary<string, string>
            {
                { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
                { "Employer", ExpectedApprenticeshipDetails.EmployerName },
                { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" },
                { "date", command.LastDayOfLearning.ToString("d MMMM yyyy") },
                { "Apprentice details URL", $"https://approvals.at-eas.apprenticeships.education.gov.uk/{ExpectedApprenticeshipDetails.EmployerAccountHashedId}/apprentices/{ExpectedApprenticeshipDetails.ApprenticeshipHashedId}/details" }
            });
    }

    [Test]
    public async Task ShouldNotSendInWithdrawFromBetaScenario()
    {
        // Arrange
        var command = new ApprenticeshipWithdrawnCommand
        {
            ApprenticeshipKey = Guid.NewGuid(),
            LastDayOfLearning = new DateTime(2024, 05, 01),
            Reason = "WithdrawFromBeta"
        };
        var handler = new ApprenticeshipWithdrawnCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        VerifyNoMessageSentToEmployer();
    }
}
