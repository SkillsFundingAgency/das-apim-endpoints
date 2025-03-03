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
        
        var actual = await controller.GetSavedSearchResult(request)  as ObjectResult;
        
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
        
        var actual = await controller.GetSavedSearchResult(request)  as NotFoundResult;
        
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
        
        var actual = await controller.GetSavedSearchResult(request)  as StatusCodeResult;
        
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}