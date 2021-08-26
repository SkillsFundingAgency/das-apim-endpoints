using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Api.Controllers;
using SFA.DAS.ManageApprenticeships.Api.Models;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetPledges;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ManageApprenticeships.Api.UnitTests.Controllers.Pledges
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

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(value);
            Assert.IsNotNull(getPledgesResponse);

            Assert.AreEqual(mediatorResult.TotalPledges, getPledgesResponse.TotalPledges);
            Assert.AreEqual(mediatorResult.Pledges.Count(), getPledgesResponse.Pledges.Count());
        }
    }
}