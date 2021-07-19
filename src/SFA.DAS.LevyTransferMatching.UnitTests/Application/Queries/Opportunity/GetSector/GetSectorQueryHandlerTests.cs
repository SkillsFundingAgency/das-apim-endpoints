using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSector;
using SFA.DAS.LevyTransferMatching.Interfaces;
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
        private Mock<ILocationLookupService> _locationLookupService;
        private List<ReferenceDataItem> _sectors;
        private GetSectorQuery _query;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _sectors = _autoFixture.Create<List<ReferenceDataItem>>();
            _query = _autoFixture.Create<GetSectorQuery>();

            _referenceDataService = new Mock<IReferenceDataService>();
            _referenceDataService.Setup(x => x.GetSectors()).ReturnsAsync(_sectors);

            _locationLookupService = new Mock<ILocationLookupService>();
            _locationLookupService.Setup(x => x.GetLocationInformation(_query.Postcode, 0, 0, true)).Returns(Task.FromResult(new LocationItem(_query.Postcode, new double[2])));

            _handler = new GetSectorQueryHandler(_referenceDataService.Object, _locationLookupService.Object);
        }

        [Test]
        public async Task Handle_Returns_Sectors()
        {
            var result = await _handler.Handle(_query, new System.Threading.CancellationToken());
            Assert.AreEqual(_sectors, result.Sectors);
        }

        [Test]
        public async Task Handle_Returns_Location()
        {
            var result = await _handler.Handle(_query, new System.Threading.CancellationToken());
            Assert.AreEqual(_query.Postcode, result.Location);
        }
    }
}
