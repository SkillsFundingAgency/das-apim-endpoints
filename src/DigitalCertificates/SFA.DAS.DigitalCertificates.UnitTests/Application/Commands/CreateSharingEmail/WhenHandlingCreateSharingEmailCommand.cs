using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharingEmail
{
    public class WhenHandlingCreateSharingEmailCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Email_Is_Created_And_Email_Sent_Successfully(
            CreateSharingEmailCommand command,
            CreateSharingEmailResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<INotificationService> mockNotificationService,
            CreateSharingEmailCommandHandler handler)
        {
            // Arrange
            command.TemplateId = command.TemplateId ?? Guid.NewGuid().ToString();

            var apiResponse = new ApiResponse<CreateSharingEmailResponse>(
                apiResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateSharingEmailResponse>(
                    It.IsAny<PostSharingByIdEmailApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.Id.Should().Be(apiResponseBody.Id);
            actual.EmailLinkCode.Should().Be(apiResponseBody.EmailLinkCode);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateSharingEmailResponse>(
                    It.Is<PostSharingByIdEmailApiRequest>(r =>
                        r.PostUrl == $"api/sharing/{command.SharingId}/email" &&
                        ((CreateSharingEmailRequest)r.Data).EmailAddress == command.EmailAddress), true), Times.Once);

            mockNotificationService.Verify(n => n.Send(It.IsAny<Notifications.Messages.Commands.SendEmailCommand>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateSharingEmailCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<INotificationService> mockNotificationService,
            CreateSharingEmailCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateSharingEmailResponse>(
                    It.IsAny<PostSharingByIdEmailApiRequest>(), true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateSharingEmailResponse>(
                    It.IsAny<PostSharingByIdEmailApiRequest>(), true), Times.Once);

            mockNotificationService.Verify(n => n.Send(It.IsAny<Notifications.Messages.Commands.SendEmailCommand>()), Times.Never);
        }
    }
}
