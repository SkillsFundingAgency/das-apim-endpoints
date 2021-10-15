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
        private Mock<IReferenceDataService> _referenceDataService;
        private List<ReferenceDataItem> _jobRoles;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _jobRoles = _autoFixture.Create<List<ReferenceDataItem>>();
            _referenceDataService = new Mock<IReferenceDataService>();
            _referenceDataService.Setup(x => x.GetJobRoles()).ReturnsAsync(_jobRoles);

            _handler = new GetJobRolesQueryHandler(_referenceDataService.Object);
        }

        [Test]
        public async Task Handle_Returns_JobRoles()
        {
            var result = await _handler.Handle(new GetJobRolesQuery(), CancellationToken.None);
            Assert.AreEqual(_jobRoles, result.ReferenceDataItems);
        }
    }
}
