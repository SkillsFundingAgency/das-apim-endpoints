using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByEmail;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetRegistrationsByEmailQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Calls_Client_With_Email_And_Returns_Registrations(
           [Frozen] Mock<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>> clientMock,
           string email,
           List<Registration> registrations)
        {
            // Arrange
            clientMock
                .Setup(c => c.Get<List<Registration>>(
                    It.Is<GetRegistrationsByEmailRequest>(r =>
                        r.GetUrl == $"registrations/email?email={email}")))
                .ReturnsAsync(registrations);

            var handler = new GetRegistrationsByEmailQueryHandler(clientMock.Object);

            var query = new GetRegistrationsByEmailQuery { Email = email };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Registrations.Should().BeSameAs(registrations);

            clientMock.Verify(c => c.Get<List<Registration>>(
                It.Is<GetRegistrationsByEmailRequest>(r =>
                    r.GetUrl == $"registrations/email?email={email}")),
                Times.Once);
        }
    }
}
