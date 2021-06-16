using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Encoding;
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
        public async Task Then_(
            CreatePledgeCommand createPledgeCommand,
            long accountId,
            CreatePledgeResult expectedCreatePledgeResult,
            [Frozen] Mock<IEncodingService> mockEncodingService,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            CreatePledgeHandler createPledgeHandler)
        {
            mockEncodingService
                .Setup(x => x.Decode(It.Is<string>(y => y == createPledgeCommand.EncodedAccountId), It.Is<EncodingType>(y => y == EncodingType.AccountId)))
                .Returns(accountId);

            mockLevyTransferMatchingService
                .Setup(x => x.CreatePledge(It.Is<Pledge>(y => y.AccountId == accountId)))
                .ReturnsAsync(expectedCreatePledgeResult);

            var actualCreatePledgeResult = await createPledgeHandler.Handle(createPledgeCommand, CancellationToken.None);

            // TODO: Needs fixing
            Assert.AreEqual(expectedCreatePledgeResult, actualCreatePledgeResult);
        }
    }
}