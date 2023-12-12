
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SearchApprenticeshipsController;

public class WhenGettingSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
        List<string> routeIds,
        string location,
        int distance,
        string whatSearchTerm,
        SearchApprenticeshipsResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<SearchApprenticeshipsQuery>(c =>
            c.SelectedRouteIds.Equals(routeIds) &&
            c.Location.Equals(location) &&
            c.Distance == distance &&
            c.WhatSearchTerm == whatSearchTerm),
                CancellationToken.None))
            .ReturnsAsync(result);

        var actual = await controller.SearchResults(routeIds, location, distance, whatSearchTerm) as OkObjectResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual.Value as SearchApprenticeshipsApiResponse;
        Assert.That(actualModel, Is.Not.Null);
        actualModel.Should().BeEquivalentTo((SearchApprenticeshipsApiResponse)result);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(List<string> routeIds,
        string location,
        int distance,
        string whatSearchTerm,
        SearchApprenticeshipsResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<SearchApprenticeshipsQuery>(c =>
                c.SelectedRouteIds.Equals(routeIds) &&
                c.Location.Equals(location) &&
                c.Distance == distance &&
                c.WhatSearchTerm == whatSearchTerm),
            CancellationToken.None)).ThrowsAsync(new Exception());
       
        var actual = await controller.SearchResults(routeIds, location, distance, whatSearchTerm) as StatusCodeResult;

        Assert.IsNotNull(actual);
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}

