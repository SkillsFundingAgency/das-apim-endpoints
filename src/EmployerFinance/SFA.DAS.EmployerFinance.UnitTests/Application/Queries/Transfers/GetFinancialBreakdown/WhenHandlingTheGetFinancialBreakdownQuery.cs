using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetFinancialBreakdown;
using SFA.DAS.EmployerFinance.Models.Constants;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.Transfers.GetFinancialBreakdown
{
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
                AmountPledged = 20,
                ProjectionStartDate = DateTime.Now
            };

            forecastingApiConfiguration
                .Setup(x => x.Get<GetTransferFinancialBreakdownResponse>(It.IsAny<GetTransferFinancialBreakdownRequest>()))
                .ReturnsAsync(getTransferFinancialBreakdownResponse);

            levyTransferMatchingApiConfiguration
                .Setup(x => x.Get<GetPledgesResponse>(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(getPledgesResponse);

            var results = await getFinancialBreakdownHandler.Handle(getFinancialBreakdownQuery, CancellationToken.None);

            Assert.That(getPledgesResponse.Pledges.Where(p => p.Status != PledgeStatus.Closed).Sum(x => x.Amount), Is.EqualTo(results.AmountPledged));
            Assert.That(getTransferFinancialBreakdownResponse.ProjectionStartDate, Is.EqualTo(results.ProjectionStartDate));
            Assert.That(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.TransferConnections), Is.EqualTo(results.TransferConnections));
            Assert.That(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications), Is.EqualTo(results.AcceptedPledgeApplications));
            Assert.That(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications), Is.EqualTo(results.ApprovedPledgeApplications));
            Assert.That(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments), Is.EqualTo(results.PledgeOriginatedCommitments));
            Assert.That(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.Commitments), Is.EqualTo(results.Commitments));            
            Assert.That((getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications) +
                getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications) 
                + getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments)
                + getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.TransferConnections)), Is.EqualTo(results.CurrentYearEstimatedCommittedSpend));
        }
    }
}