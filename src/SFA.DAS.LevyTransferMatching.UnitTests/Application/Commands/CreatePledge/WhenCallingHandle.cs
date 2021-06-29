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
        public async Task Then_Pledge_Created_And_Pledge_Id_Returned(
            CreatePledgeCommand createPledgeCommand,
            PledgeReference pledgeReference,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            CreatePledgeHandler createPledgeHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.CreatePledge(It.Is<CreatePledgeCommand>(y => y == createPledgeCommand)))
                .ReturnsAsync(pledgeReference);

            var result = await createPledgeHandler.Handle(createPledgeCommand, CancellationToken.None);

            Assert.AreEqual(result.PledgeId, pledgeReference.Id);
        }
    }
}