using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserAction
{
    public class WhenHandlingCreateUserActionCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Action_Is_Created_Successfully(
            CreateUserActionCommand command,
            PostCreateUserActionResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserActionCommandHandler handler)
        {
            // Arrange
            var apiResponse = new ApiResponse<PostCreateUserActionResponse>(
                apiResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateUserActionRequestData, PostCreateUserActionResponse>(
                    It.Is<PostCreateUserActionRequest>(r =>
                        r.Data.ActionType == command.ActionType &&
                        r.Data.FamilyName == command.FamilyName &&
                        r.Data.GivenNames == command.GivenNames &&
                        r.Data.CertificateId == command.CertificateId &&
                        r.Data.CertificateType == command.CertificateType &&
                        r.Data.CourseName == command.CourseName &&
                        r.PostUrl == $"api/users/{command.UserId}/actions"), true))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.ActionCode.Should().Be(apiResponseBody.ActionCode);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateUserActionRequestData, PostCreateUserActionResponse>(
                    It.Is<PostCreateUserActionRequest>(r =>
                        r.Data.ActionType == command.ActionType &&
                        r.Data.FamilyName == command.FamilyName &&
                        r.Data.GivenNames == command.GivenNames &&
                        r.Data.CertificateId == command.CertificateId &&
                        r.Data.CertificateType == command.CertificateType &&
                        r.Data.CourseName == command.CourseName &&
                        r.PostUrl == $"api/users/{command.UserId}/actions"), true), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateUserActionCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserActionCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateUserActionRequestData, PostCreateUserActionResponse>(
                    It.IsAny<PostCreateUserActionRequest>(), true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateUserActionRequestData, PostCreateUserActionResponse>(
                    It.IsAny<PostCreateUserActionRequest>(), true), Times.Once);
        }
    }
}
