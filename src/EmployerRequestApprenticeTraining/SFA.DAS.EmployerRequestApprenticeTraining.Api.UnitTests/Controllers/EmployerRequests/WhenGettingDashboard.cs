using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenGettingDashboard
    {
        [Test, MoqAutoData]
        public async Task Then_The_AggregatedEmployerRequests_Are_Returned_From_Mediator(
            GetAggregatedEmployerRequestsResult queryAggregatedEmployerRequestsResult,
            GetSettingsResult querySettingsResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller,
            long ukprn)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), CancellationToken.None))
                .ReturnsAsync(queryAggregatedEmployerRequestsResult);

            mockMediator
                .Setup(x => x.Send(It.IsAny<GetSettingsQuery>(), CancellationToken.None))
                .ReturnsAsync(querySettingsResult);

            var actual = await controller.GetDashboard(ukprn) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var dashboard = actual.Value as Dashboard;
            Assert.That(dashboard, Is.Not.Null);

            dashboard.AggregatedEmployerRequests.Should().BeEquivalentTo(queryAggregatedEmployerRequestsResult.AggregatedEmployerRequests);
            dashboard.ExpiryAfterMonths.Should().Be(querySettingsResult.ExpiryAfterMonths);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid employerRequestId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller,
            long ukprn)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetDashboard(ukprn) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
