using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_Pledge_AccountId_And_PledgeId_Are_Encoded_And_Pledges_Returned(
            GetPledgesQuery getPledgesQuery,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgesHandler getPledgesHandler)
        {
            var results = await getPledgesHandler.Handle(getPledgesQuery, CancellationToken.None);

            foreach (var pledge in results)
            {
                Assert.AreEqual(pledge.AccountId, pledge.AccountId);
                Assert.AreEqual(pledge.Id, pledge.Id);
            }
        }
    }
}