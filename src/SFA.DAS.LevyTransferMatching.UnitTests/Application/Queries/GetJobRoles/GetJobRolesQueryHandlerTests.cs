using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetJobRoles
{
    [TestFixture]
    public class GetJobRolesQueryHandlerTests
    {
        private GetJobRolesQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private IEnumerable<ReferenceDataItem> _jobRoles;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _jobRoles = _autoFixture.Create<List<ReferenceDataItem>>();
            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetJobRoles()).ReturnsAsync(_jobRoles);

            _handler = new GetJobRolesQueryHandler(_levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Handle_Returns_Sectors()
        {
            var result = await _handler.Handle(new GetJobRolesQuery(), CancellationToken.None);
            Assert.AreEqual(_jobRoles, result.ReferenceDataItems);
        }
    }
}
