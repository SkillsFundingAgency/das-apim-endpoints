using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Functions.ApplicationsWithAutomaticApproval
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingServiceMock;
        private ApplicationsWithAutomaticApprovalQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _levyTransferMatchingServiceMock = new Mock<ILevyTransferMatchingService>();
            _handler = new ApplicationsWithAutomaticApprovalQueryHandler(_levyTransferMatchingServiceMock.Object);
        }

        [Test]
        public async Task Handle_With_DelayedAutoApprovalChoice_ReturnsExpectedResult()
        {
            // Arrange
            var request = new ApplicationsWithAutomaticApprovalQuery();
            var cancellationToken = CancellationToken.None;

            var applications = new List<GetApplicationsResponse.Application>
            {
                new GetApplicationsResponse.Application { PledgeId = 1, Amount = 1000, PledgeRemainingAmount = 1000, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50) },
                new GetApplicationsResponse.Application { PledgeId = 1, Amount = 500, PledgeRemainingAmount = 1000,MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50) },
                new GetApplicationsResponse.Application { PledgeId = 2, Amount = 500, PledgeRemainingAmount = 600, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50) },
                new GetApplicationsResponse.Application {PledgeId = 2, Amount = 300, PledgeRemainingAmount = 600, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50)},
                new GetApplicationsResponse.Application {PledgeId = 3, Amount = 300, PledgeRemainingAmount = 600, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-10)}
            };        

            _levyTransferMatchingServiceMock.Setup(x => x.GetApplications(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(new GetApplicationsResponse { Applications = applications });
           
            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ApplicationsWithAutomaticApprovalQueryResult>(result);

            var expectedResult = new ApplicationsWithAutomaticApprovalQueryResult
            {
                Applications = new List<ApplicationsWithAutomaticApprovalQueryResult.Application>
                {
                    applications[0],
                    applications[2]
                }
            };

            Assert.AreEqual(expectedResult.Applications.Count(), result.Applications.Count());
        }

        [Test]
        public void GetAutoApplicationDataFromApplicationsResponse_ReturnsExpectedAutoApprovals()
        {
            // Arrange
            var applications = new List<GetApplicationsResponse.Application>
            {
                new GetApplicationsResponse.Application { PledgeId = 1, Amount = 1000, PledgeRemainingAmount = 1000 },
                new GetApplicationsResponse.Application { PledgeId = 1, Amount = 500, PledgeRemainingAmount = 1000 },
                new GetApplicationsResponse.Application { PledgeId = 2, Amount = 500, PledgeRemainingAmount = 600 },
                new GetApplicationsResponse.Application { PledgeId = 2, Amount = 300, PledgeRemainingAmount = 600 }
            };

            // Act
            var result = _handler.GetAutoApplicationDataFromApplicationsResponse(applications);

            // Assert
            var expectedResult = new List<ApplicationsWithAutomaticApprovalQueryResult.Application>
            {
                applications[0],
                applications[2]
            };

            Assert.AreEqual(expectedResult.Count, result.Count);

        }
    }
}
