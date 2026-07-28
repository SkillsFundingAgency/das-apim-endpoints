using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.ChangeHistory.Queries;

public class WhenGettingAllChangeHistoryForEmployer
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ChangeHistory_For_Employer(
           GetAllChangeHistoryForEmployerQuery query,
           GetAllChangeHistoryForEmployerResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForEmployerQueryHandler handler)
    {
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForEmployerResponse>(It.Is<GetAllChangeHistoryForEmployerRequest>(q => q.AccountId == query.AccountId)))
              .ReturnsAsync(response);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEquivalentTo(response.ChangeHistory);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_No_ChangeHistory_For_Employer_When_None_Exist(
           GetAllChangeHistoryForEmployerQuery query,
           GetAllChangeHistoryForEmployerResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetAllChangeHistoryForEmployerQueryHandler handler)
    {
        mockClient
              .Setup(client => client.Get<GetAllChangeHistoryForEmployerResponse>(It.Is<GetAllChangeHistoryForEmployerRequest>(q => q.AccountId == query.AccountId)))
              .ReturnsAsync((GetAllChangeHistoryForEmployerResponse)null);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEmpty();
    }
}