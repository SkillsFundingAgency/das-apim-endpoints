using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.LevyTransferMatching;

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

            var applications = new List<GetApplicationsForAutomaticApprovalResponse.Application>
            {
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 1, Amount = 1000, PledgeRemainingAmount = 1000, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50) },
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 1, Amount = 500, PledgeRemainingAmount = 1000, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50) },
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 2, Amount = 500, PledgeRemainingAmount = 600, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50) },
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 2, Amount = 300, PledgeRemainingAmount = 600, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50) },
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 3, Amount = 300, PledgeRemainingAmount = 600, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-10) }
            };

            _levyTransferMatchingServiceMock.Setup(x => x.GetApplicationsForAutomaticApproval(It.IsAny<GetApplicationsForAutomaticApprovalRequest>()))
                .ReturnsAsync(new GetApplicationsForAutomaticApprovalResponse
                {
                    Applications = applications
                });
           
            var result = await _handler.Handle(request, cancellationToken);

            result.Should().NotBeNull();
            result.Should().BeOfType<ApplicationsWithAutomaticApprovalQueryResult>();
            result.Applications.Should().HaveCount(2);
            _levyTransferMatchingServiceMock.Verify(x => x.GetApplicationsForAutomaticApproval(
                It.Is<GetApplicationsForAutomaticApprovalRequest>(sent => sent.PledgeId == null)), Times.Once);
        }

        [Test]
        public async Task Handle_With_PledgeId_Requests_That_Pledge()
        {
            var request = new ApplicationsWithAutomaticApprovalQuery { PledgeId = 2 };
            var applications = new List<GetApplicationsForAutomaticApprovalResponse.Application>
            {
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 2, Amount = 500, PledgeRemainingAmount = 600, MatchPercentage = 100, PledgeAutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval, CreatedOn = DateTime.Now.AddDays(-50) }
            };

            _levyTransferMatchingServiceMock.Setup(x => x.GetApplicationsForAutomaticApproval(It.IsAny<GetApplicationsForAutomaticApprovalRequest>()))
                .ReturnsAsync(new GetApplicationsForAutomaticApprovalResponse
                {
                    Applications = applications
                });

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Applications.Should().ContainSingle();
            result.Applications.Single().PledgeId.Should().Be(2);
            _levyTransferMatchingServiceMock.Verify(x => x.GetApplicationsForAutomaticApproval(
                It.Is<GetApplicationsForAutomaticApprovalRequest>(sent => sent.PledgeId == 2)), Times.Once);
        }

        [Test]
        public void GetAutoApplicationDataFromApplicationsResponse_ReturnsExpectedAutoApprovals()
        {
            // Arrange
            var applications = new List<GetApplicationsForAutomaticApprovalResponse.Application>
            {
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 1, Amount = 1000, PledgeRemainingAmount = 1000 },
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 1, Amount = 500, PledgeRemainingAmount = 1000 },
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 2, Amount = 500, PledgeRemainingAmount = 600 },
                new GetApplicationsForAutomaticApprovalResponse.Application { PledgeId = 2, Amount = 300, PledgeRemainingAmount = 600 }
            };

            // Act
            var result = _handler.GetAutoApplicationDataFromApplicationsResponse(applications);

            // Assert
            var expectedResult = new List<ApplicationsWithAutomaticApprovalQueryResult.Application>
            {
                applications[0],
                applications[2]
            };

            result.Should().HaveCount(expectedResult.Count);

        }
    }
}
