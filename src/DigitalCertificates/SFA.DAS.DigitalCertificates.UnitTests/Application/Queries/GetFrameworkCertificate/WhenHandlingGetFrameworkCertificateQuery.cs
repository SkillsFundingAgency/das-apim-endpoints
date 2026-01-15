using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetFrameworkCertificate
{
    public class WhenHandlingGetFrameworkCertificateQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_FrameworkLearner_Is_Retrieved_Successfully(
            Guid id,
            GetFrameworkCertificateQuery query,
            GetFrameworkCertificateResponse responseBody,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetFrameworkCertificateQueryHandler handler)
        {
            // Arrange
            query.Id = id;

            responseBody.ApprenticeSurname = "Family";
            responseBody.ApprenticeForename = "Given";
            responseBody.ApprenticeULN = 123456;
            responseBody.FrameworkName = "Framework";
            responseBody.PathwayName = "Pathway";
            responseBody.ApprenticeshipLevelName = "Level";
            responseBody.CertificationDate = DateTime.UtcNow;
            responseBody.ProviderName = "Provider";
            responseBody.EmployerName = "Employer";
            responseBody.ApprenticeStartdate = DateTime.UtcNow.AddYears(-1);
            responseBody.QualificationsAndAwardingBodies = new List<QualificationAndAwardingBody>
            {
                new QualificationAndAwardingBody { Name = "Qual1", AwardingBody = "Body1" }
            };

            var apiResponse = new ApiResponse<GetFrameworkCertificateResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetFrameworkCertificateResponse>(It.Is<GetFrameworkCertificateRequest>(r => r.Id == id)))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.FamilyName.Should().Be(responseBody.ApprenticeSurname);
            actual.GivenNames.Should().Be(responseBody.ApprenticeForename);
            actual.Uln.Should().Be(responseBody.ApprenticeULN);
            actual.CourseName.Should().Be(responseBody.FrameworkName);
            actual.CourseOption.Should().Be(responseBody.PathwayName);
            actual.CourseLevel.Should().Be(responseBody.ApprenticeshipLevelName);
            actual.DateAwarded.Should().Be(responseBody.CertificationDate);
            actual.ProviderName.Should().Be(responseBody.ProviderName);
            actual.EmployerName.Should().Be(responseBody.EmployerName);
            actual.StartDate.Should().Be(responseBody.ApprenticeStartdate);
            actual.QualificationsAndAwardingBodies.Should().NotBeNull();
            actual.QualificationsAndAwardingBodies.Should().HaveCount(1);
            actual.QualificationsAndAwardingBodies[0].Name.Should().Be("Qual1");
            actual.PrintRequestedAt.Should().Be(responseBody.PrintRequestedAt);
            actual.PrintRequestedBy.Should().Be(responseBody.PrintRequestedBy);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null(
            Guid id,
            GetFrameworkCertificateQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetFrameworkCertificateQueryHandler handler)
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
            GetFrameworkCertificateQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetFrameworkCertificateQueryHandler handler)
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
