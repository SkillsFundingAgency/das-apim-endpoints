using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class DeleteApprenticeAccountCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Send_Delete_Request_With_Correct_Url(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
            DeleteApprenticeAccountCommandHandler sut,
            CancellationToken cancellationToken)
        {
            // Arrange
            var apprenticeId = Guid.NewGuid();
            var command = new DeleteApprenticeAccountCommand { ApprenticeId = apprenticeId };
            var expectedUrl = $"apprentices/{apprenticeId}";

            // Act
            var result = await sut.Handle(command, cancellationToken);

            // Assert
            apiClientMock.Verify(
                c => c.Delete(It.Is<DeleteApprenticeAccountRequest>(
                    r => r.DeleteUrl == expectedUrl)),
                Times.Once);

            result.Should().Be(Unit.Value);
        }
    }
}