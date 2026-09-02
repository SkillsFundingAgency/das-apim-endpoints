using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByHashedAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevySummaryByHashedAccountId;

[TestFixture]
internal class WhenHandlingGetLevySummaryByHashedAccountIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Levy_Summary_From_Finance_Api_And_Returns_Result(
        GetLevySummaryByHashedAccountIdQuery query,
        GetLevySummaryByHashedAccountIdResponse apiResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        GetLevySummaryByHashedAccountIdQueryHandler handler)
    {
        mockFinanceApiClient
            .Setup(client => client.Get<GetLevySummaryByHashedAccountIdResponse>(
                It.Is<GetLevySummaryByHashedAccountIdRequest>(r => r.HashedAccountId.Equals(query.HashedAccountId))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentLevyFunds.Should().Be(apiResponse.CurrentLevyFunds);
    }
}