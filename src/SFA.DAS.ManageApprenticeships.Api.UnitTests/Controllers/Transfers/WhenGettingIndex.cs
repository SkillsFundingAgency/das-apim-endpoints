using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Api.Controllers;
using SFA.DAS.ManageApprenticeships.Api.Models.Transfers;
using SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetIndex;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ManageApprenticeships.Api.UnitTests.Controllers.Transfers
{
    public class WhenGettingIndex
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Index_From_Mediator(
            long accountId,
            GetIndexQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TransfersController transfersController)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetIndexQuery>(y => y.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actionResult = await transfersController.GetIndex(accountId);
            var okObjectResult = actionResult as OkObjectResult;
            var value = okObjectResult.Value;
            var getIndexResponse = value as GetIndexResponse;

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(value);
            Assert.IsNotNull(getIndexResponse);

            Assert.AreEqual(mediatorResult.PledgesCount, getIndexResponse.PledgesCount);
            Assert.AreEqual(mediatorResult.ApplicationsCount, getIndexResponse.ApplicationsCount);
        }
    }
}
