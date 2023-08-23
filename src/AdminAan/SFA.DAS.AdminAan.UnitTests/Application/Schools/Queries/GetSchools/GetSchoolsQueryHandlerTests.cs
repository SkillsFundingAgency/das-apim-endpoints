using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.AdminAan.Application.Schools.Queries;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.Schools.Queries.GetSchools;
public class GetSchoolsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsSchools(
        [Frozen] Mock<IReferenceDataApiClient> apiClient,
        GetSchoolsQueryHandler sut,
        GetSchoolsQueryApiResult apiResult,
        GetSchoolsQuery query)
    {
        var searchTerm = query.SearchTerm;

        var expected = new Response<GetSchoolsQueryApiResult>(
        "not used",
        new HttpResponseMessage(System.Net.HttpStatusCode.OK),
        () => apiResult);

        apiClient.Setup(x => x.GetSchools(searchTerm, 100, 1)).ReturnsAsync(expected);

        var actual = await sut.Handle(query, It.IsAny<CancellationToken>());

        actual.Should().BeEquivalentTo(expected.GetContent());
    }

    [Test, MoqAutoData]
    public async Task Handle_EndpointNotFound_ReturnsEmptySchools(
        [Frozen] Mock<IReferenceDataApiClient> apiClient,
        GetSchoolsQueryHandler sut,
        GetSchoolsQueryApiResult apiResult,
        GetSchoolsQuery query)
    {
        var searchTerm = query.SearchTerm;

        var expected = new Response<GetSchoolsQueryApiResult>(
            "not used",
            new HttpResponseMessage(System.Net.HttpStatusCode.NotFound),
            () => apiResult);

        apiClient.Setup(x => x.GetSchools(searchTerm, 100, 1)).ReturnsAsync(expected);

        var actual = await sut.Handle(query, It.IsAny<CancellationToken>());

        actual.Data.Should().BeEquivalentTo(new List<School>());
    }
}