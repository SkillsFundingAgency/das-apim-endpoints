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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharingAccess
{
    public class WhenHandlingCreateSharingAccessCommand
    {
        [Test, MoqAutoData]
        public async Task Then_Handler_Completes_When_Inner_Api_Returns_NoContent(
            CreateSharingAccessCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingAccessCommandHandler handler)
        {
            // Arrange
            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.NoContent, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostSharingSharingaccessApiRequest>(), false))
                .ReturnsAsync(apiResponse);

            // Act / Assert
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            await act.Should().NotThrowAsync<Exception>();

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<object>(
                    It.Is<PostSharingSharingaccessApiRequest>(r =>
                        r.PostUrl == "api/sharing/sharingaccess" &&
                        ((CreateSharingAccessRequest)r.Data).SharingId == command.SharingId), false), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateSharingAccessCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingAccessCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostSharingSharingaccessApiRequest>(), false))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<object>(
                    It.IsAny<PostSharingSharingaccessApiRequest>(), false), Times.Once);
        }
    }
}
