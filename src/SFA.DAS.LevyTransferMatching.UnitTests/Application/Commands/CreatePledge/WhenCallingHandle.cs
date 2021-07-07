using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_New_PledgeId_Is_Returned(
            CreatePledgeCommand createPledgeCommand,
            long accountId,
            int pledgeId,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            CreatePledgeHandler createPledgeHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.CreatePledge(It.Is<Pledge>(y => y.AccountId == accountId)))
                .ReturnsAsync(pledgeId);

            createPledgeCommand.AccountId = accountId;

            var createPledgeResult = await createPledgeHandler.Handle(createPledgeCommand, CancellationToken.None);

            Assert.AreEqual(pledgeId, createPledgeResult.PledgeId);
        }
    }
}