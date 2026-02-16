using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharingById
{
    public class WhenHandlingGetSharingByIdQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Is_Retrieved_Successfully(
        Guid sharingId,
        GetSharingByIdQuery query,
        GetSharingByIdResponse responseBody,
        [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
        GetSharingByIdQueryHandler handler)
        {
            // Arrange
            query.SharingId = sharingId;
            query.Limit = 5;

            var apiResponse = new ApiResponse<GetSharingByIdResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
            .Setup(c => c.GetWithResponseCode<GetSharingByIdResponse>(
            It.Is<GetSharingByIdRequest>(r => r.SharingId == sharingId && r.Limit == query.Limit)))
            .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Response.Should().BeEquivalentTo(responseBody);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null(
        Guid sharingId,
        GetSharingByIdQuery query,
        [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
        GetSharingByIdQueryHandler handler)
        {
            // Arrange
            query.SharingId = sharingId;

            var apiResponse = new ApiResponse<GetSharingByIdResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
            .Setup(c => c.GetWithResponseCode<GetSharingByIdResponse>(It.IsAny<GetSharingByIdRequest>()))
            .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Response.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
        GetSharingByIdQuery query,
        [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
        GetSharingByIdQueryHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
            .Setup(c => c.GetWithResponseCode<GetSharingByIdResponse>(It.IsAny<GetSharingByIdRequest>()))
            .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ApiResponseException>()
            .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
