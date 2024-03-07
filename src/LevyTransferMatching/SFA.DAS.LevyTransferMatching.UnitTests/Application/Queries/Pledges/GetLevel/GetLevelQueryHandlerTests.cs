using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetLevel;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetLevel
{
    [TestFixture]
    public class GetLevelQueryHandlerTests
    {
        private GetLevelQueryHandler _handler;
        private Mock<IReferenceDataService> _referenceDataService;
        private List<ReferenceDataItem> _levels;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _levels = _autoFixture.Create<List<ReferenceDataItem>>();
            _referenceDataService = new Mock<IReferenceDataService>();
            _referenceDataService.Setup(x => x.GetLevels()).ReturnsAsync(_levels);

            _handler = new GetLevelQueryHandler(_referenceDataService.Object);
        }

        [Test]
        public async Task Handle_Returns_Levels()
        {
            var result = await _handler.Handle(new GetLevelQuery(), CancellationToken.None);
            Assert.That(result.Levels, Is.EqualTo(_levels));
        }
    }
}