using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharingEmailAccess
{
    public class WhenHandlingCreateSharingEmailAccessCommand
    {
        [Test, MoqAutoData]
        public async Task Then_Handler_Completes_When_Inner_Api_Returns_NoContent(
            CreateSharingEmailAccessCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingEmailAccessCommandHandler handler)
        {
            // Arrange
            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.NoContent, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateSharingEmailAccessRequestData, object>(
                    It.Is<PostCreateSharingEmailAccessRequest>(r => r.Data.SharingEmailId == command.SharingEmailId), false))
                .ReturnsAsync(apiResponse);

            // Act / Assert 
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            await act.Should().NotThrowAsync<Exception>();

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateSharingEmailAccessRequestData, object>(
                    It.Is<PostCreateSharingEmailAccessRequest>(r => r.Data.SharingEmailId == command.SharingEmailId), false), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
        CreateSharingEmailAccessCommand command,
        [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
        CreateSharingEmailAccessCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateSharingEmailAccessRequestData, object>(
                    It.IsAny<PostCreateSharingEmailAccessRequest>(), false))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateSharingEmailAccessRequestData, object>(
                    It.IsAny<PostCreateSharingEmailAccessRequest>(), false), Times.Once);
        }
    }
}
