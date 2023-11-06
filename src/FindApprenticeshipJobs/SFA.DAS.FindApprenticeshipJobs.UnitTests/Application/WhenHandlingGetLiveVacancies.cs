using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;
public class WhenHandlingGetLiveVacancies
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Live_Vacancies(
        GetLiveVacanciesQuery mockQuery,
        ApiResponse<GetLiveVacanciesApiResponse> mockApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockApiClient,
        GetLiveVacanciesQueryHandler sut)
    {
        mockApiClient.Setup(client => client.GetWithResponseCode<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>())).ReturnsAsync(mockApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        AssertResponse(actual, mockApiResponse.Body);
    }

    [Test, MoqAutoData]
    public async Task And_Api_Client_Returns_Null(
        GetLiveVacanciesQuery mockQuery,
        ApiResponse<GetLiveVacanciesApiResponse> mockApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockApiClient,
        GetLiveVacanciesQueryHandler sut)
    {
        mockApiResponse.Body.Vacancies = Enumerable.Empty<LiveVacancy>();
        mockApiClient.Setup(client => client.GetWithResponseCode<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>())).ReturnsAsync(mockApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        AssertResponse(actual, mockApiResponse.Body);
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
