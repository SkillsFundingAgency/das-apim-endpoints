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
    public async Task Then_Gets_ChangeHistory_Page_And_Total_Count_From_Commitments(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        query.PageNumber = 2;
        query.PageSize = 25;
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId && q.PageNumber == query.PageNumber && q.PageSize == query.PageSize)))
              .ReturnsAsync(response);

        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEquivalentTo(response.ChangeHistory);
        result.TotalCount.Should().Be(response.TotalCount);
        result.AvailableFrom.Should().Be(response.AvailableFrom);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_No_ChangeHistory_For_Provider_When_Response_Is_Null(
           GetAllChangeHistoryForProviderQuery query,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync((GetAllChangeHistoryForProviderResponse)null);

        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Empty_ChangeHistory_When_Response_Change_History_Is_Null(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        response.ChangeHistory = null;
        response.TotalCount = 0;
        response.AvailableFrom = null;
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync(response);

        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.AvailableFrom.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Null_AvailableFrom_When_Response_Has_No_AvailableFrom(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        response.AvailableFrom = null;
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync(response);

        var result = await handler.Handle(query, CancellationToken.None);

        result.AvailableFrom.Should().BeNull();
    }

    [Test]
    public void Then_Builds_Request_Url_With_Paging_Values()
    {
        var request = new GetAllChangeHistoryForProviderRequest(123, 3, 25);

        request.GetUrl.Should().Be("api/change-history/123/Get-all-change-history?pageNumber=3&pageSize=25");
    }
}
