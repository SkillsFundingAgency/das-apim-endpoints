﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetPledges;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.Pledges
{
    public class WhenGettingPledges
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Pledges_From_Mediator(
            long accountId,
            GetPledgesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgesController pledgesController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetPledgesQuery>(y => y.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actionResult = await pledgesController.GetPledges(accountId);
            var okObjectResult = actionResult as OkObjectResult;
            var value = okObjectResult.Value;
            var getPledgesResponse = value as GetPledgesResponse;

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(getPledgesResponse, Is.Not.Null);

            Assert.That(mediatorResult.TotalPledges, Is.EqualTo(getPledgesResponse.TotalPledges));
            Assert.That(mediatorResult.Pledges.Count(), Is.EqualTo(getPledgesResponse.Pledges.Count()));
        }
    }
}