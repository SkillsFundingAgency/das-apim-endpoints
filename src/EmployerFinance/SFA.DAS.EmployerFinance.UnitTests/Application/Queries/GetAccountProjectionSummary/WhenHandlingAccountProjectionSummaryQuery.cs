using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetAccountProjectionSummary
{
    public class WhenHandlingAccountProjectionSummaryQuery
    {
        [Test, MoqAutoData]
        public async Task Then_combines_summary_from_forecasting_api_and_finance_api(
            GetAccountProjectionSummaryQuery query,
            GetProjectionCalculationResponse projectionCalcApiResponse,
            GetExpiringFundsResponse expiringFundsApiResponse,
            GetAccountProjectionSummaryFromFinanceResponse financeSummaryResponse,
            [Frozen] Mock<IForecastingApiClient<ForecastingApiConfiguration>> forecastingApiClient,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
            GetAccountProjectionSummaryQueryHandler handler)
        {
            projectionCalcApiResponse.AccountId = query.AccountId;

            forecastingApiClient
                .Setup(client => client.Get<GetProjectionCalculationResponse>(It.Is<GetProjectionCalculationRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
                .ReturnsAsync(projectionCalcApiResponse);
            
            forecastingApiClient
                .Setup(client => client.Get<GetExpiringFundsResponse>(It.Is<GetExpiringFundsRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
                .ReturnsAsync(expiringFundsApiResponse);

            financeApiClient
               .Setup(client => client.Get<GetAccountProjectionSummaryFromFinanceResponse>(It.Is<GetAccountProjectionSummaryFromFinanceRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
               .ReturnsAsync(financeSummaryResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AccountId.Should().Be(query.AccountId);
            result.FundsIn.Should().Be(financeSummaryResponse.FundsIn);
            result.FundsOut.Should().Be(projectionCalcApiResponse.FundsOut);
            result.NumberOfMonths.Should().Be(projectionCalcApiResponse.NumberOfMonths);
            result.ProjectionGenerationDate.Should().Be(projectionCalcApiResponse.ProjectionGenerationDate);
            result.ExpiryAmounts.Should().BeEquivalentTo(expiringFundsApiResponse.ExpiryAmounts);
        }

        [Test, MoqAutoData]
        public async Task Then_maps_null_response_from_forecasting_api_and_finance_api(
            GetAccountProjectionSummaryQuery query,
            [Frozen] Mock<IForecastingApiClient<ForecastingApiConfiguration>> forecastingApiClient,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
            GetAccountProjectionSummaryQueryHandler handler)
        {
            forecastingApiClient
                .Setup(client => client.Get<GetProjectionCalculationResponse>(It.Is<GetProjectionCalculationRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
                .ReturnsAsync((GetProjectionCalculationResponse)null);

            forecastingApiClient
                .Setup(client => client.Get<GetExpiringFundsResponse>(It.Is<GetExpiringFundsRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
                .ReturnsAsync((GetExpiringFundsResponse)null);

            financeApiClient
              .Setup(client => client.Get<GetAccountProjectionSummaryFromFinanceResponse>(It.Is<GetAccountProjectionSummaryFromFinanceRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
              .ReturnsAsync(() => null);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ExpiryAmounts.Should().BeEmpty();
            result.NumberOfMonths.Should().Be(0);
            result.FundsIn.Should().Be(0);
            result.FundsOut.Should().Be(0);
        }
    }
}
