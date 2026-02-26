using AutoFixture;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Extensions;
using SFA.DAS.Recruit.Api.Models.Requests;
using SFA.DAS.Recruit.Api.Models.Responses;
using SFA.DAS.Recruit.Data.Models;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using SFA.DAS.Recruit.InnerApi.Responses;
using StrawberryShake;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Vacancies;

public class WhenGettingProviderVacanciesListByStatus
{
    [Test]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Draft)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Submitted)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Live)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Closed)]
    public async Task The_The_Gql_Query_Is_Built_Correctly(
        Domain.Vacancy.VacancyStatus status,
        int ukprn,
        IOperationResult<IGetPagedVacanciesListResult> vacanciesResult,
        VacancyListFilterParams filterParams,
        SortParams<VacancySortColumn> sortParams,
        Mock<IRecruitGqlClient> gqlClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        VacancyEntityFilterInput? capturedFilter = null;
        IReadOnlyList<VacancyEntitySortInput>? capturedSort = null;
        int? capturedSkip = null;
        int? capturedTake = null;
        gqlClient
            .Setup(x => x.GetPagedVacanciesList.ExecuteAsync(
                It.IsAny<VacancyEntityFilterInput?>(),
                It.IsAny<IReadOnlyList<VacancyEntitySortInput>?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .Callback((VacancyEntityFilterInput? filter, IReadOnlyList<VacancyEntitySortInput>? sort, int? skip, int? take, CancellationToken _) =>
            {
                capturedFilter = filter;
                capturedSort = sort;
                capturedSkip = skip;
                capturedTake = take;
            })
            .ReturnsAsync(vacanciesResult);

        GqlTypeExtensions.TryMapToGqlStatus(status, out var gqlStatus);

        // act
        await sut.GetProviderVacanciesListByStatus(
            gqlClient.Object,
            ukprn,
            status,
            filterParams,
            sortParams,
            new PageParams { PageNumber = 1, PageSize = 10 },
            CancellationToken.None);

        // assert
        capturedFilter.Should().BeEquivalentTo(filterParams.Build(ukprn: ukprn, statuses: [gqlStatus]));
        capturedSort.Should().BeEquivalentTo(sortParams.Build());
        capturedSkip.Should().Be(0);
        capturedTake.Should().Be(10);
    }
    
    [Test]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Draft)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Submitted)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Live)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Closed)]
    public async Task Then_Gql_Errors_Are_Handled(
        Domain.Vacancy.VacancyStatus status,
        int ukprn,
        IOperationResult<IGetPagedVacanciesListResult> vacanciesResult,
        VacancyListFilterParams filterParams,
        SortParams<VacancySortColumn> sortParams,
        Mock<IRecruitGqlClient> gqlClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        gqlClient
            .Setup(x => x.GetPagedVacanciesList.ExecuteAsync(
                It.IsAny<VacancyEntityFilterInput?>(),
                It.IsAny<IReadOnlyList<VacancyEntitySortInput>?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacanciesResult);
        
        // act
        var result = await sut.GetProviderVacanciesListByStatus(
            gqlClient.Object,
            ukprn,
            status,
            filterParams,
            sortParams,
            new PageParams
            {
                PageNumber = 1,
                PageSize = 10
            },
            CancellationToken.None) as ProblemHttpResult;

        // assert
        result.Should().NotBeNull();
    }

    [Test]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Draft)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Submitted)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Live)]
    [MoqInlineAutoData(Domain.Vacancy.VacancyStatus.Closed)]
    public async Task Then_The_Vacancies_Are_Returned(
        Domain.Vacancy.VacancyStatus status,
        int ukprn,
        Mock<IOperationResult<IGetPagedVacanciesListResult>> vacanciesResult,
        DataResponse<Dictionary<long, VacancyStatsItem>> statsResult,
        VacancyListFilterParams filterParams,
        SortParams<VacancySortColumn> sortParams,
        Mock<IRecruitGqlClient> gqlClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        var f = new Fixture();
        var vacancies = f.CreateMany<FakeLiveVacancyItem>(3).ToList();
        vacanciesResult.Setup(x => x.Errors).Returns([]);
        vacanciesResult.Setup(x => x.Data!.PagedVacancies!.Items).Returns(vacancies);
        vacanciesResult.Setup(x => x.Data!.PagedVacancies!.TotalCount).Returns(vacancies.Count);

        gqlClient
            .Setup(x => x.GetPagedVacanciesList.ExecuteAsync(
                It.IsAny<VacancyEntityFilterInput?>(),
                It.IsAny<IReadOnlyList<VacancyEntitySortInput>?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacanciesResult.Object);
        
        var expectedItems = vacancies.Select(x => VacancyListItem.From(x, null));

        // act
        var result = await sut.GetProviderVacanciesListByStatus(
            gqlClient.Object,
            ukprn,
            status,
            filterParams,
            sortParams,
            new PageParams
            {
                PageNumber = 1,
                PageSize = 10
            },
            CancellationToken.None) as Ok<PagedDataResponse<IEnumerable<VacancyListItem>>>;

        // assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.PageInfo.RequestedPageNumber.Should().Be(1);
        result.Value.PageInfo.RequestedPageSize.Should().Be(10);
        result.Value.PageInfo.TotalCount.Should().Be((uint)vacancies.Count);
        result.Value.Data.Should().BeEquivalentTo(expectedItems);
    }
}