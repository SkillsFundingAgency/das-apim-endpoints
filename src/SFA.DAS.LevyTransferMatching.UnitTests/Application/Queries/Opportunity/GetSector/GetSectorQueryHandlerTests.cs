using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSector;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Opportunity.GetSector
{
    [TestFixture]
    public class GetSectorQueryHandlerTests
    {
        private GetSectorQueryHandler _handler;
        private Mock<IReferenceDataService> _referenceDataService;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private List<ReferenceDataItem> _sectors;
        private GetSectorQuery _query;
        private Pledge _pledge;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _sectors = _autoFixture.Create<List<ReferenceDataItem>>();
            _query = _autoFixture.Create<GetSectorQuery>();
            _pledge = _autoFixture.Create<Pledge>();

            _referenceDataService = new Mock<IReferenceDataService>();
            _referenceDataService.Setup(x => x.GetSectors()).ReturnsAsync(_sectors);
            
            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetPledge(_query.OpportunityId)).ReturnsAsync(_pledge);

            _handler = new GetSectorQueryHandler(_referenceDataService.Object, _levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Handle_Returns_Sectors()
        {
            var result = await _handler.Handle(_query, new System.Threading.CancellationToken());
            Assert.AreEqual(_sectors, result.Sectors);
        }

        [Test]
        public async Task Handle_Returns_Opportunity()
        {
            var result = await _handler.Handle(_query, new System.Threading.CancellationToken());
            Assert.NotNull(result.Opportunity);
            Assert.AreEqual(_pledge.Id, result.Opportunity.Id);
        }
    }
}