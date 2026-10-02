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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharing
{
    public class WhenHandlingCreateSharingCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Is_Created_Successfully(
            CreateSharingCommand command,
            CreateSharingResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingCommandHandler handler)
        {
            // Arrange
            command.CertificateType = "Standard";

            var apiResponse = new ApiResponse<CreateSharingResponse>(
                apiResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateSharingResponse>(
                    It.IsAny<PostSharingApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.UserId.Should().Be(apiResponseBody.UserId);
            actual.CertificateId.Should().Be(apiResponseBody.CertificateId);
            actual.CertificateType.Should().Be(apiResponseBody.CertificateType.ToString());
            actual.CourseName.Should().Be(apiResponseBody.CourseName);
            actual.SharingId.Should().Be(apiResponseBody.SharingId);
            actual.SharingNumber.Should().Be(apiResponseBody.SharingNumber);
            actual.CreatedAt.Should().Be(apiResponseBody.CreatedAt);
            actual.LinkCode.Should().Be(apiResponseBody.LinkCode);
            actual.ExpiryTime.Should().Be(apiResponseBody.ExpiryTime);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateSharingResponse>(
                    It.Is<PostSharingApiRequest>(r =>
                        r.PostUrl == "api/sharing" &&
                        ((CreateSharingRequest)r.Data).UserId == command.UserId &&
                        ((CreateSharingRequest)r.Data).CertificateId == command.CertificateId &&
                        ((CreateSharingRequest)r.Data).CertificateType == CertificateType.Standard &&
                        ((CreateSharingRequest)r.Data).CourseName == command.CourseName), true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_An_Invalid_Certificate_Type_Is_Rejected_Before_Calling_The_Api(
            CreateSharingCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingCommandHandler handler)
        {
            // Arrange
            command.CertificateType = "NotACertificateType";

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateSharingResponse>(It.IsAny<PostSharingApiRequest>(), It.IsAny<bool>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateSharingCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingCommandHandler handler)
        {
            // Arrange
            command.CertificateType = "Framework";

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateSharingResponse>(
                    It.IsAny<PostSharingApiRequest>(), true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<CreateSharingResponse>(
                    It.IsAny<PostSharingApiRequest>(), true), Times.Once);
        }
    }
}
