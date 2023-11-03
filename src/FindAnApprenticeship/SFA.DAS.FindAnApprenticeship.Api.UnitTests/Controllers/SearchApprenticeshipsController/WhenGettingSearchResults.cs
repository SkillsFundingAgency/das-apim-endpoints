using System;
using System.Collections.Generic;
using System.Net;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchResults;
using System.Threading;
using FluentAssertions.Execution;
using FluentAssertions;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SearchApprenticeshipsController
{
    public class WhenGettingSearchResults
    {
        [Test, MoqAutoData]
        public async Task Then_Mediator_Returns_Search_Results(
            List<string> routeIds,
            string location,
            string searchTerm,
            SearchResultsQueryResult mockQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] Api.Controllers.SearchApprenticeshipsController sut)
        {
            mockMediator.Setup(mediator => mediator.Send(It.IsAny<SearchResultsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

            var actual = await sut.SearchResults(routeIds, location, searchTerm) as ObjectResult;
            var actualValue = actual.Value as SearchResultsApiResponse;

            using (new AssertionScope())
            {
                actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
                actual.Value.Should().BeOfType<SearchResultsApiResponse>();
                actualValue.TotalApprenticeshipCount.Should().Be(mockQueryResult.TotalApprenticeshipCount);
            }
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
            List<string> routeIds,
            string location,
            string searchTerm,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
        {
            mockMediator.Setup(mediator => mediator.Send(It.IsAny<SearchResultsQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.SearchResults(routeIds, location, searchTerm) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
