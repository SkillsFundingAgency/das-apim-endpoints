using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Encoding;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_Pledge_AccountId_And_PledgeId_Are_Encoded_And_Pledges_Returned(
            GetPledgesQuery getPledgesQuery,
            [Frozen] Mock<IEncodingService> mockEncodingService,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgesHandler getPledgesHandler)
        {
            Func<long, EncodingType, string> fakeEncoder =
                (value, encodingType) =>
                {
                    return $"{encodingType}_{value}";
                };

            mockEncodingService
                .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(y => y == EncodingType.AccountId || y == EncodingType.PledgeId)))
                .Returns(fakeEncoder);

            var results = await getPledgesHandler.Handle(getPledgesQuery, CancellationToken.None);

            foreach (var pledge in results)
            {
                Assert.AreEqual(pledge.EncodedAccountId, $"{EncodingType.AccountId}_{pledge.AccountId}");
                Assert.AreEqual(pledge.EncodedPledgeId, $"{EncodingType.PledgeId}_{pledge.Id}");
            }
        }
    }
}