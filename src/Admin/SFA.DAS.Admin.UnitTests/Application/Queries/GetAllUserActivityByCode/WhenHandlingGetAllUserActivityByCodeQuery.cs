using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Apim.Shared.Exceptions;
using GetUserActionByCodeResponse = SFA.DAS.DigitalCertificates.Contracts.ApiResponses.GetUserActionByCodeQueryResult;
using GetUserActionsResponse = SFA.DAS.DigitalCertificates.Contracts.ApiResponses.GetUserActionsQueryResult;
using GetUserByIdResponse = SFA.DAS.DigitalCertificates.Contracts.ApiResponses.GetUserByIdQueryResult;

namespace SFA.DAS.Admin.UnitTests.Application.Queries.GetAllUserActivityByCode
{
    public class WhenHandlingGetAllUserActivityByCodeQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_UserActivity_Is_Retrieved_Successfully(
            string code,
            GetAllUserActivityByCodeQuery query,
            GetUserActionByCodeResponse codeResponseBody,
            GetUserActionsResponse userActionsBody,
            GetUserByIdResponse userDetailsBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetAllUserActivityByCodeQueryHandler handler)
        {
            // Arrange
            query.Code = code;

            var codeApiResponse = new ApiResponse<GetUserActionByCodeResponse>(codeResponseBody, HttpStatusCode.OK, string.Empty);
            var actionsApiResponse = new ApiResponse<GetUserActionsResponse>(userActionsBody, HttpStatusCode.OK, string.Empty);
            var detailsApiResponse = new ApiResponse<GetUserByIdResponse>(userDetailsBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)))
                .ReturnsAsync(codeApiResponse);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionsResponse>(It.Is<GetUsersByUserIdActionsApiRequest>(r => r.UserId == codeResponseBody.UserId)))
                .ReturnsAsync(actionsApiResponse);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserByIdResponse>(It.Is<GetUsersIdByUserIdApiRequest>(r => r.UserId == codeResponseBody.UserId)))
                .ReturnsAsync(detailsApiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.UserId.Should().Be(userDetailsBody.UserId);
            actual.GovUKIdentifier.Should().Be(userDetailsBody.GovUkIdentifier);
            actual.EmailAddress.Should().Be(userDetailsBody.EmailAddress);
            actual.UserActions.Should().HaveCount(userActionsBody.UserActions?.Count ?? 0);

            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)), Times.Once);
            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionsResponse>(It.Is<GetUsersByUserIdActionsApiRequest>(r => r.UserId == codeResponseBody.UserId)), Times.Once);
            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserByIdResponse>(It.Is<GetUsersIdByUserIdApiRequest>(r => r.UserId == codeResponseBody.UserId)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null(
            string code,
            GetAllUserActivityByCodeQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetAllUserActivityByCodeQueryHandler handler)
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

            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.Is<GetUsersUseractionsByCodeApiRequest>(r => r.Code == code)), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Any_Api_Call_Fails(
            GetAllUserActivityByCodeQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetAllUserActivityByCodeQueryHandler handler)
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

            mockDigitalCertificatesApiClient.Verify(c => c.GetWithResponseCode<GetUserActionByCodeResponse>(It.IsAny<GetUsersUseractionsByCodeApiRequest>()), Times.Once);
        }
    }
}
