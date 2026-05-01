using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using MediatR;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserMatch
{
    public class WhenHandlingCreateUserMatchCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Match_Attempt_Is_Forwarded_To_Inner_Api(
            CreateUserMatchCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler handler)
        {
            // Arrange
            var apiResponse = new ApiResponse<object>(new object(), HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.Is<PostCreateUserMatchRequest>(r =>
                        r.Data.Uln == command.Uln &&
                        r.Data.FamilyName == command.FamilyName &&
                        r.Data.DateOfBirth == command.DateOfBirth &&
                        r.Data.CertificateType == command.CertificateType &&
                        r.Data.CourseCode == command.CourseCode &&
                        r.Data.CourseName == command.CourseName &&
                        r.Data.CourseLevel == command.CourseLevel &&
                        r.Data.DateAwarded == command.DateAwarded &&
                        r.Data.ProviderName == command.ProviderName &&
                        r.Data.Ukprn == command.Ukprn &&
                        r.Data.IsMatched == command.IsMatched &&
                        r.Data.IsFailed == command.IsFailed &&
                        r.PostUrl == $"api/users/{command.UserId}/match"), false))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.Is<PostCreateUserMatchRequest>(r =>
                        r.Data.Uln == command.Uln &&
                        r.Data.FamilyName == command.FamilyName &&
                        r.Data.DateOfBirth == command.DateOfBirth &&
                        r.Data.CertificateType == command.CertificateType &&
                        r.Data.CourseCode == command.CourseCode &&
                        r.Data.CourseName == command.CourseName &&
                        r.Data.CourseLevel == command.CourseLevel &&
                        r.Data.DateAwarded == command.DateAwarded &&
                        r.Data.ProviderName == command.ProviderName &&
                        r.Data.Ukprn == command.Ukprn &&
                        r.Data.IsMatched == command.IsMatched &&
                        r.Data.IsFailed == command.IsFailed &&
                        r.PostUrl == $"api/users/{command.UserId}/match"), false), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateUserMatchCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.IsAny<PostCreateUserMatchRequest>(), false))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(It.IsAny<PostCreateUserMatchRequest>(), false), Times.Once);
        }
    }
}
