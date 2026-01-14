using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.LevyTransferMatching.Queries.GetApprovedAccountApplication;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.SelectAcceptedEmployerAccountApplications;

public class WhenGettingAcceptedEmployerAccountApplications
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_To_GetAcceptedEmployerAccountApplications_Returns_ExpectedValues(
        GetAcceptedEmployerAccountApplicationsQuery query,
        GetApplicationsResponse response,
        [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> client,
        GetAcceptedEmployerAccountApplicationsQueryHandler handler
    )
    {
        client.Setup(x =>
                x.Get<GetApplicationsResponse>(
                    It.Is<GetAcceptedEmployerAccountPledgeApplicationsRequest>(x =>
                        x.EmployerAccountId == query.EmployerAccountId)))
            .ReturnsAsync(response);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Applications.Should().BeEquivalentTo(response.Applications);
    }
}