using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserAction
{
    public class WhenHandlingCreateUserActionCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Action_Is_Created_Successfully(
            CreateUserActionCommand command,
            CreateUserActionResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserActionCommandHandler handler)
        {
            // Arrange
            command.ActionType = "Reprint";
            command.CertificateType = "Framework";

            var apiResponse = new ApiResponse<CreateUserActionResponse>(
                apiResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateUserActionResponse>(
                    It.IsAny<PostUsersByUserIdUserActionsApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.ActionCode.Should().Be(apiResponseBody.ActionCode);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateUserActionResponse>(
                    It.Is<PostUsersByUserIdUserActionsApiRequest>(r =>
                        ((CreateUserActionRequest)r.Data).ActionType == ActionType.Reprint &&
                        ((CreateUserActionRequest)r.Data).FamilyName == command.FamilyName &&
                        ((CreateUserActionRequest)r.Data).GivenNames == command.GivenNames &&
                        ((CreateUserActionRequest)r.Data).CertificateId == command.CertificateId &&
                        ((CreateUserActionRequest)r.Data).CertificateType == CertificateType.Framework &&
                        ((CreateUserActionRequest)r.Data).CourseName == command.CourseName &&
                        r.PostUrl == $"api/users/{command.UserId}/user-actions"), true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_A_Missing_Certificate_Type_Is_Sent_As_Unknown(
            CreateUserActionCommand command,
            CreateUserActionResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserActionCommandHandler handler)
        {
            // Arrange
            command.ActionType = "help";
            command.CertificateId = null;
            command.CertificateType = null;

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateUserActionResponse>(
                    It.IsAny<PostUsersByUserIdUserActionsApiRequest>(), true))
                .ReturnsAsync(new ApiResponse<CreateUserActionResponse>(apiResponseBody, HttpStatusCode.OK, string.Empty));

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateUserActionResponse>(
                    It.Is<PostUsersByUserIdUserActionsApiRequest>(r =>
                        ((CreateUserActionRequest)r.Data).ActionType == ActionType.Help &&
                        ((CreateUserActionRequest)r.Data).CertificateType == CertificateType.Unknown), true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_An_Invalid_Action_Type_Is_Rejected_Before_Calling_The_Api(
            CreateUserActionCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserActionCommandHandler handler)
        {
            // Arrange
            command.ActionType = "NotAnActionType";
            command.CertificateType = "Standard";

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateUserActionResponse>(It.IsAny<PostUsersByUserIdUserActionsApiRequest>(), It.IsAny<bool>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateUserActionCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserActionCommandHandler handler)
        {
            // Arrange
            command.ActionType = "Contact";
            command.CertificateType = "Standard";

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateUserActionResponse>(
                    It.IsAny<PostUsersByUserIdUserActionsApiRequest>(), true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateUserActionResponse>(
                    It.IsAny<PostUsersByUserIdUserActionsApiRequest>(), true), Times.Once);
        }
    }
}
