using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetFinancialBreakdown;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
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
           GetTransferFinancialBreakdownResponse getTransferFinancialBreakdownResponse,
           [Frozen] Mock<IForecastingApiClient<ForecastingApiConfiguration>> forecastingApiConfiguration,
           GetFinancialBreakdownHandler getFinancialBreakdownHandler)
        {
            var getFinancialBreakdownQuery = new GetFinancialBreakdownQuery()
            {
                AccountId = accountId,
            };

            forecastingApiConfiguration
                .Setup(x => x.Get<GetTransferFinancialBreakdownResponse>(It.IsAny<GetTransferFinancialBreakdownRequest>()))
                .ReturnsAsync(getTransferFinancialBreakdownResponse);
            
            var results = await getFinancialBreakdownHandler.Handle(getFinancialBreakdownQuery, CancellationToken.None);

            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.TransferConnections), results.TransferConnections);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications), results.AcceptedPledgeApplications);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications), results.ApprovedPledgeApplications);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments), results.PledgeOriginatedCommitments);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsOut.Commitments), results.Commitments);
            Assert.AreEqual(getTransferFinancialBreakdownResponse.Breakdown.Sum(x => x.FundsIn), results.FundsIn);            
        }
    }
}