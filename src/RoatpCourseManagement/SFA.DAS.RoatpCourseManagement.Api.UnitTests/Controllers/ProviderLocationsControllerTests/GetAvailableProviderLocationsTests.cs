using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAvailableProviderLocations;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetProviderLocationDetails;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderLocationsControllerTests
{
    [TestFixture]
    public class GetAvailableProviderLocationsTests
    {
        [Test, MoqAutoData]
        public async Task GetAvailableProviderLocations_ValidRequest_ReturnsLocations(
            int ukprn,
            int larsCode,
            [Frozen] Mock<IMediator> mediatorMock,
            GetAvailableProviderLocationsQueryResult result,
            [Greedy] GetProviderLocationsController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetAvailableProviderLocationsQuery>(q => q.Ukprn == ukprn && q.LarsCode == larsCode), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await sut.GetAvailableProviderLocations(ukprn, larsCode);

            var okResult = response as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(result);
        }
    }
}
