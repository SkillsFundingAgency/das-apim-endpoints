using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Linq;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevySummaryByAccountId;

[TestFixture]
internal class WhenHandlingGetLevySummaryByAccountIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Levy_Summary_From_Finance_Api_And_Returns_Result(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse apiResponse,
        GetEmployerFundingProjectionByAccountIdResponse fundingProjectionResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> mockFundingProjectionApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        mockFinanceApiClient
            .Setup(client => client.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId.Equals(query.AccountId))))
            .ReturnsAsync(apiResponse);

        mockFundingProjectionApiClient
            .Setup(client => client.Get<GetEmployerFundingProjectionByAccountIdResponse>(
                It.Is<GetEmployerFundingProjectionByAccountIdRequest>(r => r.AccountId.Equals(query.AccountId))))
            .ReturnsAsync(fundingProjectionResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.CurrentLevyFunds.Should().Be(apiResponse.CurrentLevyFunds);
        result.TotalLevyDeclaredLast12Months.Should().Be(apiResponse.TotalLevyDeclaredLast12Months);
        result.TotalLevySpentLast12Months.Should().Be(apiResponse.TotalLevySpentLast12Months);
        result.TotalLevyExpiredLast12Months.Should().Be(apiResponse.TotalLevyExpiredLast12Months);
        result.TotalCommittedLearnerCosts.Should().Be(fundingProjectionResponse.FundingBreakdowns.Sum(x => x.CommittedLearnerCost));
        result.TotalCommittedTransfersCosts.Should().Be(fundingProjectionResponse.FundingBreakdowns.Sum(x => x.CommittedTransferOut));
    }
}