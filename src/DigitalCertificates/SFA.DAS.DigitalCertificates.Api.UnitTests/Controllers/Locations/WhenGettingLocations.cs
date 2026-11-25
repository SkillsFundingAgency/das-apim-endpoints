using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Queries.GetLocations;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Locations
{
    public class WhenGettingLocations
    {
        [Test, MoqAutoData]
        public async Task Then_The_Addresses_Are_Returned_From_Mediator(
            string query,
            GetLocationsResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetLocationsQuery>(p => p.Query == query), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetLocations(query) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult.Addresses);
        }

        [Test, MoqAutoData]
        public async Task Then_Ok_Returned_When_No_Addresses(
            string query,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LocationsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetLocationsQuery>(), CancellationToken.None))
                .ReturnsAsync(new GetLocationsResult { Addresses = null });

            var actual = await controller.GetLocations(query) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            string query,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] LocationsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetLocationsQuery>(), CancellationToken.None))
                .ThrowsAsync(new System.Exception());

            var actual = await controller.GetLocations(query) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
