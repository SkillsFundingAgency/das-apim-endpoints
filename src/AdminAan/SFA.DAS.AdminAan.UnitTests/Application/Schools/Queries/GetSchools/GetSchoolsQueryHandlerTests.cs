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
        GetSchoolsQuery query)
    {
        var searchTerm = "dee";
        query.SearchTerm = searchTerm;

        var schools = new List<School>
        {
            new() { Name = "Deeside school", Urn = "123456" },
            new() { Name = "Deer school", Urn = "123457" },
            new() { Name = "Dee school child centre", Urn = "12345" },
            new() { Name = "Deering school", Urn = "123458" }
        };

        var schoolsExpected = schools.Where(s => s.Urn.Length == 6).ToList();

        var apiResult = new GetSchoolsQueryApiResult(schools);

        var returned = new Response<GetSchoolsQueryApiResult>(
        "not used",
        new HttpResponseMessage(System.Net.HttpStatusCode.OK),
        () => apiResult);

        apiClient.Setup(x => x.GetSchools(searchTerm, 100, 1, It.IsAny<CancellationToken>())).ReturnsAsync(returned);

        var actual = await sut.Handle(query, It.IsAny<CancellationToken>());

        actual.Data.Count.Should().Be(3);
        actual.Should().BeEquivalentTo(new GetSchoolsQueryApiResult(schoolsExpected));
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

        apiClient.Setup(x => x.GetSchools(searchTerm, 100, 1, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var actual = await sut.Handle(query, It.IsAny<CancellationToken>());

        actual.Data.Should().BeEquivalentTo(new List<School>());
    }
}