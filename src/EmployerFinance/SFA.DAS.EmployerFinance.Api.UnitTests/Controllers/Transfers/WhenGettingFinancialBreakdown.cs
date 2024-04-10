using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Api.Models.Transfers;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetFinancialBreakdown;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.Transfers
{
    public class WhenGettingFinancialBreakdown
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Financial_Breakdown_From_Mediator(
           long accountId,
           GetFinancialBreakdownResult mediatorResult,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] TransfersController transfersController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetFinancialBreakdownQuery>(y => y.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actionResult = await transfersController.GetFinancialBreakdown(accountId);
            var okObjectResult = actionResult as OkObjectResult;
            var value = okObjectResult.Value;
            var getFinancialBreakdownResponse = value as GetFinancialBreakdownResponse;

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(getFinancialBreakdownResponse, Is.Not.Null);

            Assert.That(mediatorResult.Commitments, Is.EqualTo(getFinancialBreakdownResponse.Commitments));
            Assert.That(mediatorResult.AcceptedPledgeApplications, Is.EqualTo(getFinancialBreakdownResponse.AcceptedPledgeApplications));
            Assert.That(mediatorResult.ApprovedPledgeApplications, Is.EqualTo(getFinancialBreakdownResponse.ApprovedPledgeApplications));
            Assert.That(mediatorResult.TransferConnections, Is.EqualTo(getFinancialBreakdownResponse.TransferConnections));
        }
    }
}
