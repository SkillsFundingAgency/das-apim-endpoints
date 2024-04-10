using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunities.GetConfirmation;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Opportunities
{
    [TestFixture]
    public class GetConfirmationQueryHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();
        private GetConfirmationQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private GetConfirmationQuery _query;
        private Pledge _pledge;

        [SetUp]
        public void SetUp()
        {
            _query = _fixture.Create<GetConfirmationQuery>();
            _pledge = _fixture.Create<Pledge>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.GetPledge(_query.OpportunityId))
                .ReturnsAsync(_pledge);

            _handler = new GetConfirmationQueryHandler(_levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Handle_Returns_Confirmation_Result()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(result.AccountName, Is.EqualTo(_pledge.DasAccountName));
            Assert.That(result.IsNamePublic, Is.EqualTo(_pledge.IsNamePublic));
        }
    }
}
