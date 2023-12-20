using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Threading;
using FluentAssertions.Execution;
using FluentAssertions;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.LocationsController
{
    public class WhenGettingSearchByLocation
    {
        [Test, MoqAutoData]
        public async Task Then_Passes_Query_To_Mediator_And_Returns_Locations(
            string searchTerm,
            GetLocationsBySearchQueryResult queryResponse,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] Api.Controllers.LocationsController controller)
        {
            queryResponse.Locations = queryResponse.Locations.Select(c => { c.Postcode = null; return c; }).ToList();
            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetLocationsBySearchQuery>(c => c.SearchTerm.Equals(searchTerm)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResponse);

            var actual = await controller.SearchByLocation(searchTerm) as ObjectResult;

            using (new AssertionScope())
            {
                actual.StatusCode.Should().Be(200);
                actual.Value.As<GetLocationBySearchResponse>().Locations.Should().BeEquivalentTo(queryResponse.Locations, options => options.ExcludingMissingMembers());
            }
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string searchTerm,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] Api.Controllers.LocationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetLocationsBySearchQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.SearchByLocation(searchTerm) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}