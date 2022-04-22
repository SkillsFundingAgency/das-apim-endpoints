using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetFinancialBreakdown;
using SFA.DAS.ManageApprenticeships.Models.Constants;
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

namespace SFA.DAS.ManageApprenticeships.UnitTests.Application.Queries.Transfers.GetFinancialBreakdown
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
            GetFinancialBreakdownQuery getFinancialBreakdownQuery = new GetFinancialBreakdownQuery()
            {
                AccountId = accountId,
            };

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
                FundsIn = 200,
                FundsOut = fundOut
            };

            var breakDownList = new List<GetTransferFinancialBreakdownResponse.BreakdownDetails>
            {
                breakdownDetails
            };

            var getPledgesResponse = new Fixture().Create<GetPledgesResponse>();

            var getTransferFinancialBreakdownResponse = new GetTransferFinancialBreakdownResponse()
            {
                Breakdown = breakDownList,
                AccountId = accountId,
                NumberOfMonths = 12,
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

            Assert.AreEqual(getTransferFinancialBreakdownResponse.NumberOfMonths, results.NumberOfMonths);
            Assert.AreEqual(getPledgesResponse.Pledges.Where(p => p.Status != PledgeStatus.Closed).Sum(x => x.Amount), results.AmountPledged);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.ProjectionStartDate, results.ProjectionStartDate);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown[0].FundsOut.TransferConnections, results.TransferConnections);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown[0].FundsOut.AcceptedPledgeApplications, results.AcceptedPledgeApplications);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown[0].FundsOut.ApprovedPledgeApplications, results.ApprovedPledgeApplications);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown[0].FundsOut.PledgeOriginatedCommitments, results.PledgeOriginatedCommitments);
        }
    }
}
