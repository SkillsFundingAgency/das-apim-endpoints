using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class AddApprenticeshipProgressCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Post_AddApprenticeshipProgressRequest_With_ApprenticeId_In_Url(
            Mock<IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration>> clientMock,
            AddApprenticeshipProgressCommand command,
            CancellationToken cancellationToken)
        {
            // Arrange
            var apprenticeId = Guid.NewGuid();
            command.ApprenticeId = apprenticeId;

            clientMock
                .Setup(c => c.PostWithResponseCode<object>(
                    It.IsAny<AddApprenticeshipProgressRequest>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult<ApiResponse<object>>(null));

            var sut = new AddApprenticeshipProgressCommandHandler(clientMock.Object);

            // Act
            var result = await sut.Handle(command, cancellationToken);

            // Assert
            result.Should().Be(Unit.Value);

            clientMock.Verify(c => c.PostWithResponseCode<object>(
                    It.Is<AddApprenticeshipProgressRequest>(
                        request => request.PostUrl == $"notifications/apprenticeship-progress/{apprenticeId}"),
                    It.IsAny<bool>()),
                Times.Once);
        }
    }
}