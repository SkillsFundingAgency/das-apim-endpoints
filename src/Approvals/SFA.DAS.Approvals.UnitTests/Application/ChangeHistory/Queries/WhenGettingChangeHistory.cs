using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.ChangeHistory.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.ChangeHistory.Queries;

public class WhenGettingChangeHistory
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ChangeHistory(
           GetChangeHistoryQuery query,
           GetChangeHistoryResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetChangeHistoryQueryHandler handler)
    {
        mockClient
              .Setup(client => client.Get<GetChangeHistoryResponse>(It.Is<GetChangeHistoryRequest>(q => q.ApprenticeshipId == query.ApprenticeshipId)))
              .ReturnsAsync(response);
        var result = await handler.Handle(query, CancellationToken.None);

        result.ChangeHistory.Should().BeEquivalentTo(response.ChangeHistory);
    }
}
