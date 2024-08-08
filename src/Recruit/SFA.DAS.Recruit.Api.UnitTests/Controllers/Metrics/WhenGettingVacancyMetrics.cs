using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Metrics
{
    public class WhenGettingVacancyMetrics
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancy_Metrics_By_Reference_From_Mediator(
            DateTime startDate,
            DateTime endDate,
            GetVacancyMetricsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] MetricsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetVacancyMetricsQuery>(c => c.StartDate.Equals(startDate)
                                                        && c.EndDate.Equals(endDate)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetVacancyMetrics(startDate, endDate) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetVacancyMetricsResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_InternalServerError(
            DateTime startDate,
            DateTime endDate,
            GetVacancyMetricsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] MetricsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetVacancyMetricsQuery>(c => c.StartDate.Equals(startDate)
                                                        && c.EndDate.Equals(endDate)),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetVacancyMetrics(startDate, endDate) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
