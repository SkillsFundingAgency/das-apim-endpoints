using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedStandardCertificate;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharedStandardCertificate
{
    public class WhenHandlingGetSharedStandardCertificateQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Certificate_Is_Retrieved_Successfully(
            Guid id,
            GetSharedStandardCertificateQuery query,
            GetStandardCertificateResponse responseBody,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetSharedStandardCertificateQueryHandler handler)
        {
            // Arrange
            query.Id = id;

            var apiResponse = new ApiResponse<GetStandardCertificateResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetStandardCertificateResponse>(It.Is<GetStandardCertificateRequest>(r => r.Id == id && r.IncludeLogs == false)))
                .ReturnsAsync(apiResponse);


            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.FamilyName.Should().Be(responseBody.LearnerFamilyName);
            actual.GivenNames.Should().Be(responseBody.LearnerGivenNames);
            // basic mappings
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null(
            Guid id,
            GetSharedStandardCertificateQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetSharedStandardCertificateQueryHandler handler)
        {
            // Arrange
            query.Id = id;

            var apiResponse = new ApiResponse<GetStandardCertificateResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetStandardCertificateResponse>(It.IsAny<GetStandardCertificateRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetSharedStandardCertificateQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetSharedStandardCertificateQueryHandler handler)
        {
            // Arrange
            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetStandardCertificateResponse>(It.IsAny<GetStandardCertificateRequest>()))
                .ThrowsAsync(new Exception("Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>();
        }
    }
}
