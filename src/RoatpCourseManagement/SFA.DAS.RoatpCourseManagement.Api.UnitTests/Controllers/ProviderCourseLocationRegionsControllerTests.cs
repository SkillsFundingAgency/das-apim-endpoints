using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderCourseLocationRegionsControllerTests
    {
        [Test, MoqAutoData]
        public async Task GetAllStandardSubRegions_ValidRequest_ReturnsLocations(
            int ukprn,
            int larsCode,
            [Frozen] Mock<IMediator> mediatorMock,
            GetAllStandardRegionsQueryResult result,
            [Greedy] ProviderCourseLocationRegionsController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetAllStandardRegionsQuery>(q => q.Ukprn == ukprn && q.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await sut.GetAllStandardSubRegions(ukprn, larsCode);

            var okResult = response as OkObjectResult;
            okResult.Should().NotBeNull();
            var queryResult = okResult.Value as GetAllStandardRegionsQueryResult;
            queryResult.Should().NotBeNull();
            queryResult.Regions.Should().BeEquivalentTo(result.Regions);
        }

        [Test, MoqAutoData]
        public async Task GetAllStandardSubRegions_InvalidRequest_ReturnsLocations(
            int ukprn,
            int larsCode,
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProviderCourseLocationRegionsController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetAllStandardRegionsQuery>(q => q.Ukprn == ukprn && q.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(new GetAllStandardRegionsQueryResult());

            var response = await sut.GetAllStandardSubRegions(ukprn, larsCode);

            (response as NotFoundResult).Should().NotBeNull();
        }
    }
}
