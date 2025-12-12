using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharing
{
    public class WhenHandlingCreateSharingCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Is_Created_Successfully(
            CreateSharingCommand command,
            PostCreateSharingResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingCommandHandler handler)
        {
            // Arrange
            var apiResponse = new ApiResponse<PostCreateSharingResponse>(
                apiResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateSharingRequestData, PostCreateSharingResponse>(
                    It.Is<PostCreateSharingRequest>(r =>
                        r.Data.UserId == command.UserId &&
                        r.Data.CertificateId == command.CertificateId &&
                        r.Data.CertificateType == command.CertificateType &&
                        r.Data.CourseName == command.CourseName), true))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.UserId.Should().Be(apiResponseBody.UserId);
            actual.CertificateId.Should().Be(apiResponseBody.CertificateId);
            actual.CertificateType.Should().Be(apiResponseBody.CertificateType);
            actual.CourseName.Should().Be(apiResponseBody.CourseName);
            actual.SharingId.Should().Be(apiResponseBody.SharingId);
            actual.SharingNumber.Should().Be(apiResponseBody.SharingNumber);
            actual.CreatedAt.Should().Be(apiResponseBody.CreatedAt);
            actual.LinkCode.Should().Be(apiResponseBody.LinkCode);
            actual.ExpiryTime.Should().Be(apiResponseBody.ExpiryTime);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateSharingRequestData, PostCreateSharingResponse>(
                    It.Is<PostCreateSharingRequest>(r =>
                        r.Data.UserId == command.UserId &&
                        r.Data.CertificateId == command.CertificateId &&
                        r.Data.CertificateType == command.CertificateType &&
                        r.Data.CourseName == command.CourseName), true), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateSharingCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateSharingCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateSharingRequestData, PostCreateSharingResponse>(
                    It.IsAny<PostCreateSharingRequest>(), true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateSharingRequestData, PostCreateSharingResponse>(
                    It.IsAny<PostCreateSharingRequest>(), true), Times.Once);
        }
    }
}