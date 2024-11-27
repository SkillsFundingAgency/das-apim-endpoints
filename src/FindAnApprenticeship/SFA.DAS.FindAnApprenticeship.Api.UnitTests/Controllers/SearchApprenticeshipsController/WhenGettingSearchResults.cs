﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SearchApprenticeshipsController;

public class WhenGettingSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
        Guid candidateId,
        GetSearchApprenticeshipsModel model,
        SearchApprenticeshipsResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
    {
        model.CandidateId = candidateId.ToString();
        model.LevelIds = ["1", "2", "3"];
        model.RouteIds = ["1", "2", "3"];
        model.Sort = VacancySort.DistanceAsc;
        mediator.Setup(x => x.Send(It.Is<SearchApprenticeshipsQuery>(c =>
                    c.CandidateId.Equals(candidateId) &&
                    c.Location.Equals(model.Location) &&
                    c.Distance == model.Distance &&
                    c.SearchTerm == model.SearchTerm),
                CancellationToken.None))
            .ReturnsAsync(result);

        var actual = await controller.SearchResults(model) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual.Value as SearchApprenticeshipsApiResponse;
        Assert.That(actualModel, Is.Not.Null);
        actualModel.Should().BeEquivalentTo((SearchApprenticeshipsApiResponse)result);

        mediator.Verify(m => m.Send(It.Is<SearchApprenticeshipsQuery>(c =>
                c.CandidateId.Equals(candidateId) &&
                c.Location.Equals(model.Location) &&
                c.Distance == model.Distance &&
                c.Sort == VacancySort.DistanceAsc),
            CancellationToken.None));
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
        GetSearchApprenticeshipsModel model,
        string whatSearchTerm,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
    {
        model.CandidateId = null;
        model.Sort = VacancySort.DistanceAsc;
        model.LevelIds = ["1", "2", "3"];
        model.RouteIds = ["1", "2", "3"];
        mediator.Setup(x => x.Send(It.Is<SearchApprenticeshipsQuery>(c =>
                c.Location.Equals(model.Location) &&
                c.Distance == model.Distance &&
                c.SearchTerm == whatSearchTerm),
            CancellationToken.None)).ThrowsAsync(new Exception());


        var actual = await controller.SearchResults(model) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        mediator.Verify(m => m.Send(It.Is<SearchApprenticeshipsQuery>(c =>
                c.Location.Equals(model.Location) &&
                c.Distance == model.Distance && 
                c.Sort == VacancySort.DistanceAsc),
            CancellationToken.None));
    }
}