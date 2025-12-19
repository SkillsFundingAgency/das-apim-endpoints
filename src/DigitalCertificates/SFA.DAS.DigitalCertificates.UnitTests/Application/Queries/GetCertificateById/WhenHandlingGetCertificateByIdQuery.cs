using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateById;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetCertificateById
{
    public class WhenHandlingGetCertificateByIdQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Certificate_Is_Retrieved_Successfully(
            Guid id,
            GetCertificateByIdQuery query,
            GetCertificateByIdResponse responseBody,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificateByIdQueryHandler handler)
        {
            // Arrange
            query.Id = id;

            responseBody.OrganisationId = Guid.NewGuid();

            var apiResponse = new ApiResponse<GetCertificateByIdResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateByIdResponse>(It.Is<GetCertificateByIdRequest>(r => r.Id == id && r.IncludeLogs)))
                .ReturnsAsync(apiResponse);

            var orgResponseBody = new GetOrganisationResponse { EndPointAssessorName = "Test Assessor" };
            var orgApiResponse = new ApiResponse<GetOrganisationResponse>(orgResponseBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetOrganisationResponse>(It.Is<GetOrganisationRequest>(r => r.OrganisationId == responseBody.OrganisationId)))
                .ReturnsAsync(orgApiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.Uln.Should().Be(responseBody.Uln.ToString());

            actual.FamilyName.Should().Be(responseBody.LearnerFamilyName);
            actual.GivenNames.Should().Be(responseBody.LearnerGivenNames);

            actual.AssessorName.Should().Be(orgResponseBody.EndPointAssessorName);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null(
            Guid id,
            GetCertificateByIdQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificateByIdQueryHandler handler)
        {
            // Arrange
            query.Id = id;

            var apiResponse = new ApiResponse<GetCertificateByIdResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateByIdResponse>(It.IsAny<GetCertificateByIdRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetCertificateByIdQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificateByIdQueryHandler handler)
        {
            // Arrange
            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateByIdResponse>(It.IsAny<GetCertificateByIdRequest>()))
                .ThrowsAsync(new Exception("Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>();
        }
    }
}
