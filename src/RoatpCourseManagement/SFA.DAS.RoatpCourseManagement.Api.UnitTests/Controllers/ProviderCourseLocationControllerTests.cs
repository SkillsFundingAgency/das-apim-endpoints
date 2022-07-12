﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderCourseLocationControllerTests
    {
        [Test, MoqAutoData]
        public async Task GetProviderCourseLocations_ValidRequest_ReturnsProviderCourseLocations(
            int ukprn,
            int larsCode,
            [Frozen] Mock<IMediator> mediatorMock,
            GetProviderCourseLocationResult result,
            [Greedy] ProviderCourseLocationController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderCourseLocationQuery>(q => q.LarsCode == larsCode && q.Ukprn == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await sut.GetProviderCourseLocations(ukprn, larsCode);

            var okResult = response as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(result);
        }

        [Test, MoqAutoData]
        public async Task GetProviderCourseLocations_InvalidRequest_ReturnsLocations(
            int ukprn,
            int larsCode,
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProviderCourseLocationController sut)
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderCourseLocationQuery>(q => q.LarsCode == larsCode && q.Ukprn == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderCourseLocationResult());

            var response = await sut.GetProviderCourseLocations(ukprn, larsCode);

            (response as NotFoundResult).Should().NotBeNull();
        }
    }
}
