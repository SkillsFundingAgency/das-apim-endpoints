﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetRejectApplications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetRejectApplications
{
    public class GetRejectApplicationsQueryHandlerTests
    {
        private GetRejectApplicationsQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _service;
        private GetRejectApplicationsQuery _query;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _query = _fixture.Create<GetRejectApplicationsQuery>();
            _service = new Mock<ILevyTransferMatchingService>();
            _service.Setup(x => x.GetApplications(It.Is<GetApplicationsRequest>(p => p.PledgeId == _query.PledgeId 
                                && p.ApplicationStatusFilter == ApplicationStatus.Pending)))
                .ReturnsAsync(new GetApplicationsResponse()
                {
                    Applications = new List<GetApplicationsResponse.Application>
                    {
                        new GetApplicationsResponse.Application
                        {
                            DasAccountName = "Mega Corp",
                            Id = 4
                        },
                        new GetApplicationsResponse.Application
                        {
                            DasAccountName = "Mega Corp",
                            Id = 5
                        }
                    }
                });
            _handler = new GetRejectApplicationsQueryHandler(_service.Object);
        }

        [Test]
        public async Task Handle_Returns_EmployerAccountName()
        {
            var result = await _handler.Handle(_query , CancellationToken.None);
            var applications = result.Applications.Select(x => x).ToList();
            Assert.That(applications[0].DasAccountName, Is.EqualTo("Mega Corp"));
            Assert.That(applications[0].Id, Is.EqualTo(4));
        }
    }
}
