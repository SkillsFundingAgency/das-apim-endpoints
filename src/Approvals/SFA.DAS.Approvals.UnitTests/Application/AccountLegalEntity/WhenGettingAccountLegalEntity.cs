using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.AccountLegalEntity;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.AccountLegalEntity
{
    public class WhenGettingAccountLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_AccountLegalEntity_Is_Returned(
           GetAccountLegalEntityQuery query,
           GetAccountLegalEntityResponse apiResponse,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
           GetAccountLegalEntityQueryHandler handler
           )
        {
            apiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.Is<GetAccountLegalEntityRequest>(x => x.AccountLegalEntityId == query.AccountLegalEntityId))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo((GetAccountLegalEntityQueryResult)apiResponse);
        }
    }
}
