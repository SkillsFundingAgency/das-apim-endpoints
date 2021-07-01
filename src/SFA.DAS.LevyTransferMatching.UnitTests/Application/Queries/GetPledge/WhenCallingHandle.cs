using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledge
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_Valid_Id_Specified_Then_Pledge_Returned(
            int pledgeId,
            Pledge pledge,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgeQueryHandler getPledgeQueryHandler)
        {
            GetPledgeQuery getPledgeQuery = new GetPledgeQuery()
            {
                PledgeId = pledgeId,
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == pledgeId)))
                .ReturnsAsync(pledge);

            var result = await getPledgeQueryHandler.Handle(getPledgeQuery, CancellationToken.None);

            Assert.IsNotNull(result);
        }

        [Test, MoqAutoData]
        public async Task And_Invalid_Id_Specified_Then_No_Pledges_Returned(
            int pledgeId,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgeQueryHandler getPledgeQueryHandler)
        {
            GetPledgeQuery getPledgesQuery = new GetPledgeQuery()
            {
                PledgeId = pledgeId,
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == pledgeId)))
                .ReturnsAsync((Pledge)null);

            var result = await getPledgeQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            Assert.IsNull(result);
        }
    }
}