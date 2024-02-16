using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    public class WhenCallingCreatePledge
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_PledgeReference_From_Mediator(
            long accountId,
            CreatePledgeRequest createPledgeRequest,
            CreatePledgeResult createPledgeResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<CreatePledgeCommand>((x) => x.AccountId == accountId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createPledgeResult);

            var controllerResult = await pledgeController.CreatePledge(accountId, createPledgeRequest);
            var createdResult = controllerResult as CreatedResult;
            var pledgeReference = createdResult.Value as PledgeIdDto;

            Assert.That(controllerResult, Is.Not.Null);
            Assert.IsNotNull(createdResult);
            Assert.IsNotNull(pledgeReference);
            Assert.AreEqual(createdResult.StatusCode, (int)HttpStatusCode.Created);
            Assert.AreEqual(createdResult.Location, $"/accounts/{accountId}/pledges/{createPledgeResult.PledgeId}");
            Assert.AreEqual(pledgeReference.Id, createPledgeResult.PledgeId);
        }
    }
}