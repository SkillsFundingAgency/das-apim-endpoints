using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearchVacancies;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers;

public class WhenPostingGetSavedSearchResult
{
    [Test, MoqAutoData]
    public async Task Then_The_GetSavedSearchVacanciesQuery_Is_Called_With_Request_Values_And_Vacancies_Returned(
        GetCandidateSavedVacanciesRequest request,
        GetSavedSearchVacanciesQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetSavedSearchVacanciesQuery>(c =>
            c.ApprenticeshipSearchResultsSortOrder == request.ApprenticeshipSearchResultsSortOrder &&
            c.Id == request.Id &&
            c.UserId == request.UserId &&
            c.Distance == request.Distance &&
            c.SearchTerm == request.SearchTerm &&
            c.Location == request.Location &&
            c.DisabilityConfident == request.DisabilityConfident &&
            c.Longitude == request.Longitude &&
            c.Latitude == request.Latitude &&
            c.SelectedLevelIds == request.SelectedLevelIds &&
            c.SelectedRouteIds == request.SelectedRouteIds &&
            c.UnSubscribeToken == request.UnSubscribeToken &&
            c.LastRunDateFilter == request.LastRunDateFilter &&
            c.PageNumber == request.PageNumber &&
            c.PageSize == request.PageSize
        ), It.IsAny<CancellationToken>())).ReturnsAsync(result);
        
        var actual = await controller.GetSavedSearchResult(
            Mock.Of<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>>(),
            request)  as ObjectResult;
        
        var actualValue = actual!.Value as GetSavedSearchVacanciesQueryResult;
        actualValue.Should().BeEquivalentTo(result);
    }
    
    
    [Test, MoqAutoData]
    public async Task Then_If_GetSavedSearchVacanciesQuery_Returns_Null_NotFound_Is_Returned(
        GetCandidateSavedVacanciesRequest request,
        GetSavedSearchVacanciesQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetSavedSearchVacanciesQuery>(
        ), It.IsAny<CancellationToken>())).ReturnsAsync((GetSavedSearchVacanciesQueryResult)null!);
        
        var actual = await controller.GetSavedSearchResult(
            Mock.Of<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>>(),
            request)  as NotFoundResult;
        
        actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_GetSavedSearchVacanciesQuery_Throws_Exception_Then_InternalServerError_Is_Returned(
        GetCandidateSavedVacanciesRequest request,
        GetSavedSearchVacanciesQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetSavedSearchVacanciesQuery>(
        ), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        
        var actual = await controller.GetSavedSearchResult(
            Mock.Of<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>>(),
            request)  as StatusCodeResult;
        
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_When_A_Candidate_Is_Not_Found_Their_Saved_Search_Is_Deleted(
        GetCandidateSavedVacanciesRequest request,
        GetSavedSearchVacanciesQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaInnerClient,
        [Greedy] SavedSearchesController controller)
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchVacanciesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as GetSavedSearchVacanciesQueryResult);

        // act
        await controller.GetSavedSearchResult(faaInnerClient.Object, request);

        // assert
        faaInnerClient.Verify(x => x.Delete(It.Is<DeleteSavedSearchesRequest>(c => c.Ids[0] == request.Id && c.Ids.Count == 1)), Times.Once);
    }
}