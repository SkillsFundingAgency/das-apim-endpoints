﻿using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Functions.GetApplicationsForAutomaticRejection
{
    [TestFixture]
    public class WhenCallingHandle
    {

        private static readonly DateTime _threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);
        private static readonly DateTime _autoApprovalGoLiveDate = new DateTime(2023, 8, 24);

        private Mock<ILevyTransferMatchingService> _levyTransferMatchingServiceMock;
        private GetApplicationsForAutomaticRejectionQueryHandler _handler;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _levyTransferMatchingServiceMock = new Mock<ILevyTransferMatchingService>();
            _handler = new GetApplicationsForAutomaticRejectionQueryHandler(_levyTransferMatchingServiceMock.Object);
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_ValidRequest_Includes_Pre_Release_applications_over_3_month_old()
        {
            // Arrange
            var request = _fixture.Create<GetApplicationsForAutomaticRejectionQuery>();
            var applications = new List<GetApplicationsResponse.Application>
            {
                new GetApplicationsResponse.Application { Id = 1, PledgeId = 1, PledgeCreatedOn = _autoApprovalGoLiveDate.AddMonths(-12), MatchPercentage = 100, CreatedOn = _threeMonthsAgo.AddMonths(-1) },
                new GetApplicationsResponse.Application { Id = 2, PledgeId = 2, PledgeCreatedOn = _autoApprovalGoLiveDate.AddMonths(1),  MatchPercentage = 100, CreatedOn = _threeMonthsAgo.AddMonths(1) },
                new GetApplicationsResponse.Application { Id = 3, PledgeId = 3, PledgeCreatedOn = _autoApprovalGoLiveDate.AddMonths(-12), MatchPercentage = 100, CreatedOn = _threeMonthsAgo.AddMonths(-6) }
            };

            _levyTransferMatchingServiceMock.Setup(x => x.GetApplications(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(new GetApplicationsResponse { Applications = applications });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Applications.Any(), Is.True);
            Assert.That(result.Applications.Count() == 2);
        }

        [Test]
        public async Task Handle_ValidRequest_Includes_Post_Release_Non_100_Match_applications_over_3_month_old()
        {
            // Arrange
            var request = _fixture.Create<GetApplicationsForAutomaticRejectionQuery>();
            var applications = new List<GetApplicationsResponse.Application>
            {
                new GetApplicationsResponse.Application { Id = 1, PledgeId = 1, PledgeCreatedOn = _autoApprovalGoLiveDate.AddMonths(1), MatchPercentage = 50, CreatedOn = _threeMonthsAgo.AddMonths(-1) },
                new GetApplicationsResponse.Application { Id = 2, PledgeId = 2, PledgeCreatedOn = _autoApprovalGoLiveDate.AddMonths(1),  MatchPercentage = 50, CreatedOn = _threeMonthsAgo.AddMonths(1) },
                new GetApplicationsResponse.Application { Id = 3, PledgeId = 3, PledgeCreatedOn = _autoApprovalGoLiveDate.AddMonths(1), MatchPercentage = 100, CreatedOn = _threeMonthsAgo.AddMonths(1) }
            };

            _levyTransferMatchingServiceMock.Setup(x => x.GetApplications(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(new GetApplicationsResponse { Applications = applications });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Applications.Any(), Is.True);
            Assert.That(result.Applications.Count() == 1);
        }
    }
}
