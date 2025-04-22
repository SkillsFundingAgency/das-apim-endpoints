using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerAccounts
{
    [TestFixture]
    public class WhenGettingDashboardByAccountId
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            long accountId,
            ApplicationStatus status,
            GetDashboardByAccountIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetDashboardByAccountIdQuery>(c => c.AccountId.Equals(accountId) &&  c.Status == status),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetDashboard(accountId, status) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetDashboardByAccountIdQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountId,
            ApplicationStatus status,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetDashboardByAccountIdQuery>(c => c.AccountId.Equals(accountId) && c.Status == status),
                    It.IsAny<CancellationToken>()))
               .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetDashboard(accountId, status) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
