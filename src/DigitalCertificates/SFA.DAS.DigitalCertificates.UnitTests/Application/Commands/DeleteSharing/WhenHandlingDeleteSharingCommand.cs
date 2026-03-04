using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.DeleteSharing
{
    public class WhenHandlingDeleteSharingCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Is_Deleted_Successfully(
            DeleteSharingCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            DeleteSharingCommandHandler handler)
        {
            // Arrange
            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.NoContent, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.DeleteWithResponseCode<object>(
                    It.Is<DeleteSharingRequest>(r => r.SharingId == command.SharingId), It.IsAny<bool>()))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            mockDigitalCertificatesApiClient.Verify(client =>
                client.DeleteWithResponseCode<object>(
                    It.Is<DeleteSharingRequest>(r => r.SharingId == command.SharingId), It.IsAny<bool>()), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            DeleteSharingCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            DeleteSharingCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.DeleteWithResponseCode<object>(
                    It.IsAny<DeleteSharingRequest>(), It.IsAny<bool>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.DeleteWithResponseCode<object>(It.IsAny<DeleteSharingRequest>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
