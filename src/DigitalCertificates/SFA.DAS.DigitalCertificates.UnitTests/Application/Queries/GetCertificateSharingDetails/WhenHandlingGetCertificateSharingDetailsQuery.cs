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

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetCertificateSharingDetails
{
    public class WhenHandlingGetCertificateSharingDetailsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharings_Are_Retrieved_Successfully(
        Guid userId,
        GetCertificateSharingDetailsQuery query,
        GetCertificateSharingDetailsResponse responseBody,
        [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
        GetCertificateSharingDetailsQueryHandler handler)
        {
            query.UserId = userId;
            query.CertificateId = responseBody.CertificateId;

            var apiResponse = new ApiResponse<GetCertificateSharingDetailsResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
            .Setup(c => c.GetWithResponseCode<GetCertificateSharingDetailsResponse>(
            It.Is<GetCertificateSharingDetailsRequest>(r => r.UserId == userId && r.CertificateId == responseBody.CertificateId && r.Limit == query.Limit)))
            .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Response.Should().BeEquivalentTo(responseBody);
        }

        [Test, MoqAutoData]
        public void Then_NotFound_Throws_ApiResponseException(
        Guid userId,
        GetCertificateSharingDetailsQuery query,
        [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
        GetCertificateSharingDetailsQueryHandler handler)
        {
            query.UserId = userId;

            var apiResponse = new ApiResponse<GetCertificateSharingDetailsResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
            .Setup(c => c.GetWithResponseCode<GetCertificateSharingDetailsResponse>(It.IsAny<GetCertificateSharingDetailsRequest>()))
            .ReturnsAsync(apiResponse);

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().ThrowAsync<ApiResponseException>()
            .Where(e => e.Status == HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
        GetCertificateSharingDetailsQuery query,
        [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
        GetCertificateSharingDetailsQueryHandler handler)
        {
            mockDigitalCertificatesApiClient
            .Setup(c => c.GetWithResponseCode<GetCertificateSharingDetailsResponse>(It.IsAny<GetCertificateSharingDetailsRequest>()))
            .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
            .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
