using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetPledges;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetPledges
{
    public class WhenHandlingTheGetPledgesQuery
    {
        [Test, MoqAutoData]
        public async Task And_AccountId_Specified_Then_Pledges_Returned(
            long accountId,
            GetPledgesResponse getPledgesResponse,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockLevyTransferMatchingClient,
            GetPledgesQueryHandler getPledgesQueryHandler)
        {
            GetPledgesQuery getPledgesQuery = new GetPledgesQuery()
            {
                AccountId = accountId,
            };

            mockLevyTransferMatchingClient
                .Setup(x => x.Get<GetPledgesResponse>(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(getPledgesResponse);

            var results = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            CollectionAssert.AreEqual(getPledgesResponse.Pledges, results.Pledges);
            Assert.That(getPledgesResponse.TotalPledges, Is.EqualTo(results.TotalPledges));
        }
    }
}