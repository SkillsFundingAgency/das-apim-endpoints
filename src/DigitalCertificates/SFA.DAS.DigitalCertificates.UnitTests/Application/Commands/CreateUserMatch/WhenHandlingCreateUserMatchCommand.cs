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
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
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
            command.CertificateType = "Standard";

            var dateOfBirth = new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var identityResponse = new ApiResponse<GetUserIdentityResponse>(
                new GetUserIdentityResponse
                {
                    DateOfBirth = dateOfBirth,
                    Identity = new List<IdentityNameDto>
                    {
                        new IdentityNameDto
                        {
                            UserIdentityId = command.UserIdentityId.GetValueOrDefault(),
                            FamilyName = "Smith",
                            ValidSince = DateTime.UtcNow
                        }
                    }
                },
                HttpStatusCode.OK,
                string.Empty);

            var apiResponse = new ApiResponse<object>(new object(), HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUsersByUserIdIdentityApiRequest>(r =>
                        r.GetUrl == $"api/users/{command.UserId}/identity")))
                .ReturnsAsync(identityResponse);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostUsersByUserIdMatchApiRequest>(), false))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await _sut.Handle(command, CancellationToken.None);

            // Assert
            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUsersByUserIdIdentityApiRequest>(r =>
                        r.GetUrl == $"api/users/{command.UserId}/identity")),
                Times.Once);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<object>(
                    It.Is<PostUsersByUserIdMatchApiRequest>(r =>
                        ((CreateUserMatchRequest)r.Data).Uln == command.Uln &&
                        ((CreateUserMatchRequest)r.Data).FamilyName == "Smith" &&
                        ((CreateUserMatchRequest)r.Data).DateOfBirth == dateOfBirth &&
                        ((CreateUserMatchRequest)r.Data).CertificateType == CertificateType.Standard &&
                        ((CreateUserMatchRequest)r.Data).CourseCode == command.CourseCode &&
                        ((CreateUserMatchRequest)r.Data).CourseName == command.CourseName &&
                        ((CreateUserMatchRequest)r.Data).CourseLevel == command.CourseLevel &&
                        ((CreateUserMatchRequest)r.Data).YearAwarded == command.YearAwarded &&
                        ((CreateUserMatchRequest)r.Data).ProviderName == command.ProviderName &&
                        ((CreateUserMatchRequest)r.Data).Ukprn == command.Ukprn &&
                        ((CreateUserMatchRequest)r.Data).IsMatched == command.IsMatched &&
                        ((CreateUserMatchRequest)r.Data).IsFailed == command.IsFailed &&
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
            command.CertificateType = "Framework";

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(
                new GetUserIdentityResponse
                {
                    DateOfBirth = dateOfBirth,
                    Identity = new List<IdentityNameDto>
                    {
                        new IdentityNameDto
                        {
                            UserIdentityId = Guid.NewGuid(),
                            FamilyName = "Wrong surname",
                            ValidSince = DateTime.UtcNow.AddDays(-1)
                        },
                        new IdentityNameDto
                        {
                            UserIdentityId = userIdentityId,
                            FamilyName = familyName,
                            ValidSince = DateTime.UtcNow
                        }
                    }
                },
                HttpStatusCode.OK,
                string.Empty);

            var apiResponse = new ApiResponse<object>(new object(), HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.IsAny<GetUsersByUserIdIdentityApiRequest>()))
                .ReturnsAsync(identityResponse);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostUsersByUserIdMatchApiRequest>(), false))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await _sut.Handle(command, CancellationToken.None);

            // Assert
            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<object>(
                    It.Is<PostUsersByUserIdMatchApiRequest>(r =>
                        ((CreateUserMatchRequest)r.Data).FamilyName == familyName &&
                        ((CreateUserMatchRequest)r.Data).DateOfBirth == dateOfBirth &&
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
            command.CertificateType = "Standard";

            var dateOfBirth = new DateTime(1990, 1, 1);

            var olderIdentity = new IdentityNameDto
            {
                UserIdentityId = Guid.NewGuid(),
                FamilyName = "Old",
                GivenNames = "Old Name",
                ValidSince = new DateTime(2020, 1, 1)
            };

            var latestIdentity = new IdentityNameDto
            {
                UserIdentityId = Guid.NewGuid(),
                FamilyName = "Current",
                GivenNames = "Current Name",
                ValidSince = new DateTime(2024, 1, 1)
            };

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(
                new GetUserIdentityResponse
                {
                    DateOfBirth = dateOfBirth,
                    Identity = new List<IdentityNameDto>
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
                    It.Is<GetUsersByUserIdIdentityApiRequest>(r => r.GetUrl == $"api/users/{command.UserId}/identity")))
                .ReturnsAsync(identityResponse);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostUsersByUserIdMatchApiRequest>(), false))
                .ReturnsAsync(apiResponse);

            var result = await sut.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<object>(
                    It.Is<PostUsersByUserIdMatchApiRequest>(r =>
                        ((CreateUserMatchRequest)r.Data).FamilyName == "Current" &&
                        ((CreateUserMatchRequest)r.Data).DateOfBirth == dateOfBirth), false),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Get_User_Identity_Api_Call_Fails(
            CreateUserMatchCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler _sut)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.IsAny<GetUsersByUserIdIdentityApiRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                    client.GetWithResponseCode<GetUserIdentityResponse>(
                        It.IsAny<GetUsersByUserIdIdentityApiRequest>()),
                Times.Once);

            mockDigitalCertificatesApiClient.Verify(client =>
                    client.PostWithResponseCode<object>(
                        It.IsAny<PostUsersByUserIdMatchApiRequest>(), It.IsAny<bool>()),
                Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Create_User_Match_Api_Call_Fails(
            CreateUserMatchCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserMatchCommandHandler _sut)
        {
            // Arrange
            command.CertificateType = "Standard";

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(
                new GetUserIdentityResponse
                {
                    DateOfBirth = new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                    Identity = new List<IdentityNameDto>
                    {
                        new IdentityNameDto
                        {
                            UserIdentityId = command.UserIdentityId ?? Guid.NewGuid(),
                            FamilyName = "Smith",
                            ValidSince = DateTime.UtcNow
                        }
                    }
                },
                HttpStatusCode.OK,
                string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserIdentityResponse>(
                    It.IsAny<GetUsersByUserIdIdentityApiRequest>()))
                .ReturnsAsync(identityResponse);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostUsersByUserIdMatchApiRequest>(), false))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                    client.GetWithResponseCode<GetUserIdentityResponse>(
                        It.IsAny<GetUsersByUserIdIdentityApiRequest>()),
                Times.Once);

            mockDigitalCertificatesApiClient.Verify(client =>
                    client.PostWithResponseCode<object>(
                        It.IsAny<PostUsersByUserIdMatchApiRequest>(), false),
                Times.Once);
        }
    }
}
