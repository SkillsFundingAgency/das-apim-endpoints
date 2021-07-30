using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_No_AccountId_Specified_Then_All_Pledges_Returned(
            GetPledgesResponse getPledgesResponse,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetPledgesQueryHandler getPledgesQueryHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery();

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(getPledgesResponse);

            var results = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            CollectionAssert.AreEqual(getPledgesResponse.Items, results);
        }
    }
}