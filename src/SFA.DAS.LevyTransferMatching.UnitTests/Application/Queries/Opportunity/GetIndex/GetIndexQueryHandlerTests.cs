﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Opportunity.GetIndex
{
    [TestFixture]
    public class GetIndexQueryHandlerTests
    {
        private GetIndexQueryHandler _handler;
        private Mock<IReferenceDataService> _referenceDataService;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private List<ReferenceDataItem> _sectors;
        private List<ReferenceDataItem> _jobRoles;
        private List<ReferenceDataItem> _levels;
        private GetIndexQuery _query;
        private GetPledgesResponse _pledges;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _sectors = _autoFixture.Create<List<ReferenceDataItem>>();
            _jobRoles = _autoFixture.Create<List<ReferenceDataItem>>();
            _levels = _autoFixture.Create<List<ReferenceDataItem>>();
            _query = _autoFixture.Create<GetIndexQuery>();
            _pledges = _autoFixture.Create<GetPledgesResponse>();

            _referenceDataService = new Mock<IReferenceDataService>();
            _referenceDataService.Setup(x => x.GetSectors()).ReturnsAsync(_sectors);
            _referenceDataService.Setup(x => x.GetJobRoles()).ReturnsAsync(_jobRoles);
            _referenceDataService.Setup(x => x.GetLevels()).ReturnsAsync(_levels);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>())).ReturnsAsync(_pledges);

            _handler = new GetIndexQueryHandler(_levyTransferMatchingService.Object, _referenceDataService.Object);
        }

        [Test]
        public async Task Handle_Returns_Opportunities()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            CollectionAssert.AreEqual(result.Opportunities.Select(x => x.Id), _pledges.Items.Select(x => x.Id));
        }

        [Test]
        public async Task Handle_Returns_Sectors()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_sectors, result.Sectors);
        }

        [Test]
        public async Task Handle_Returns_JobRoles()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_jobRoles, result.JobRoles);
        }

        [Test]
        public async Task Handle_Returns_Levels()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_levels, result.Levels);
        }
    }
}
