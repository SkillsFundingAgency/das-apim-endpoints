using System;
using System.Collections.Generic;
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
using SFA.DAS.Reservations.Application.AccountProviderLegalEntities.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.Api.UnitTests.Controllers.AccountProviderLegalEntities
{
    public class WhenCallingGetAccountProviderLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_AccountProviderLegalEntities_From_Mediator(
            int ukprn,
            List<Operation> operations,
            GetAccountProviderLegalEntitiesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountProviderLegalEntitiesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountProviderLegalEntitiesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(ukprn, operations) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetProviderAccountLegalEntitiesResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult.AccountProviderLegalEntities);
        }
        [Test, MoqAutoData]
        public async Task Then_Returns_Not_Found_If_No_Cohort(
            int ukprn,
            List<Operation> operations,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountProviderLegalEntitiesController controller)
        {

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountProviderLegalEntitiesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAccountProviderLegalEntitiesResult { AccountProviderLegalEntities = null });

            var controllerResult = await controller.Get(ukprn, operations) as NotFoundResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            List<Operation> operations,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountProviderLegalEntitiesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountProviderLegalEntitiesQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(ukprn, operations) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
