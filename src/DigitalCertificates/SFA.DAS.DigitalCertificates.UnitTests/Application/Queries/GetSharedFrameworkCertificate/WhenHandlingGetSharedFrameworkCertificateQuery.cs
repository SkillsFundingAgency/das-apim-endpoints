using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedFrameworkCertificate;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharedFrameworkCertificate
{
    public class WhenHandlingGetSharedFrameworkCertificateQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Certificate_Is_Retrieved_Successfully(
            Guid id,
            GetSharedFrameworkCertificateQuery query,
            GetFrameworkCertificateResponse responseBody,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetSharedFrameworkCertificateQueryHandler handler)
        {
            // Arrange
            query.Id = id;

            var apiResponse = new ApiResponse<GetFrameworkCertificateResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetFrameworkCertificateResponse>(It.Is<GetFrameworkCertificateRequest>(r => r.Id == id && r.IncludeLogs == false)))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.FamilyName.Should().Be(responseBody.ApprenticeSurname);
            actual.GivenNames.Should().Be(responseBody.ApprenticeForename);
            actual.CertificateReference.Should().Be(responseBody.CertificateReference);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null(
            Guid id,
            GetSharedFrameworkCertificateQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetSharedFrameworkCertificateQueryHandler handler)
        {
            // Arrange
            query.Id = id;

            var apiResponse = new ApiResponse<GetFrameworkCertificateResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetFrameworkCertificateResponse>(It.IsAny<GetFrameworkCertificateRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetSharedFrameworkCertificateQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetSharedFrameworkCertificateQueryHandler handler)
        {
            // Arrange
            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetFrameworkCertificateResponse>(It.IsAny<GetFrameworkCertificateRequest>()))
                .ThrowsAsync(new Exception("Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>();
        }
    }
}
