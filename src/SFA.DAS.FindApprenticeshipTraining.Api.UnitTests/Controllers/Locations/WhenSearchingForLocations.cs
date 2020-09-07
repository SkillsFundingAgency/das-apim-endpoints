using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Locations
{
    public class WhenSearchingForLocations
    {
        [Test, MoqAutoData]
        public async Task Then_Passes_Query_To_Mediator_And_Returns_Locations(
            string searchTerm,
            GetLocationsQueryResponse queryResponse,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            //Arrange
            queryResponse.Locations = queryResponse.Locations.Select(c => { c.Postcode = null; return c; }).ToList();

            mockMediator
                .Setup(mediator => mediator.Send(It.Is<GetLocationsQuery>(c => c.SearchTerm.Equals(searchTerm)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResponse);

            var actual = await controller.GetByQuery(searchTerm) as ObjectResult;

            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = actual.Value as GetLocationSearchResponse;
            model.Locations.Should().BeEquivalentTo(queryResponse.Locations, options=>options.ExcludingMissingMembers());
            
        }
        
        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string searchTerm,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetLocationsQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetByQuery(searchTerm) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}