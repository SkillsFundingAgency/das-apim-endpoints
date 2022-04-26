using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetAccountProjectionSummary;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ManageApprenticeships.UnitTests.Application.Queries.GetAccountProjectionSummary
{
    public class WhenHandlingAccountProjectionSummaryQuery
    {
        [Test, MoqAutoData]
        public async Task Then_combines_summary_from_forecasting_api(
            GetAccountProjectionSummaryQuery query,
            GetProjectionCalculationResponse projectionCalcApiResponse,
            GetExpiringFundsResponse expiringFundsApiResponse,
            [Frozen] Mock<IForecastingApiClient<ForecastingApiConfiguration>> mockApiClient,
            GetAccountProjectionSummaryQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetProjectionCalculationResponse>(It.Is<GetProjectionCalculationRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
                .ReturnsAsync(projectionCalcApiResponse);

            mockApiClient
                .Setup(client => client.Get<GetExpiringFundsResponse>(It.Is<GetExpiringFundsRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
                .ReturnsAsync(expiringFundsApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(projectionCalcApiResponse, opts => opts.ExcludingMissingMembers());
            result.ExpiryAmounts.Should().BeEquivalentTo(expiringFundsApiResponse.ExpiryAmounts);
        }

        [Test, MoqAutoData]
        public async Task Then_maps_null_response_from_forecasting_api(
            GetAccountProjectionSummaryQuery query,
            [Frozen] Mock<IForecastingApiClient<ForecastingApiConfiguration>> mockApiClient,
            GetAccountProjectionSummaryQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetProjectionCalculationResponse>(It.Is<GetProjectionCalculationRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
                .ReturnsAsync((GetProjectionCalculationResponse)null);

            mockApiClient
                .Setup(client => client.Get<GetExpiringFundsResponse>(It.Is<GetExpiringFundsRequest>(x => x.GetUrl.Contains(query.AccountId.ToString()))))
                .ReturnsAsync((GetExpiringFundsResponse)null);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ExpiryAmounts.Should().BeEmpty();
            result.NumberOfMonths.Should().Be(0);
            result.FundsIn.Should().Be(0);
            result.FundsOut.Should().Be(0);
        }
    }
}
