using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Admin.Application.Queries.GetUserActionByCode;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using GetUserActionByCodeResponse = SFA.DAS.DigitalCertificates.Contracts.ApiResponses.GetUserActionByCodeQueryResult;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Admin.UnitTests.Application.Queries.GetUserActionByCode
{
    public class WhenHandlingGetUserActionByCodeQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_UserAction_Is_Retrieved_Successfully(
            string code,
            GetUserActionByCodeQuery query,
            GetUserActionByCodeResponse responseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetUserActionByCodeQueryHandler handler)
        {
            // Arrange
            query.Code = code;

            var apiResponse = new ApiResponse<GetUserActionByCodeResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(
                    It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            var expected = (GetUserActionByCodeQueryResult)responseBody;

            actual.Should().NotBeNull();
            actual.Id.Should().Be(expected.Id);
            actual.UserId.Should().Be(expected.UserId);
            actual.ActionType.Should().Be(expected.ActionType);
            actual.ActionTime.Should().Be(expected.ActionTime);
            actual.ActionStatus.Should().Be(expected.ActionStatus);
            actual.Uln.Should().Be(expected.Uln);
            actual.FamilyName.Should().Be(expected.FamilyName);
            actual.GivenNames.Should().Be(expected.GivenNames);
            actual.CertificateId.Should().Be(expected.CertificateId);
            actual.CertificateType.Should().Be(expected.CertificateType);
            actual.CourseName.Should().Be(expected.CourseName);
            actual.AdminActions.Should().HaveCount(expected.AdminActions.Count);

            var expectedAdminAction = expected.AdminActions.First();
            var actualAdminAction = actual.AdminActions.First();
            actualAdminAction.Username.Should().Be(expectedAdminAction.Username);
            actualAdminAction.ActionTime.Should().Be(expectedAdminAction.ActionTime);
            actualAdminAction.Action.Should().Be(expectedAdminAction.Action);

            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(
                It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null(
            string code,
            GetUserActionByCodeQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetUserActionByCodeQueryHandler handler)
        {
            // Arrange
            query.Code = code;

            var apiResponse = new ApiResponse<GetUserActionByCodeResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.IsAny<GetUsersUseractionsByCodeApiRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().BeNull();

            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(
                It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Null_Response_Body_Returns_Null(
            string code,
            GetUserActionByCodeQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetUserActionByCodeQueryHandler handler)
        {
            // Arrange
            query.Code = code;

            var apiResponse = new ApiResponse<GetUserActionByCodeResponse>(null, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.IsAny<GetUsersUseractionsByCodeApiRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().BeNull();

            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(
                It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetUserActionByCodeQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetUserActionByCodeQueryHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.IsAny<GetUsersUseractionsByCodeApiRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(
                It.IsAny<GetUsersUseractionsByCodeApiRequest>()), Times.Once);
        }
    }
}
