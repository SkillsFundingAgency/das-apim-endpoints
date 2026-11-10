using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetUserActions
{
    public class WhenHandlingGetUserActionsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_UserActions_Are_Retrieved_Successfully(
            Guid userId,
            GetUserActionsQuery query,
            GetUserActionsResponse responseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetUserActionsQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var apiResponse = new ApiResponse<GetUserActionsResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionsResponse>(
                    It.Is<GetUsersByUserIdUserActionsApiRequest>(r => r.UserId == userId)))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.UserActions.Should().HaveCount(responseBody.UserActions.Count);
            var expected = responseBody.UserActions.First();
            var actualFirst = Enumerable.First(actual.UserActions);
            actualFirst.Id.Should().Be(expected.Id);
            actualFirst.UserId.Should().Be(expected.UserId);
            actualFirst.ActionCode.Should().Be(expected.ActionCode);
            actualFirst.Uln.Should().Be(expected.Uln);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Empty_UserActions_List(
            Guid userId,
            GetUserActionsQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetUserActionsQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var apiResponse = new ApiResponse<GetUserActionsResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionsResponse>(It.IsAny<GetUsersByUserIdUserActionsApiRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.UserActions.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetUserActionsQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetUserActionsQueryHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserActionsResponse>(It.IsAny<GetUsersByUserIdUserActionsApiRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
