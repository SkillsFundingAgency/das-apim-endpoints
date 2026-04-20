using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetCertificatesMatch
{
    public class WhenHandlingGetCertificatesMatchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Null_If_User_Is_Already_Authorised(
            Guid userId,
            GetCertificatesMatchQuery query,
            GetUserIdentityResponse identityBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesMatchQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;
            identityBody.Authorisation = new IdentityAuthorisation { AuthorisationId = Guid.NewGuid(), Uln = 1234567890 };

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(identityBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUserIdentityRequest>(r => r.UserId == userId)))
                .ReturnsAsync(identityResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().BeNull();
            mockAssessorsApiClient.Verify(
                c => c.GetWithResponseCode<GetCertificateSearchResponse>(It.IsAny<GetCertificateSearchRequest>()),
                Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Empty_Result_If_Identity_Not_Found(
            Guid userId,
            GetCertificatesMatchQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesMatchQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserIdentityResponse>(It.IsAny<GetUserIdentityRequest>()))
                .ReturnsAsync(identityResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.Matches.Should().BeEmpty();
            actual.Masks.Should().BeEmpty();
            mockAssessorsApiClient.Verify(
                c => c.GetWithResponseCode<GetCertificateSearchResponse>(It.IsAny<GetCertificateSearchRequest>()),
                Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Empty_Result_If_No_Certificate_Matches_Found(
            Guid userId,
            GetCertificatesMatchQuery query,
            GetUserIdentityResponse identityBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesMatchQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;
            identityBody.Authorisation = null;
            identityBody.DateOfBirth = new DateTime(1990, 1, 1);
            identityBody.Identity = [new IdentityName { FamilyName = "Smith", GivenNames = "John" }];
            identityBody.Excluded = [];

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(identityBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUserIdentityRequest>(r => r.UserId == userId)))
                .ReturnsAsync(identityResponse);

            var searchResponse = new ApiResponse<GetCertificateSearchResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateSearchResponse>(It.IsAny<GetCertificateSearchRequest>()))
                .ReturnsAsync(searchResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.Matches.Should().BeEmpty();
            actual.Masks.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Matches_And_Standard_Masks_When_Standard_Certificates_Found(
            Guid userId,
            GetCertificatesMatchQuery query,
            GetCertificateMasksResponse masksBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesMatchQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var identityBody = new GetUserIdentityResponse
            {
                Authorisation = null,
                DateOfBirth = new DateTime(1990, 1, 1),
                Identity = [new IdentityName { FamilyName = "Smith", GivenNames = "John" }],
                Excluded = []
            };

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(identityBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUserIdentityRequest>(r => r.UserId == userId)))
                .ReturnsAsync(identityResponse);

            var searchBody = new GetCertificateSearchResponse
            {
                Matches =
                [
                    new CertificateSearchMatch { Uln = 1000000001, CertificateType = "Standard", CourseName = "Course A", CourseCode = "ABC", CourseLevel = "3", ProviderName = "Provider X", Ukprn = 10000001 }
                ]
            };

            var searchResponse = new ApiResponse<GetCertificateSearchResponse>(searchBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateSearchResponse>(It.IsAny<GetCertificateSearchRequest>()))
                .ReturnsAsync(searchResponse);

            masksBody.Masks = masksBody.Masks.Take(3).ToList();
            foreach (var mask in masksBody.Masks) mask.CertificateType = "Standard";

            var masksResponse = new ApiResponse<GetCertificateMasksResponse>(masksBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateMasksResponse>(It.IsAny<GetStandardCertificateMasksRequest>()))
                .ReturnsAsync(masksResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.Matches.Should().BeEquivalentTo(searchBody.Matches.Select(m => (CertificateMatchResult)m));
            actual.Masks.Should().BeEquivalentTo(masksBody.Masks.Select(m => (CertificateMaskResult)m));
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Matches_And_Framework_Masks_When_Framework_Certificates_Found(
            Guid userId,
            GetCertificatesMatchQuery query,
            GetCertificateMasksResponse masksBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesMatchQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var identityBody = new GetUserIdentityResponse
            {
                Authorisation = null,
                DateOfBirth = new DateTime(1990, 1, 1),
                Identity = [new IdentityName { FamilyName = "Jones", GivenNames = "Jane" }],
                Excluded = []
            };

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(identityBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUserIdentityRequest>(r => r.UserId == userId)))
                .ReturnsAsync(identityResponse);

            var searchBody = new GetCertificateSearchResponse
            {
                Matches =
                [
                    new CertificateSearchMatch { Uln = 1000000002, CertificateType = "Framework", CourseName = "Framework B", CourseCode = "FWK", CourseLevel = "2", ProviderName = "Provider Y", Ukprn = 10000002 }
                ]
            };

            var searchResponse = new ApiResponse<GetCertificateSearchResponse>(searchBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateSearchResponse>(It.IsAny<GetCertificateSearchRequest>()))
                .ReturnsAsync(searchResponse);

            masksBody.Masks = masksBody.Masks.Take(3).ToList();
            foreach (var mask in masksBody.Masks) mask.CertificateType = "Framework";

            var frameworkMasksResponse = new ApiResponse<GetCertificateMasksResponse>(masksBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateMasksResponse>(It.IsAny<GetFrameworkCertificateMasksRequest>()))
                .ReturnsAsync(frameworkMasksResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.Matches.Should().BeEquivalentTo(searchBody.Matches.Select(m => (CertificateMatchResult)m));
            actual.Masks.Should().BeEquivalentTo(masksBody.Masks.Select(m => (CertificateMaskResult)m));
        }

        [Test, MoqAutoData]
        public async Task Then_Masks_Are_Capped_At_Five_When_Both_Standard_And_Framework_Found(
            Guid userId,
            GetCertificatesMatchQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCertificatesMatchQueryHandler handler)
        {
            // Arrange
            query.UserId = userId;

            var identityBody = new GetUserIdentityResponse
            {
                Authorisation = null,
                DateOfBirth = new DateTime(1990, 1, 1),
                Identity = [new IdentityName { FamilyName = "Taylor", GivenNames = "Alex" }],
                Excluded = []
            };

            var identityResponse = new ApiResponse<GetUserIdentityResponse>(identityBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(c => c.GetWithResponseCode<GetUserIdentityResponse>(
                    It.Is<GetUserIdentityRequest>(r => r.UserId == userId)))
                .ReturnsAsync(identityResponse);

            var searchBody = new GetCertificateSearchResponse
            {
                Matches =
                [
                    new CertificateSearchMatch { Uln = 1000000003, CertificateType = "Standard" },
                    new CertificateSearchMatch { Uln = 1000000004, CertificateType = "Framework" }
                ]
            };

            var searchResponse = new ApiResponse<GetCertificateSearchResponse>(searchBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateSearchResponse>(It.IsAny<GetCertificateSearchRequest>()))
                .ReturnsAsync(searchResponse);

            var standardMasksBody = new GetCertificateMasksResponse
            {
                Masks = Enumerable.Range(1, 3).Select(i => new CertificateMask { CertificateType = "Standard", CourseName = $"Standard {i}" }).ToList()
            };
            var standardMasksResponse = new ApiResponse<GetCertificateMasksResponse>(standardMasksBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateMasksResponse>(It.IsAny<GetStandardCertificateMasksRequest>()))
                .ReturnsAsync(standardMasksResponse);

            var frameworkMasksBody = new GetCertificateMasksResponse
            {
                Masks = Enumerable.Range(1, 3).Select(i => new CertificateMask { CertificateType = "Framework", CourseName = $"Framework {i}" }).ToList()
            };
            var frameworkMasksResponse = new ApiResponse<GetCertificateMasksResponse>(frameworkMasksBody, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetCertificateMasksResponse>(It.IsAny<GetFrameworkCertificateMasksRequest>()))
                .ReturnsAsync(frameworkMasksResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Should().NotBeNull();
            actual.Masks.Should().HaveCount(5);
        }
    }
}
