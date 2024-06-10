using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenGettingLocations
    {
        [Test, MoqAutoData]
        public async Task Then_The_Locations_Are_Returned_From_Mediator(
            string searchTerm,
            GetLocationsQueryResponse queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetLocationsQuery>(p => p.SearchTerm == searchTerm), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetByQuery(searchTerm) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var getLocationSearchResponse = actual.Value as GetLocationSearchResponse;
            getLocationSearchResponse.Should().NotBeNull();
            getLocationSearchResponse.Locations.Should().BeEquivalentTo(queryResult.Locations.Select(s => (GetLocationSearchResponseItem)s));
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            string searchTerm,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] LocationsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetLocationsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetByQuery(searchTerm) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
