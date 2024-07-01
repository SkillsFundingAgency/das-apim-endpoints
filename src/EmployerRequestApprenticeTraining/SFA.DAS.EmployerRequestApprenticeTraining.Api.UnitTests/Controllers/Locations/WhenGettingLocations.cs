using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.Locations
{
    public class WhenGettingLocations
    {
        [Test, MoqAutoData]
        public async Task Then_The_Locations_Are_Returned_From_Mediator(
            string searchTerm,
            GetLocationsResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetLocationsQuery>(p => p.SearchTerm == searchTerm), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.Get(searchTerm, false) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var locationSearchResponse = actual.Value as List<LocationSearchResponse>;
            locationSearchResponse.Should().NotBeNull();
            locationSearchResponse.Should().BeEquivalentTo(queryResult.Locations.Select(s => (LocationSearchResponse)s));
        }

        [Test, MoqAutoData]
        public async Task Then_Exact_Location_Is_Returned_From_Mediator_When_ExactMatch_Is_True(
            string searchTerm,
            GetLocationResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetLocationQuery>(p => p.ExactSearchTerm == searchTerm), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.Get(searchTerm, true) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var locationSearchResponse = actual.Value as List<LocationSearchResponse>;
            locationSearchResponse.Should().NotBeNull();
            locationSearchResponse.Should().BeEquivalentTo(new List<LocationSearchResponse> { (LocationSearchResponse)queryResult.Location });
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            string searchTerm,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] LocationsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetLocationsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.Get(searchTerm, false) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown_For_ExactMatch(
            string searchTerm,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] LocationsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetLocationQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.Get(searchTerm, true) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
