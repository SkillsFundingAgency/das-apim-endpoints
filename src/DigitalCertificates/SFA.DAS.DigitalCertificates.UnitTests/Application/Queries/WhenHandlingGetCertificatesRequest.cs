using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificates;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries
{
    public class WhenHandlingGetCertificatesRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Authorisation_And_Certificates_Are_Retrieved_Successfully(
            Guid userId,
            GetCertificatesQuery query,
            GetAuthorisationResponse authorisationResponseBody,
            GetCertificatesResponse certificatesResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var authorisationResponse = new ApiResponse<GetAuthorisationResponse>(
                authorisationResponseBody, HttpStatusCode.OK, string.Empty);

            var certificatesResponse = new ApiResponse<GetCertificatesResponse>(
                certificatesResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetAuthorisationResponse>(
                    It.Is<GetAuthorisationRequest>(r => r.UserId == userId)))
                .ReturnsAsync(authorisationResponse);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificatesResponse>(
                    It.Is<GetCertificatesRequest>(r => r.Uln == authorisationResponseBody.Authorisation.Uln)))
                .ReturnsAsync(certificatesResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Authorisation.Should().BeEquivalentTo(authorisationResponseBody.Authorisation);
            actual.Certificates.Should().BeEquivalentTo(certificatesResponseBody.Certificates);
        }

        [Test, MoqAutoData]
        public async Task Then_Empty_Result_Returned_If_Authorisation_Not_Found(
            Guid userId,
            GetCertificatesQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var authorisationResponse = new ApiResponse<GetAuthorisationResponse>(
                null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetAuthorisationResponse>(
                    It.IsAny<GetAuthorisationRequest>()))
                .ReturnsAsync(authorisationResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Authorisation.Should().BeNull();
            actual.Certificates.Should().BeEmpty();
            mockAssessorsApiClient.Verify(
                x => x.GetWithResponseCode<GetCertificatesResponse>(It.IsAny<GetCertificatesRequest>()),
                Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Empty_Certificates_Returned_If_Certificates_Not_Found(
            Guid userId,
            GetCertificatesQuery query,
            GetAuthorisationResponse authorisationResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var authorisationResponse = new ApiResponse<GetAuthorisationResponse>(
                authorisationResponseBody, HttpStatusCode.OK, string.Empty);

            var certificatesResponse = new ApiResponse<GetCertificatesResponse>(
                null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetAuthorisationResponse>(
                    It.Is<GetAuthorisationRequest>(r => r.UserId == userId)))
                .ReturnsAsync(authorisationResponse);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificatesResponse>(
                    It.Is<GetCertificatesRequest>(r => r.Uln == authorisationResponseBody.Authorisation.Uln)))
                .ReturnsAsync(certificatesResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Authorisation.Should().BeEquivalentTo(authorisationResponseBody.Authorisation);
            actual.Certificates.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetCertificatesQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesQueryHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetAuthorisationResponse>(It.IsAny<GetAuthorisationRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
