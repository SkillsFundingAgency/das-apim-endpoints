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
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.AccountLegalEntity;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.AccountLegalEntity
{
    public class WhenGettingAnAccountLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_AccountLegalEntity_From_Mediator(
                    long accountLegalEntityId,
                    GetAccountLegalEntityQueryResult mediatorResult,
                    [Frozen] Mock<IMediator> mockMediator,
                    [Greedy] AccountLegalEntityController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAccountLegalEntityQuery>(x => x.AccountLegalEntityId == accountLegalEntityId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(accountLegalEntityId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountLegalEntityQueryResult;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_AccountLegalEntity_Is_Returned_From_Mediator(
            long accountLegalEntityId,
            [Greedy] AccountLegalEntityController controller)
        {
            var controllerResult = await controller.Get(accountLegalEntityId) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountLegalEntityId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountLegalEntityController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountLegalEntityQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(accountLegalEntityId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
