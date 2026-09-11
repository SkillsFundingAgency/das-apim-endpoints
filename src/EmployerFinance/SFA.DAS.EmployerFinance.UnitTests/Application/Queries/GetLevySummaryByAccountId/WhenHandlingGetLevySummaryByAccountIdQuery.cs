using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevySummaryByAccountId;

[TestFixture]
internal class WhenHandlingGetLevySummaryByAccountIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Levy_Summary_From_Finance_Api_And_Returns_Result(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse apiResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        mockFinanceApiClient
            .Setup(client => client.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId.Equals(query.AccountId))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentLevyFunds.Should().Be(apiResponse.CurrentLevyFunds);
        result.TotalLevyDeclaredLast12Months.Should().Be(apiResponse.TotalLevyDeclaredLast12Months);
        result.TotalLevySpentLast12Months.Should().Be(apiResponse.TotalLevySpentLast12Months);
    }
}