using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApproved;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetApplicationApproved
{
    [TestFixture]
    public class GetApplicationApprovedQueryHandlerTests
    {
        private GetApplicationApprovedQueryHandler _handler;
        private string _employerAccountName;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _autoFixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _employerAccountName = _autoFixture.Create<string>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetApplication(It.IsAny<GetApplicationRequest>())).ReturnsAsync(new GetApplicationResponse { EmployerAccountName = _employerAccountName });

            _handler = new GetApplicationApprovedQueryHandler(_levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Handle_Returns_EmployerAccountName()
        {
            var result = await _handler.Handle(new GetApplicationApprovedQuery(), CancellationToken.None);
            Assert.That(result.EmployerAccountName, Is.EqualTo(_employerAccountName));
        }
    }
}
