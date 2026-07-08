using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.ChangeHistory.Queries;

public class WhenGettingAllChangeHistoryForProvider
{
    [Test, MoqAutoData]
    public async Task Then_Gets_All_ChangeHistory_For_Provider(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForEmployerQueryHandler handler)
    {
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync(response);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEquivalentTo(response.ChangeHistory);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_No_ChangeHistory_For_Provider_When_None_Exist(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForEmployerQueryHandler handler)
    {
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync((GetAllChangeHistoryForProviderResponse)null);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeNull();
    }
}