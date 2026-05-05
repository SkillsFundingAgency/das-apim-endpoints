using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
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
                    It.Is<GetUserActionsRequest>(r => r.UserId == userId)))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.UserActions.Should().HaveCount(responseBody.UserActions.Count);
            // check a couple of mappings
            var expected = responseBody.UserActions[0];
            var actualFirst = System.Linq.Enumerable.First(actual.UserActions);
            actualFirst.Id.Should().Be(expected.Id);
            actualFirst.UserId.Should().Be(expected.UserId);
            actualFirst.ActionCode.Should().Be(expected.ActionCode);
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
                .Setup(c => c.GetWithResponseCode<GetUserActionsResponse>(It.IsAny<GetUserActionsRequest>()))
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
                .Setup(c => c.GetWithResponseCode<GetUserActionsResponse>(It.IsAny<GetUserActionsRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
