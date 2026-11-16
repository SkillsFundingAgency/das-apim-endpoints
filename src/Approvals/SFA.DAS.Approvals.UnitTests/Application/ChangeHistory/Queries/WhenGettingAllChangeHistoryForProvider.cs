using System.Linq;
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
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        query.PageNumber = 1;
        query.PageSize = 100;
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync(response);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEquivalentTo(response.ChangeHistory);
        result.TotalCount.Should().Be(response.ChangeHistory.Count());
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Requested_Page_Of_ChangeHistory_For_Provider(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        response.ChangeHistory = response.ChangeHistory.Take(3).ToList();
        query.PageNumber = 2;
        query.PageSize = 2;
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync(response);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEquivalentTo(response.ChangeHistory.Skip(2).Take(2));
        result.TotalCount.Should().Be(3);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_No_ChangeHistory_For_Provider_When_None_Exist(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync((GetAllChangeHistoryForProviderResponse)null);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Empty_Page_And_Total_Count_When_Page_Is_Beyond_Last_Page(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        response.ChangeHistory = response.ChangeHistory.Take(3).ToList();
        query.PageNumber = 10;
        query.PageSize = 2;
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync(response);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEmpty();
        result.TotalCount.Should().Be(3);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Empty_Result_When_Response_Change_History_Is_Null(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        response.ChangeHistory = null;
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync(response);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Empty_Page_When_Page_Number_Causes_Offset_Overflow(
           GetAllChangeHistoryForProviderQuery query,
           GetAllChangeHistoryForProviderResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForProviderQueryHandler handler)
    {
        response.ChangeHistory = response.ChangeHistory.Take(3).ToList();
        query.PageNumber = int.MaxValue;
        query.PageSize = 100;
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForProviderResponse>(It.Is<GetAllChangeHistoryForProviderRequest>(q => q.ProviderId == query.ProviderId)))
              .ReturnsAsync(response);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEmpty();
        result.TotalCount.Should().Be(3);
    }
}