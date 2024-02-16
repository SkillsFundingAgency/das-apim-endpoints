using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApplicationDetails;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Opportunity.GetApplicationDetails
{
    [TestFixture]
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_Valid_Id_Specified_Then_Opportunity_Returned(
            int opportunityId,
            Pledge opportunity,
            GetStandardsListResponse response,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> client,
            GetApplicationDetailsQueryHandler getApplicationDetailsQueryHandler)
        {
            var getApplicationDetailsQuery = new GetApplicationDetailsQuery()
            {
                OpportunityId = opportunityId,
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == opportunityId)))
                .ReturnsAsync(opportunity);

            client
                .Setup(x => x.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(response);

            var result = await getApplicationDetailsQueryHandler.Handle(getApplicationDetailsQuery, CancellationToken.None);

            Assert.IsNotNull(result.Opportunity);
        }

        [Test, MoqAutoData]
        public async Task And_Invalid_Id_Specified_Then_No_Opportunity_Returned(
            int opportunityId,
            string standardId,
            GetStandardsListResponse response,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> client,
            GetApplicationDetailsQueryHandler getApplicationDetailsQueryHandler)
        {
            var getApplicationDetailsQuery = new GetApplicationDetailsQuery()
            {
                StandardId = standardId,
                OpportunityId = opportunityId
            };

            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == opportunityId)))
                .ReturnsAsync((Pledge)null);

            client
                .Setup(x => x.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(response);

            var result = await getApplicationDetailsQueryHandler.Handle(getApplicationDetailsQuery, CancellationToken.None);

            Assert.That(result.Opportunity, Is.Null);
        }

        [Test, MoqAutoData]
        public async Task And_No_Id_Specified_Then_All_Standards_Returned(
            GetStandardsListResponse response,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> client,
            GetApplicationDetailsQueryHandler getApplicationDetailsQueryHandler)
        {
            var getStandardsQuery = new GetApplicationDetailsQuery();

            client
                .Setup(x => x.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(response);

            var result = await getApplicationDetailsQueryHandler.Handle(getStandardsQuery, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Standards, Is.Not.Null);
            Assert.That(response.Standards.Count(), Is.EqualTo(result.Standards.Count()));
            client.Verify(x => x.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Valid_Id_Specified_Then_Standard_Returned(
            GetStandardsListItem response,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> client,
            GetApplicationDetailsQueryHandler getApplicationDetailsQueryHandler)
        {
            var getStandardsQuery = new GetApplicationDetailsQuery() { StandardId = "1" };

            client
                .Setup(x => x.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(response);

            var result = await getApplicationDetailsQueryHandler.Handle(getStandardsQuery, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.IsNotNull(result.Standards);
            Assert.AreEqual(1, result.Standards.Count());
        }
    }
}
