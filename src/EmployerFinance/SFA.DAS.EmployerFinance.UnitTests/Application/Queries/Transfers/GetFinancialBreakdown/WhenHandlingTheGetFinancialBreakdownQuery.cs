using AutoFixture;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetFinancialBreakdown;
using SFA.DAS.EmployerFinance.Models.Constants;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.Transfers.GetFinancialBreakdown;

public class WhenHandlingTheGetFinancialBreakdownQuery
{
        [Test, MoqAutoData]
        public async Task And_AccountId_Specified_Then_Projection_Returned(
           long accountId,
           [Frozen] Mock<IForecastingApiClient<ForecastingApiConfiguration>> forecastingApiConfiguration,
           [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> levyTransferMatchingApiConfiguration,
           GetFinancialBreakdownHandler getFinancialBreakdownHandler)
        {

            var fundOut = new GetTransferFinancialBreakdownResponse.FundsDetails()
            {
                TransferConnections = 200,
                AcceptedPledgeApplications = 200,
                ApprovedPledgeApplications = 200,
                Commitments = 200,
                PledgeOriginatedCommitments = 200
            };

            var breakdownDetails = new GetTransferFinancialBreakdownResponse.BreakdownDetails()
            {                
                FundsOut = fundOut
            };

            var breakDownList = new List<GetTransferFinancialBreakdownResponse.BreakdownDetails>
            {
                breakdownDetails
            };

            GetFinancialBreakdownQuery getFinancialBreakdownQuery = new GetFinancialBreakdownQuery()
            {
                AccountId = accountId,
            };
          
            var getPledgesResponse = new Fixture().Create<GetPledgesResponse>();

            var getTransferFinancialBreakdownResponse = new GetTransferFinancialBreakdownResponse()
            {
                Breakdown = breakDownList,
                AccountId = accountId,                
                AmountPledged = 20
            };

            forecastingApiConfiguration
                .Setup(x => x.Get<GetTransferFinancialBreakdownResponse>(It.IsAny<GetTransferFinancialBreakdownRequest>()))
                .ReturnsAsync(getTransferFinancialBreakdownResponse);

            levyTransferMatchingApiConfiguration
                .Setup(x => x.Get<GetPledgesResponse>(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(getPledgesResponse);

            var results = await getFinancialBreakdownHandler.Handle(getFinancialBreakdownQuery, CancellationToken.None);

            results.AmountPledged.Should().Be(getPledgesResponse.Pledges.Where(p => p.Status != PledgeStatus.Closed).Sum(x => x.Amount));
            results.TransferConnections.Should().Be(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.TransferConnections));
            results.AcceptedPledgeApplications.Should().Be(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications));
            results.ApprovedPledgeApplications.Should().Be(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications));
            results.PledgeOriginatedCommitments.Should().Be(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments));
            results.Commitments.Should().Be(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.Commitments));
        }
}