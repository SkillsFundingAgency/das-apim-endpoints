using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetIndex;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ManageApprenticeships.UnitTests.Application.Queries.Transfers.GetIndex
{
    public class WhenHandlingTheGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task And_AccountId_Specified_Then_Index_Returned(
            long accountId,
            GetPledgesResponse getPledgesResponse,
            GetAccountTransferStatusResponse getAccountTransferStatusResponse,
            GetApplicationsResponse getApplicationsResponse,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> levyTransferMatchingClient,
            GetIndexQueryHandler getIndexQueryHandler)
        {
            GetIndexQuery getIndexQuery = new GetIndexQuery()
            {
                AccountId = accountId,
            };

            levyTransferMatchingClient
                .Setup(x => x.Get<GetPledgesResponse>(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(getPledgesResponse);

            levyTransferMatchingClient
                .Setup(x => x.Get<GetApplicationsResponse>(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(getApplicationsResponse);

            var results = await getIndexQueryHandler.Handle(getIndexQuery, CancellationToken.None);

            Assert.AreEqual(getPledgesResponse.TotalPledges, results.PledgesCount);
            Assert.AreEqual(getApplicationsResponse.Applications.Count(), results.ApplicationsCount);
        }
    }
}
