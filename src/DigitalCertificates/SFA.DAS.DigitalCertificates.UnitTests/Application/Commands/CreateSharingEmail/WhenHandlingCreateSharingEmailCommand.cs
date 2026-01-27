using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharingEmail
{
    public class WhenHandlingCreateSharingEmailCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Email_Is_Created_And_Email_Sent_Successfully(
            CreateSharingEmailCommand command,
            PostCreateSharingEmailResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<SFA.DAS.SharedOuterApi.Interfaces.INotificationService> mockNotificationService,
            CreateSharingEmailCommandHandler handler)
        {
            // Arrange
            command.TemplateId = command.TemplateId ?? Guid.NewGuid().ToString();

            var apiResponse = new ApiResponse<PostCreateSharingEmailResponse>(
                apiResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateSharingEmailRequestData, PostCreateSharingEmailResponse>(
                    It.Is<PostCreateSharingEmailRequest>(r => r.Data.EmailAddress == command.EmailAddress), true))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.Id.Should().Be(apiResponseBody.Id);
            actual.EmailLinkCode.Should().Be(apiResponseBody.EmailLinkCode);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateSharingEmailRequestData, PostCreateSharingEmailResponse>(
                    It.Is<PostCreateSharingEmailRequest>(r => r.Data.EmailAddress == command.EmailAddress), true), Times.Once);

            mockNotificationService.Verify(n => n.Send(It.IsAny<Notifications.Messages.Commands.SendEmailCommand>()), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateSharingEmailCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingEmailCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateSharingEmailRequestData, PostCreateSharingEmailResponse>(
                    It.IsAny<PostCreateSharingEmailRequest>(), true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateSharingEmailRequestData, PostCreateSharingEmailResponse>(
                    It.IsAny<PostCreateSharingEmailRequest>(), true), Times.Once);
        }
    }
}
