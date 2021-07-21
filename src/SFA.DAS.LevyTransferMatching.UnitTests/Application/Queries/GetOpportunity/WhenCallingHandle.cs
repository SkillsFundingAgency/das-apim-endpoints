using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetOpportunity;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetOpportunity
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_Valid_Id_Specified_Then_Opportunity_Returned(
            int opportunityId,
            Pledge opportunity,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetOpportunityQueryHandler getOpportunityQueryHandler)
        {
            GetOpportunityQuery getOpportunityQuery = new GetOpportunityQuery()
            {
                OpportunityId = opportunityId,
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == opportunityId)))
                .ReturnsAsync(opportunity);

            var result = await getOpportunityQueryHandler.Handle(getOpportunityQuery, CancellationToken.None);

            Assert.IsNotNull(result);
        }

        [Test, MoqAutoData]
        public async Task And_Invalid_Id_Specified_Then_No_Opportunity_Returned(
            int opportunityId,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetOpportunityQueryHandler getOpportunityQueryHandler)
        {
            GetOpportunityQuery getOpportunityQuery = new GetOpportunityQuery()
            {
                OpportunityId = opportunityId,
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == opportunityId)))
                .ReturnsAsync((Pledge)null);

            var result = await getOpportunityQueryHandler.Handle(getOpportunityQuery, CancellationToken.None);

            Assert.IsNull(result);
        }
    }
}