using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByCode;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharingByCode
{
    public class WhenHandlingGetSharingByCodeQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Email_Only_Returns_Email_Response(
            Guid code,
            GetSharingByCodeQuery query,
            GetSharingByEmailLinkCodeResponse emailResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetSharingByCodeQueryHandler handler)
        {
            query.Code = code;

            var apiEmailResponse = new ApiResponse<GetSharingByEmailLinkCodeResponse>(emailResponseBody, HttpStatusCode.OK, string.Empty);
            var apiLinkResponse = new ApiResponse<GetSharingByLinkCodeResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingByEmailLinkCodeResponse>(It.IsAny<GetSharingByEmailLinkCodeRequest>()))
                .ReturnsAsync(apiEmailResponse);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingByLinkCodeResponse>(It.IsAny<GetSharingByLinkCodeRequest>()))
                .ReturnsAsync(apiLinkResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Response.Should().NotBeNull();
            actual.Response.CertificateId.Should().Be(emailResponseBody.CertificateId);
            actual.Response.SharingEmailId.Should().Be(emailResponseBody.SharingEmailId);
            actual.BothFound.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_Link_Only_Returns_Link_Response(
            Guid code,
            GetSharingByCodeQuery query,
            GetSharingByLinkCodeResponse linkResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetSharingByCodeQueryHandler handler)
        {
            query.Code = code;

            var apiEmailResponse = new ApiResponse<GetSharingByEmailLinkCodeResponse>(null, HttpStatusCode.NotFound, string.Empty);
            var apiLinkResponse = new ApiResponse<GetSharingByLinkCodeResponse>(linkResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingByEmailLinkCodeResponse>(It.IsAny<GetSharingByEmailLinkCodeRequest>()))
                .ReturnsAsync(apiEmailResponse);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingByLinkCodeResponse>(It.IsAny<GetSharingByLinkCodeRequest>()))
                .ReturnsAsync(apiLinkResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Response.Should().NotBeNull();
            actual.Response.CertificateId.Should().Be(linkResponseBody.CertificateId);
            actual.Response.SharingId.Should().Be(linkResponseBody.SharingId);
            actual.BothFound.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_Neither_Returns_Null(
            Guid code,
            GetSharingByCodeQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetSharingByCodeQueryHandler handler)
        {
            query.Code = code;

            var apiEmailResponse = new ApiResponse<GetSharingByEmailLinkCodeResponse>(null, HttpStatusCode.NotFound, string.Empty);
            var apiLinkResponse = new ApiResponse<GetSharingByLinkCodeResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingByEmailLinkCodeResponse>(It.IsAny<GetSharingByEmailLinkCodeRequest>()))
                .ReturnsAsync(apiEmailResponse);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingByLinkCodeResponse>(It.IsAny<GetSharingByLinkCodeRequest>()))
                .ReturnsAsync(apiLinkResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Response.Should().BeNull();
            actual.BothFound.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_Both_Found_Sets_BothFound(
            Guid code,
            GetSharingByCodeQuery query,
            GetSharingByEmailLinkCodeResponse emailResponseBody,
            GetSharingByLinkCodeResponse linkResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetSharingByCodeQueryHandler handler)
        {
            query.Code = code;

            var apiEmailResponse = new ApiResponse<GetSharingByEmailLinkCodeResponse>(emailResponseBody, HttpStatusCode.OK, string.Empty);
            var apiLinkResponse = new ApiResponse<GetSharingByLinkCodeResponse>(linkResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingByEmailLinkCodeResponse>(It.IsAny<GetSharingByEmailLinkCodeRequest>()))
                .ReturnsAsync(apiEmailResponse);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetSharingByLinkCodeResponse>(It.IsAny<GetSharingByLinkCodeRequest>()))
                .ReturnsAsync(apiLinkResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Response.Should().NotBeNull();
            actual.BothFound.Should().BeTrue();
        }
    }
}
