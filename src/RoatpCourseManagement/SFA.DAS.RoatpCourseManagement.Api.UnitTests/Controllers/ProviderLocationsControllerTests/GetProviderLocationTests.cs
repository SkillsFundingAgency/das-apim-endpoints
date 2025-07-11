﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetProviderLocationDetails;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderLocationsControllerTests
{
    [TestFixture]
    public class GetProviderLocationTests
    {
        [Test, MoqAutoData]
        public async Task GetProviderLocation_ValidRequest_ReturnsLocations(
            int ukprn,
            Guid id,
            [Frozen] Mock<IMediator> mediatorMock,
            GetProviderLocationDetailsQueryResult result,
            [Greedy] GetProviderLocationsController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderLocationDetailsQuery>(c => c.GetUrl.Equals(new GetProviderLocationDetailsQuery(ukprn, id).GetUrl)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await sut.GetProviderLocation(ukprn, id);

            var okResult = response as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(result.ProviderLocation);
        }

        [Test, MoqAutoData]
        public async Task GetProviderLocation_ReturnsWithNull_Returns_NotFound(
            int ukprn,
            Guid id,
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] GetProviderLocationsController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderLocationDetailsQuery>(c => c.GetUrl.Equals(new GetProviderLocationDetailsQuery(ukprn, id).GetUrl)), It.IsAny<CancellationToken>())).ReturnsAsync((GetProviderLocationDetailsQueryResult)null);

            var response = await sut.GetProviderLocation(ukprn, id);

            (response as NotFoundResult).Should().NotBeNull();
        }
    }
}
