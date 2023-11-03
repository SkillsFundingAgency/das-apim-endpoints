using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;
public class WhenHandlingGetLiveVacancies
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Live_Vacancies(
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockApiClient,
        GetLiveVacanciesQuery mockQuery,
        GetLiveVacanciesApiResponse mockApiResponse)
    {
        mockApiClient.Setup(client => client.Get<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>())).ReturnsAsync(mockApiResponse);
        var sut = new GetLiveVacanciesQueryHandler(mockApiClient.Object);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        AssertResponse(actual, mockApiResponse);
    }

    [Test, MoqAutoData]
    public async Task And_Api_Client_Returns_Null(
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockApiClient, GetLiveVacanciesQuery mockQuery)
    {
        var mockApiResponse = new GetLiveVacanciesApiResponse() { Vacancies = Enumerable.Empty<LiveVacancy>() };
        mockApiClient.Setup(client => client.Get<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>())).ReturnsAsync(mockApiResponse);
        var sut = new GetLiveVacanciesQueryHandler(mockApiClient.Object);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        AssertResponse(actual, mockApiResponse);
    }

    private static void AssertResponse(GetLiveVacanciesQueryResult actual, GetLiveVacanciesApiResponse mockApiResponse)
    {
        actual.Should().BeOfType<GetLiveVacanciesQueryResult>();
        actual.Vacancies.Should().BeEquivalentTo(mockApiResponse.Vacancies);
        actual.PageSize.Should().Be(mockApiResponse.PageSize);
        actual.PageNo.Should().Be(mockApiResponse.PageNo);
        actual.TotalLiveVacanciesReturned.Should().Be(mockApiResponse.TotalLiveVacanciesReturned);
        actual.TotalLiveVacancies.Should().Be(mockApiResponse.TotalLiveVacancies);
        actual.TotalPages.Should().Be(mockApiResponse.TotalPages);
    }
}
