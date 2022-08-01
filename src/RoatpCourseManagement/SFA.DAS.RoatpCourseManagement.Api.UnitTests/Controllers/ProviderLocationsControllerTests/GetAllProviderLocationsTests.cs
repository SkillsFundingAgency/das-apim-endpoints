using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderLocationsControllerTests
{
    [TestFixture]
    public class GetAllProviderLocationsTests
    {
        [Test, MoqAutoData]
        public async Task GetAllLocations_ValidRequest_ReturnsLocations(
            int ukprn,
            [Frozen] Mock<IMediator> mediatorMock,
            GetAllProviderLocationsQueryResult result,
            [Greedy] GetProviderLocationsController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetAllProviderLocationsQuery>(c =>
                        c.GetUrl.Equals(new GetAllProviderLocationsQuery(ukprn).GetUrl)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await sut.GetAllProviderLocations(ukprn);

            var okResult = response as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(result.ProviderLocations);
        }

        [Test, MoqAutoData]
        public async Task GetAllLocations_InvalidRequest_ReturnsLocations(
            int ukprn,
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] GetProviderLocationsController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetAllProviderLocationsQuery>(c =>
                        c.GetUrl.Equals(new GetAllProviderLocationsQuery(ukprn).GetUrl)), It.IsAny<CancellationToken>())).ReturnsAsync(new GetAllProviderLocationsQueryResult());

            var response = await sut.GetAllProviderLocations(ukprn);

            (response as BadRequestResult).Should().NotBeNull();
        }
    }
}
