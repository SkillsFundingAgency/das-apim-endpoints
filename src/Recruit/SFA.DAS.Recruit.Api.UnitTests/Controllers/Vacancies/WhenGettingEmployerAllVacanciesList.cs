using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using AutoFixture;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Extensions;
using SFA.DAS.Recruit.Api.Models.Requests;
using SFA.DAS.Recruit.Api.Models.Responses;
using SFA.DAS.Recruit.Data.Models;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using StrawberryShake;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Vacancies;

public class WhenGettingEmployerAllVacanciesList
{
    [Test, MoqAutoData]
    public async Task The_The_Gql_Query_Is_Built_Correctly(
        long accountId,
        IOperationResult<IGetPagedVacanciesListResult> vacanciesResult,
        VacancyListFilterParams filterParams,
        SortParams<VacancySortColumn> sortParams,
        Mock<IRecruitGqlClient> gqlClient,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
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
        
        // act
        await sut.GetEmployerAllVacanciesList(
            gqlClient.Object,
            apiClient.Object,
            accountId,
            filterParams,
            sortParams,
            new PageParams { PageNumber = 1, PageSize = 10 },
            CancellationToken.None);

        // assert
        capturedFilter.Should().BeEquivalentTo(filterParams.Build(accountId: accountId));
        capturedSort.Should().BeEquivalentTo(sortParams.Build());
        capturedSkip.Should().Be(0);
        capturedTake.Should().Be(10);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Gql_Errors_Are_Handled(
        long accountId,
        IOperationResult<IGetPagedVacanciesListResult> vacanciesResult,
        VacancyListFilterParams filterParams,
        SortParams<VacancySortColumn> sortParams,
        Mock<IRecruitGqlClient> gqlClient,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
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
        var result = await sut.GetEmployerAllVacanciesList(
            gqlClient.Object,
            apiClient.Object,
            accountId,
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
    
    [Test, MoqAutoData]
    public async Task Then_Stats_Are_Requested_For_The_Vacancies(
        long accountId,
        Mock<IOperationResult<IGetPagedVacanciesListResult>> vacanciesResult,
        DataResponse<Dictionary<long, VacancyStatsItem>> statsResult,
        VacancyListFilterParams filterParams,
        SortParams<VacancySortColumn> sortParams,
        Mock<IRecruitGqlClient> gqlClient,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        var f = new Fixture();
        var vacancies = f.CreateMany<FakeLiveVacancyItem>(3).ToList();
        vacanciesResult.Setup(x => x.Errors).Returns([]);
        vacanciesResult.Setup(x => x.Data!.PagedVacancies!.Items).Returns(vacancies);
        gqlClient
            .Setup(x => x.GetPagedVacanciesList.ExecuteAsync(
                It.IsAny<VacancyEntityFilterInput?>(),
                It.IsAny<IReadOnlyList<VacancyEntitySortInput>?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacanciesResult.Object);

        var apiResponse = new ApiResponse<DataResponse<Dictionary<long, VacancyStatsItem>>>(statsResult, HttpStatusCode.OK, null);
        GetEmployerVacancyApplicationStatsRequest? capturedRequest = null;
        apiClient
            .Setup(x => x.GetWithResponseCode<DataResponse<Dictionary<long, VacancyStatsItem>>>(It.IsAny<GetEmployerVacancyApplicationStatsRequest>()))
            .Callback((IGetApiRequest request) => capturedRequest = request as GetEmployerVacancyApplicationStatsRequest)
            .ReturnsAsync(apiResponse);

        // act
        await sut.GetEmployerAllVacanciesList(
            gqlClient.Object,
            apiClient.Object,
            accountId,
            filterParams,
            sortParams,
            new PageParams
            {
                PageNumber = 1,
                PageSize = 10
            },
            CancellationToken.None);

        // assert
        capturedRequest.Should().NotBeNull();
        vacancies.Should().AllSatisfy(x =>
        {
            capturedRequest.GetUrl.Should().Contain($"vacancyReferences={x.VacancyReference}");
        });
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Vacancies_Are_Returned(
        long accountId,
        Mock<IOperationResult<IGetPagedVacanciesListResult>> vacanciesResult,
        DataResponse<Dictionary<long, VacancyStatsItem>> statsResult,
        VacancyListFilterParams filterParams,
        SortParams<VacancySortColumn> sortParams,
        Mock<IRecruitGqlClient> gqlClient,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
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

        var statsDict = vacancies.ToDictionary<FakeLiveVacancyItem?, long, VacancyStatsItem>(v => v!.VacancyReference!.Value, v => f.Create<VacancyStatsItem>());
        var statsDataResponse = new DataResponse<Dictionary<long, VacancyStatsItem>>(statsDict);
        
        apiClient
            .Setup(x => x.GetWithResponseCode<DataResponse<Dictionary<long, VacancyStatsItem>>>(It.IsAny<GetEmployerVacancyApplicationStatsRequest>()))
            .ReturnsAsync(new ApiResponse<DataResponse<Dictionary<long, VacancyStatsItem>>>(statsDataResponse, HttpStatusCode.OK, null));
        
        var expectedItems = vacancies.AssignStatsToVacancies(statsDict);

        // act
        var result = await sut.GetEmployerAllVacanciesList(
            gqlClient.Object,
            apiClient.Object,
            accountId,
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