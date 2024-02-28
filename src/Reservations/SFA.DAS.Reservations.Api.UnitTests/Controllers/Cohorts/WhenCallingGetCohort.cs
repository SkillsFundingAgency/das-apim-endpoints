using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Controllers;
using SFA.DAS.Reservations.Application.Providers.Queries.GetCohort;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.Api.UnitTests.Controllers.Providers
{
    public class WhenCallingGetCohort
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Cohort_From_Mediator(
            long cohortId,
            GetCohortResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CohortsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCohortQuery>(query => query.CohortId == cohortId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(cohortId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCohortResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult.Cohort);
        }
        [Test, MoqAutoData]
        public async Task Then_Returns_Not_Found_If_No_Cohort(
            long cohortId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CohortsController controller)
        {
            
            mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetCohortQuery>(query => query.CohortId == cohortId),
                   It.IsAny<CancellationToken>()))
              .ReturnsAsync(new GetCohortResult { Cohort = null });

            var controllerResult = await controller.Get(cohortId) as NotFoundResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
           long cohortId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CohortsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCohortQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(cohortId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}