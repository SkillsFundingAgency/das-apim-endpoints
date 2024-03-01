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
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.Accounts
{
    public class WhenGetEnglishFractionCurrent
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_EnglishFractionCurrent_From_Mediator(
            string hashedAccountId,
            string[] empRefs,
            GetEnglishFractionCurrentQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetEnglishFractionCurrentQuery>(p => p.HashedAccountId == hashedAccountId && p.EmpRefs == empRefs),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetEnglishFractionCurrent(hashedAccountId, empRefs) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetEnglishFractionResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo((GetEnglishFractionResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_EnglishFractionCurrent_Are_Returned_From_Mediator(
            string hashedAccountId,
            string[] empRefs,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetEnglishFractionCurrentQuery>(p => p.HashedAccountId == hashedAccountId && p.EmpRefs == empRefs),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(() => null);

            var controllerResult = await controller.GetEnglishFractionCurrent(hashedAccountId, empRefs) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string hashedAccountId,
            string[] empRefs,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetEnglishFractionCurrentQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetEnglishFractionCurrent(hashedAccountId, empRefs) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
