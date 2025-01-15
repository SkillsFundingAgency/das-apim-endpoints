using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.Apprenticeships.Constants;
using SFA.DAS.Employer.Shared.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications.Handlers
{
    internal class WhenHandleChangeOfDateApprovedCommand : BaseHandlerTestHelper
    {
        private UrlBuilder _externalEmployerUrlHelper;

        public WhenHandleChangeOfDateApprovedCommand()
        {
            _externalEmployerUrlHelper = new UrlBuilder("AT");
        }

        [SetUp]
        public void SetUp()
        {
            Reset();
        }

        [Test]
        public async Task ShouldSendToProvider()
        {
            // Arrange
            var command = new ChangeOfDateApprovedCommand
            {
                ApprenticeshipKey = Guid.NewGuid()               
            };
            var handler = new ChangeOfDateApprovedCommandHandler(GetExtendedNotificationService(), _externalEmployerUrlHelper);

            // Act
            var response = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            VerifySentToProvider("EmployerApprovedChangeOfDateToProvider", new Dictionary<string, string>
            {
                { "Training provider", ExpectedApprenticeshipDetails.ProviderName },
                { "Employer", ExpectedApprenticeshipDetails.EmployerName },
                { "apprentice", $"{ExpectedApprenticeshipDetails.ApprenticeFirstName} {ExpectedApprenticeshipDetails.ApprenticeLastName}" },
                { "Apprentice details URL", $"https://approvals.at-eas.apprenticeships.education.gov.uk/{ExpectedApprenticeshipDetails.EmployerAccountHashedId}/apprentices/{ExpectedApprenticeshipDetails.ApprenticeshipHashedId}/details" }
            });
        }        
    }
}
