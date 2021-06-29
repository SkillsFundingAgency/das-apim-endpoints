using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_No_Id_Specified_Then_All_Pledges_Returned(
            IEnumerable<Pledge> pledges,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgesHandler getPledgesHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery()
            {
                PledgeId = null, // No pledge ID specified
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledges())
                .ReturnsAsync(pledges);

            var results = await getPledgesHandler.Handle(getPledgesQuery, CancellationToken.None);

            CollectionAssert.AreEqual(pledges, results);
        }

        [Test, MoqAutoData]
        public async Task And_Valid_Id_Specified_Then_One_Pledge_Returned(
            int pledgeId,
            Pledge pledge,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgesHandler getPledgesHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery()
            {
                PledgeId = pledgeId,
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == pledgeId)))
                .ReturnsAsync(pledge);

            var results = await getPledgesHandler.Handle(getPledgesQuery, CancellationToken.None);

            CollectionAssert.AreEqual(new Pledge[] { pledge }, results);
        }

        [Test, MoqAutoData]
        public async Task And_Invalid_Id_Specified_Then_No_Pledges_Returned(
            int pledgeId,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgesHandler getPledgesHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery()
            {
                PledgeId = pledgeId,
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == pledgeId)))
                .ReturnsAsync((Pledge)null);

            var results = await getPledgesHandler.Handle(getPledgesQuery, CancellationToken.None);

            CollectionAssert.AreEqual(new Pledge[] { }, results);
        }
    }
}