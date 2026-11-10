using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprovalRequests;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.ApprovalRequest.Queries;

public class WhenGettingApprovalRequests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ApprovalRequests(
           GetApprovalRequestQuery query,
           GetApprovalRequestResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetApprovalRequestQueryHandler handler)
    {
        mockClient
              .Setup(client => client.GetWithResponseCode<GetApprovalRequestResponse>(It.Is<GetApprovalRequest>(q => q.ApprenticeshipId == query.ApprenticeshipId && q.Status == query.Status)))
              .ReturnsAsync(new ApiResponse<GetApprovalRequestResponse>(response, HttpStatusCode.OK, string.Empty));
        var result = await handler.Handle(query, CancellationToken.None);

        result.ApprovalRequests.Should().BeEquivalentTo(response?.ApprovalRequests ?? []);
        result.ApprenticeName.Should().Be(response?.ApprenticeName);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_empty_ApprovalRequests(
           GetApprovalRequestQuery query,
           GetApprovalRequestResponse response,
           [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockClient,
           GetApprovalRequestQueryHandler handler)
    {
        query.ApprenticeshipId = 1;
        mockClient
              .Setup(client => client.GetWithResponseCode<GetApprovalRequestResponse>(It.Is<GetApprovalRequest>(q => q.ApprenticeshipId == query.ApprenticeshipId && q.Status == query.Status)))
              .ReturnsAsync(new ApiResponse<GetApprovalRequestResponse>(new GetApprovalRequestResponse(), HttpStatusCode.OK, string.Empty));
        var result = await handler.Handle(query, CancellationToken.None);

        result.ApprovalRequests.Should().BeEmpty();
        result.ApprenticeName.Should().BeNull();
    }
}