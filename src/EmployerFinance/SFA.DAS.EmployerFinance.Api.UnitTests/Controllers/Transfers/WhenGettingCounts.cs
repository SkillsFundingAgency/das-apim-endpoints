using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Controllers;
using SFA.DAS.EmployerFinance.Api.Models.Transfers;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Controllers.Transfers
{
    public class WhenGettingCounts
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Counts_From_Mediator(
            long accountId,
            GetCountsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TransfersController transfersController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetCountsQuery>(y => y.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actionResult = await transfersController.GetCounts(accountId);
            var okObjectResult = actionResult as OkObjectResult;
            var value = okObjectResult.Value;
            var getIndexResponse = value as GetCountsResponse;

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(okObjectResult, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(getIndexResponse, Is.Not.Null);

            Assert.That(mediatorResult.PledgesCount, Is.EqualTo(getIndexResponse.PledgesCount));
            Assert.That(mediatorResult.ApplicationsCount, Is.EqualTo(getIndexResponse.ApplicationsCount));
            Assert.That(mediatorResult.CurrentYearEstimatedCommittedSpend, Is.EqualTo(getIndexResponse.CurrentYearEstimatedCommittedSpend));
        }
    }
}
