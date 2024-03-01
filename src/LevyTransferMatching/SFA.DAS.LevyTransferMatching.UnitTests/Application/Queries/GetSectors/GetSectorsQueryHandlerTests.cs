using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetSectors;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetSectors
{
    [TestFixture]
    public class GetSectorsQueryHandlerTests
    {
        private GetSectorsQueryHandler _handler;
        private Mock<IReferenceDataService> _referenceDataService;
        private List<ReferenceDataItem> _sectors;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _sectors = _autoFixture.Create<List<ReferenceDataItem>>();
            _referenceDataService = new Mock<IReferenceDataService>();
            _referenceDataService.Setup(x => x.GetSectors()).ReturnsAsync(_sectors);

            _handler = new GetSectorsQueryHandler(_referenceDataService.Object);
        }

        [Test]
        public async Task Handle_Returns_Sectors()
        {
            var result = await _handler.Handle(new GetSectorsQuery(), CancellationToken.None);
            Assert.That(result.ReferenceDataItems, Is.EqualTo(_sectors));
        }
    }
}
