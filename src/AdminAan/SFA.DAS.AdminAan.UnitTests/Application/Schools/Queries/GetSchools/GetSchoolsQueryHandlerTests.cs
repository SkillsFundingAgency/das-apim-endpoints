using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.Schools.Queries;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.Schools.Queries.GetSchools;
public class GetSchoolsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsSchools(
        [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> apiClient,
        GetSchoolsQueryHandler sut,
        GetSchoolsQuery query)
    {
        // Arrange
        var searchTerm = "dee";
        query.SearchTerm = searchTerm;

        var schools = new List<EducationalOrganisation>
        {
            new() { Name = "Deeside school", URN = "123456" },
            new() { Name = "Deer school", URN = "123457" },
            new() { Name = "Dee school child centre", URN = "12345" },
            new() { Name = "Deering school", URN = "123458" }
        };
        
        var apiResponseBody = new EducationalOrganisationResponse
        {
            EducationalOrganisations = schools
        };

        var expectedResponse = new GetSchoolsQueryApiResult(new List<School>
        {
            new() { Name = "Deeside school", Urn = "123456" },
            new() { Name = "Deer school", Urn = "123457" },
            new() { Name = "Deering school", Urn = "123458" }
        });
        
        var apiResponse = new ApiResponse<EducationalOrganisationResponse>(apiResponseBody, HttpStatusCode.OK, null);

        apiClient
            .Setup(x => x.GetWithResponseCode<EducationalOrganisationResponse>(
                It.Is<SearchEducationalOrganisationsRequest>(r => r.SearchTerm == query.SearchTerm)))
            .ReturnsAsync(apiResponse)
            .Verifiable();

        // Act
        var actual = await sut.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        actual.Data.Count.Should().Be(3);
        actual.Should().BeEquivalentTo(expectedResponse);
    }

    [Test, MoqAutoData]
    public async Task Handle_EndpointNotFound_ReturnsEmptySchools(
        [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> apiClient,
        GetSchoolsQueryHandler sut,
        GetSchoolsQuery query)
    {
        // Arrange
        var apiResponse = new ApiResponse<EducationalOrganisationResponse>(null, HttpStatusCode.NotFound, null);

        apiClient
            .Setup(x => x.GetWithResponseCode<EducationalOrganisationResponse>(
                It.Is<SearchEducationalOrganisationsRequest>(r => r.SearchTerm == query.SearchTerm)))
            .ReturnsAsync(apiResponse)
            .Verifiable();

        // Act
        var actual = await sut.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        actual.Data.Should().BeEquivalentTo(new List<School>());
        apiClient.Verify();
        apiClient.VerifyNoOtherCalls();
    }
}