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
            string encodedAccountId,
            CreatePledgeRequest createPledgeRequest,
            CreatePledgeResult createPledgeResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] PledgeController pledgeController)
        {
            mockMediator
                .Setup(x => x.Send(
                    It.Is<CreatePledgeCommand>((x) => x.Pledge.EncodedAccountId == encodedAccountId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createPledgeResult);

            var controllerResult = await pledgeController.CreatePledge(encodedAccountId, createPledgeRequest);
            var acceptedResult = controllerResult as AcceptedResult;
            var pledgeReference = acceptedResult.Value as PledgeReferenceDto;

            Assert.IsNotNull(controllerResult);
            Assert.IsNotNull(acceptedResult);
            Assert.IsNotNull(pledgeReference);
            Assert.AreEqual(acceptedResult.StatusCode, (int)HttpStatusCode.Accepted);
            Assert.AreEqual(acceptedResult.Location, $"/accounts/{encodedAccountId}/pledges/{createPledgeResult.PledgeReference.Id}");
            Assert.AreEqual(pledgeReference.Id, createPledgeResult.PledgeReference.Id);
        }
    }
}