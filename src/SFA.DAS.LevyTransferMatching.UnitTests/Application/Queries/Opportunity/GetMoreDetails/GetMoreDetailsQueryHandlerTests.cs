using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetMoreDetails;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Opportunity.GetMoreDetails
{
    [TestFixture]
    public class GetMoreDetailsQueryHandlerTests
    {
        private GetMoreDetailsQueryHandler _handler;
        private Mock<IReferenceDataService> _referenceDataService;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private List<ReferenceDataItem> _sectors;
        private List<ReferenceDataItem> _jobRoles;
        private List<ReferenceDataItem> _levels;
        private GetMoreDetailsQuery _query;
        private Pledge _pledge;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _sectors = _autoFixture.Create<List<ReferenceDataItem>>();
            _jobRoles = _autoFixture.Create<List<ReferenceDataItem>>();
            _levels = _autoFixture.Create<List<ReferenceDataItem>>();
            _query = _autoFixture.Create<GetMoreDetailsQuery>();
            _pledge = _autoFixture.Create<Pledge>();

            _referenceDataService = new Mock<IReferenceDataService>();
            _referenceDataService.Setup(x => x.GetSectors()).ReturnsAsync(_sectors);
            _referenceDataService.Setup(x => x.GetJobRoles()).ReturnsAsync(_jobRoles);
            _referenceDataService.Setup(x => x.GetLevels()).ReturnsAsync(_levels);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetPledge(_query.OpportunityId)).ReturnsAsync(_pledge);

            _handler = new GetMoreDetailsQueryHandler(_referenceDataService.Object, _levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Handle_Returns_Opportunity()
        {
            var result = await _handler.Handle(_query, new System.Threading.CancellationToken());
            Assert.NotNull(result.Opportunity);
            Assert.AreEqual(_pledge.Id, result.Opportunity.Id);
        }

        [Test]
        public async Task Handle_Returns_Sectors()
        {
            var result = await _handler.Handle(_query, new System.Threading.CancellationToken());
            Assert.AreEqual(_sectors, result.Sectors);
        }

        [Test]
        public async Task Handle_Returns_JobRoles()
        {
            var result = await _handler.Handle(_query, new System.Threading.CancellationToken());
            Assert.AreEqual(_jobRoles, result.JobRoles);
        }

        [Test]
        public async Task Handle_Returns_Levels()
        {
            var result = await _handler.Handle(_query, new System.Threading.CancellationToken());
            Assert.AreEqual(_levels, result.Levels);
        }
    }
}
