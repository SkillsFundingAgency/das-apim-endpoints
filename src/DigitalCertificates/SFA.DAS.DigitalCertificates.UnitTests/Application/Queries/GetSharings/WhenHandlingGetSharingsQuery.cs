using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharings
{
    public class WhenHandlingGetSharingsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharings_Are_Retrieved_Successfully(
            Guid userId,
            GetSharingsQuery query,
            GetSharingsResponse responseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetSharingsQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;
            query.CertificateId = responseBody.CertificateId;

            var apiResponse = new ApiResponse<GetSharingsResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingsResponse>(
                    It.Is<GetSharingsRequest>(r =>
                        r.UserId == userId &&
                        r.CertificateId == responseBody.CertificateId &&
                        r.Limit == query.Limit)))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Response.Should().BeEquivalentTo(responseBody);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null_Response(
            Guid userId,
            GetSharingsQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetSharingsQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var apiResponse = new ApiResponse<GetSharingsResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingsResponse>(It.IsAny<GetSharingsRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Response.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetSharingsQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetSharingsQueryHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingsResponse>(It.IsAny<GetSharingsRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}