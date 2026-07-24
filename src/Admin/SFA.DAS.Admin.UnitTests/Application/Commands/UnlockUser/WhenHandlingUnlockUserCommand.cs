using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Admin.Application.Commands.UnlockUser;
using SFA.DAS.Apim.Shared.Exceptions;

namespace SFA.DAS.Admin.UnitTests.Application.Commands.UnlockUser
{
    public class WhenHandlingUnlockUserCommand
    {
        [Test, MoqAutoData]
        public async Task Then_AdminAction_Is_Posted_When_Put_Returns_204(
            UnlockUserCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UnlockUserCommandHandler handler)
        {
            // Arrange
            var putResponse = new ApiResponse<NullResponse>(null, HttpStatusCode.NoContent, string.Empty);
            var postResponse = new ApiResponse<CreateAdminActionRequest>(null, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.PutWithResponseCode<object, NullResponse>(It.IsAny<PutUsersByUserIdUnlockApiRequest>()))
                .ReturnsAsync(putResponse);

            mockDigitalCertificatesApiClient
                .Setup(c => c.PostWithResponseCode<CreateAdminActionRequest>(It.IsAny<PostUserActionsByUserActionIdAdminActionsApiRequest>()))
                .ReturnsAsync(postResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(c => c.PutWithResponseCode<object, NullResponse>(It.IsAny<PutUsersByUserIdUnlockApiRequest>()), Times.Once);

            mockDigitalCertificatesApiClient.Verify(c => c.PostWithResponseCode<CreateAdminActionRequest>(
                It.Is<PostUserActionsByUserActionIdAdminActionsApiRequest>(p => p.UserActionId == command.UserActionId && ((CreateAdminActionRequest)p.Data).Username == command.Username && ((CreateAdminActionRequest)p.Data).Action.ToString() == "Unlocked")), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Throws_ArgumentException_If_Put_Returns_BadRequest(
            UnlockUserCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UnlockUserCommandHandler handler)
        {
            // Arrange
            var putResponse = new ApiResponse<NullResponse>(null, HttpStatusCode.BadRequest, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.PutWithResponseCode<object, NullResponse>(It.IsAny<PutUsersByUserIdUnlockApiRequest>()))
                .ReturnsAsync(putResponse);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiResponseException>();

            mockDigitalCertificatesApiClient.Verify(c => c.PostWithResponseCode<CreateAdminActionRequest>(It.IsAny<PostUserActionsByUserActionIdAdminActionsApiRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Post_AdminAction_Fails(
            UnlockUserCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UnlockUserCommandHandler handler)
        {
            // Arrange
            var putResponse = new ApiResponse<NullResponse>(null, HttpStatusCode.NoContent, string.Empty);
            var postResponse = new ApiResponse<CreateAdminActionRequest>(null, HttpStatusCode.BadRequest, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.PutWithResponseCode<object, NullResponse>(It.IsAny<PutUsersByUserIdUnlockApiRequest>()))
                .ReturnsAsync(putResponse);

            mockDigitalCertificatesApiClient
                .Setup(c => c.PostWithResponseCode<CreateAdminActionRequest>(It.IsAny<PostUserActionsByUserActionIdAdminActionsApiRequest>()))
                .ReturnsAsync(postResponse);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>();
        }
    }
}
