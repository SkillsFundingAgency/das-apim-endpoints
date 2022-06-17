using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Api.Controllers;
using SFA.DAS.ManageApprenticeships.Api.Models.Transfers;
using SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetFinancialBreakdown;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ManageApprenticeships.Api.UnitTests.Controllers.Transfers
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

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(value);
            Assert.IsNotNull(getFinancialBreakdownResponse);

            Assert.AreEqual(mediatorResult.Commitments, getFinancialBreakdownResponse.Commitments);
            Assert.AreEqual(mediatorResult.AcceptedPledgeApplications, getFinancialBreakdownResponse.AcceptedPledgeApplications);
            Assert.AreEqual(mediatorResult.ApprovedPledgeApplications, getFinancialBreakdownResponse.ApprovedPledgeApplications);
            Assert.AreEqual(mediatorResult.TransferConnections, getFinancialBreakdownResponse.TransferConnections);
        }
    }
}
