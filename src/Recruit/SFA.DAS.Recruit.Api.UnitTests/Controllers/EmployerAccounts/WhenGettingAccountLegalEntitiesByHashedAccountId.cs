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
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerAccounts
{
    public class WhenGettingAccountLegalEntitiesByHashedAccountId
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            string hashedAccountId,
            GetAccountLegalEntitiesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAccountLegalEntitiesQuery>(c=>c.HashedAccountId.Equals(hashedAccountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAccountLegalEntities(hashedAccountId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountLegalEntitiesResponse;
            Assert.That(model, Is.Not.Null);
            model.AccountLegalEntities.Should().BeEquivalentTo(mediatorResult.AccountLegalEntities, options=>options.Excluding(x=>x.Agreements));
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string hashedAccountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountLegalEntitiesQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAccountLegalEntities(hashedAccountId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}