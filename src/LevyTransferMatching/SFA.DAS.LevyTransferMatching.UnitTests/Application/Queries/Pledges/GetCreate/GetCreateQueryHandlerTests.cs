using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetCreate;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetCreate
{
    [TestFixture]
    public class GetCreateQueryHandlerTests
    {
        private GetCreateQueryHandler _handler;
        private Mock<IReferenceDataService> _referenceDataService;
        private List<ReferenceDataItem> _jobRoles;
        private List<ReferenceDataItem> _sectors;
        private List<ReferenceDataItem> _levels;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _jobRoles = _autoFixture.Create<List<ReferenceDataItem>>();
            _sectors = _autoFixture.Create<List<ReferenceDataItem>>();
            _levels = _autoFixture.Create<List<ReferenceDataItem>>();

            _referenceDataService = new Mock<IReferenceDataService>();
            _referenceDataService.Setup(x => x.GetJobRoles()).ReturnsAsync(_jobRoles);
            _referenceDataService.Setup(x => x.GetSectors()).ReturnsAsync(_sectors);
            _referenceDataService.Setup(x => x.GetLevels()).ReturnsAsync(_levels);

            _handler = new GetCreateQueryHandler(_referenceDataService.Object);
        }

        [Test]
        public async Task Handle_Returns_JobRoles()
        {
            var result = await _handler.Handle(new GetCreateQuery(), CancellationToken.None);
            Assert.That(result.JobRoles, Is.EqualTo(_jobRoles));
        }

        [Test]
        public async Task Handle_Returns_Sectors()
        {
            var result = await _handler.Handle(new GetCreateQuery(), CancellationToken.None);
            Assert.That(result.Sectors, Is.EqualTo(_sectors));
        }

        [Test]
        public async Task Handle_Returns_Levels()
        {
            var result = await _handler.Handle(new GetCreateQuery(), CancellationToken.None);
            Assert.That(result.Levels, Is.EqualTo(_levels));
        }
    }
}
