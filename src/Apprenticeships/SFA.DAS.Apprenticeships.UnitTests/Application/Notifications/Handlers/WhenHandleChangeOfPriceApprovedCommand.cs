using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications.Handlers
{
    internal class WhenHandleChangeOfPriceApprovedCommand : BaseHandlerTestHelper
    {
        private UrlBuilder _externalEmployerUrlHelper;

        public WhenHandleChangeOfPriceApprovedCommand()
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
            var command = new ChangeOfPriceApprovedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Approver = RequestParty.Provider
            };
            var handler = new ChangeOfPriceApprovedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifySentToEmployer("ProviderApprovedChangeOfPriceToEmployer", new Dictionary<string, string>
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
            var command = new ChangeOfPriceApprovedCommand
            {
                ApprenticeshipKey = Guid.NewGuid(),
                Approver = RequestParty.Employer
            };
            var handler = new ChangeOfPriceApprovedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifyNoMessageSentToEmployer();
        }
    }
}
