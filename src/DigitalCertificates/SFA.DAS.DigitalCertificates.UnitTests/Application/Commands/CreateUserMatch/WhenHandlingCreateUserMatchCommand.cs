using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserMatch
{
    public class WhenHandlingCreateUserMatchCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Match_Attempt_Is_Forwarded_To_Inner_Api(
            CreateUserMatchCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler _sut)
        {
            // Arrange
            var dateOfBirth = new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var identityResponse = new ApiResponse<GetUserIdentityResponse>(
                new GetUserIdentityResponse
                {
                    DateOfBirth = dateOfBirth,
                    Identity = new List<IdentityName>
                    {
                        new IdentityName
                        {
                            UserIdentityId = command.UserIdentityId.GetValueOrDefault(),
                            FamilyName = "Smith",
                            ValidFrom = DateTime.UtcNow
                        }
                    }
                },
                HttpStatusCode.OK,
                string.Empty);

            var apiResponse = new ApiResponse<object>(new object(), HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUserIdentityRequest>(r =>
                        r.GetUrl == $"api/users/{command.UserId}/identity")))
                .ReturnsAsync(identityResponse);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.Is<PostCreateUserMatchRequest>(r =>
                        r.Data.Uln == command.Uln &&
                        r.Data.FamilyName == "Smith" &&
                        r.Data.DateOfBirth == dateOfBirth &&
                        r.Data.CertificateType == command.CertificateType &&
                        r.Data.CourseCode == command.CourseCode &&
                        r.Data.CourseName == command.CourseName &&
                        r.Data.CourseLevel == command.CourseLevel &&
                        r.Data.YearAwarded == command.YearAwarded &&
                        r.Data.ProviderName == command.ProviderName &&
                        r.Data.Ukprn == command.Ukprn &&
                        r.Data.IsMatched == command.IsMatched &&
                        r.Data.IsFailed == command.IsFailed &&
                        r.PostUrl == $"api/users/{command.UserId}/match"), false))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await _sut.Handle(command, CancellationToken.None);

            // Assert
            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUserIdentityRequest>(r =>
                        r.GetUrl == $"api/users/{command.UserId}/identity")),
                Times.Once);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.Is<PostCreateUserMatchRequest>(r =>
                        r.Data.Uln == command.Uln &&
                        r.Data.FamilyName == "Smith" &&
                        r.Data.DateOfBirth == dateOfBirth &&
                        r.Data.CertificateType == command.CertificateType &&
                        r.Data.CourseCode == command.CourseCode &&
                        r.Data.CourseName == command.CourseName &&
                        r.Data.CourseLevel == command.CourseLevel &&
                        r.Data.YearAwarded == command.YearAwarded &&
                        r.Data.ProviderName == command.ProviderName &&
                        r.Data.Ukprn == command.Ukprn &&
                        r.Data.IsMatched == command.IsMatched &&
                        r.Data.IsFailed == command.IsFailed &&
                        r.PostUrl == $"api/users/{command.UserId}/match"), false),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_User_Identity_Is_Used_When_UserIdentityId_Is_Supplied(
            CreateUserMatchCommand command,
            Guid userIdentityId,
            string familyName,
            DateTime dateOfBirth,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler _sut)
        {
            // Arrange
            command.UserIdentityId = userIdentityId;

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(
                new GetUserIdentityResponse
                {
                    DateOfBirth = dateOfBirth,
                    Identity = new List<IdentityName>
                    {
                        new IdentityName
                        {
                            UserIdentityId = Guid.NewGuid(),
                            FamilyName = "Wrong surname",
                            ValidFrom = DateTime.UtcNow.AddDays(-1)
                        },
                        new IdentityName
                        {
                            UserIdentityId = userIdentityId,
                            FamilyName = familyName,
                            ValidFrom = DateTime.UtcNow
                        }
                    }
                },
                HttpStatusCode.OK,
                string.Empty);

            var apiResponse = new ApiResponse<object>(new object(), HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.IsAny<GetUserIdentityRequest>()))
                .ReturnsAsync(identityResponse);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.Is<PostCreateUserMatchRequest>(r =>
                        r.Data.FamilyName == familyName &&
                        r.Data.DateOfBirth == dateOfBirth &&
                        r.PostUrl == $"api/users/{command.UserId}/match"), false))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await _sut.Handle(command, CancellationToken.None);

            // Assert
            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.Is<PostCreateUserMatchRequest>(r =>
                        r.Data.FamilyName == familyName &&
                        r.Data.DateOfBirth == dateOfBirth &&
                        r.PostUrl == $"api/users/{command.UserId}/match"), false),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_When_UserIdentityId_Is_Null_The_Latest_UserIdentity_Is_Used(
            CreateUserMatchCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler sut)
        {
            command.UserIdentityId = null;

            var dateOfBirth = new DateTime(1990, 1, 1);

            var olderIdentity = new IdentityName
            {
                UserIdentityId = Guid.NewGuid(),
                FamilyName = "Old",
                GivenNames = "Old Name",
                ValidFrom = new DateTime(2020, 1, 1)
            };

            var latestIdentity = new IdentityName
            {
                UserIdentityId = Guid.NewGuid(),
                FamilyName = "Current",
                GivenNames = "Current Name",
                ValidFrom = new DateTime(2024, 1, 1)
            };

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(
                new GetUserIdentityResponse
                {
                    DateOfBirth = dateOfBirth,
                    Identity = new List<IdentityName>
                    {
                olderIdentity,
                latestIdentity
                    }
                },
                HttpStatusCode.OK,
                string.Empty);

            var apiResponse = new ApiResponse<object>(new object(), HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUserIdentityRequest>(r => r.GetUrl == $"api/users/{command.UserId}/identity")))
                .ReturnsAsync(identityResponse);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.Is<PostCreateUserMatchRequest>(r =>
                        r.Data.FamilyName == "Current" &&
                        r.Data.DateOfBirth == dateOfBirth),
                    false))
                .ReturnsAsync(apiResponse);

            var result = await sut.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Get_User_Identity_Api_Call_Fails(
            CreateUserMatchCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler _sut)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.IsAny<GetUserIdentityRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                    client.GetWithResponseCode<GetUserIdentityResponse>(
                        It.IsAny<GetUserIdentityRequest>()),
                Times.Once);

            mockDigitalCertificatesApiClient.Verify(client =>
                    client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                        It.IsAny<PostCreateUserMatchRequest>(), false),
                Times.Never);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Create_User_Match_Api_Call_Fails(
            CreateUserMatchCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler _sut)
        {
            // Arrange
            var identityResponse = new ApiResponse<GetUserIdentityResponse>(
                new GetUserIdentityResponse
                {
                    DateOfBirth = new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                    Identity = new List<IdentityName>
                    {
                        new IdentityName
                        {
                            UserIdentityId = command.UserIdentityId ?? Guid.NewGuid(),
                            FamilyName = "Smith",
                            ValidFrom = DateTime.UtcNow
                        }
                    }
                },
                HttpStatusCode.OK,
                string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.IsAny<GetUserIdentityRequest>()))
                .ReturnsAsync(identityResponse);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                    It.IsAny<PostCreateUserMatchRequest>(), false))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                    client.GetWithResponseCode<GetUserIdentityResponse>(
                        It.IsAny<GetUserIdentityRequest>()),
                Times.Once);

            mockDigitalCertificatesApiClient.Verify(client =>
                    client.PostWithResponseCode<PostCreateUserMatchRequestData, object>(
                        It.IsAny<PostCreateUserMatchRequest>(), false),
                Times.Once);
        }
    }
}