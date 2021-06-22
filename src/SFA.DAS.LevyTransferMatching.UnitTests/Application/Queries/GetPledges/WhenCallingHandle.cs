using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Encoding;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_Pledge_AccountId_And_PledgeId_Are_Encoded_And_All_Pledges_Returned(
            [Frozen] Mock<IEncodingService> mockEncodingService,
            GetPledgesHandler getPledgesHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery()
            {
                // Get all pledges
            };

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

        [Test, MoqAutoData]
        public async Task And_EncodedId_Requested_Incorrect_Format_Then_No_Pledges_Returned(
            string encodedId,
            [Frozen] Mock<IEncodingService> mockEncodingService,
            GetPledgesHandler getPledgesHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery()
            {
                EncodedId = encodedId,
            };

            mockEncodingService
                .Setup(x => x.Decode(It.Is<string>(y => y == encodedId), It.Is<EncodingType>(y => y == EncodingType.PledgeId)))
                .Throws<IndexOutOfRangeException>();

            var results = await getPledgesHandler.Handle(getPledgesQuery, CancellationToken.None);

            Assert.AreEqual(results.Count(), 0);
        }

        [Test, MoqAutoData]
        public async Task And_EncodedId_Requested_Then_Pledge_AccountId_And_PledgeId_Are_Encoded_And_One_Pledge_Returned(
            string encodedId,
            long id,
            Pledge expectedPledge,
            [Frozen] Mock<IEncodingService> mockEncodingService,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgesHandler getPledgesHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery()
            {
                EncodedId = encodedId,
            };

            Func<long, EncodingType, string> fakeEncoder =
                (value, encodingType) =>
                {
                    return $"{encodingType}_{value}";
                };

            mockEncodingService
                .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(y => y == EncodingType.AccountId || y == EncodingType.PledgeId)))
                .Returns(fakeEncoder);

            mockEncodingService
                .Setup(x => x.Decode(It.Is<string>(y => y == encodedId), It.Is<EncodingType>(y => y == EncodingType.PledgeId)))
                .Returns(id);

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == id)))
                .ReturnsAsync(expectedPledge);

            var results = await getPledgesHandler.Handle(getPledgesQuery, CancellationToken.None);

            var actualPledge = results.SingleOrDefault();

            Assert.IsNotNull(actualPledge);
            Assert.AreEqual(expectedPledge, actualPledge);
            Assert.AreEqual(actualPledge.EncodedAccountId, $"{EncodingType.AccountId}_{actualPledge.AccountId}");
            Assert.AreEqual(actualPledge.EncodedPledgeId, $"{EncodingType.PledgeId}_{actualPledge.Id}");
        }
    }
}