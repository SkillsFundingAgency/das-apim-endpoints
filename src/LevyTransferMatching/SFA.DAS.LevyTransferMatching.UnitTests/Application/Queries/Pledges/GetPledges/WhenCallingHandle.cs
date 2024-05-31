using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetPledges
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private GetPledgesQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private GetPledgesQuery _query;
        private GetPledgesResponse _pledgeResponse;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _pledgeResponse = _fixture.Create<GetPledgesResponse>();

            var accountId = _fixture.Create<int>();
            _query = new GetPledgesQuery(accountId);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>())).ReturnsAsync(_pledgeResponse);

            _handler = new GetPledgesQueryHandler(_levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Returns_Pledges()
        {
            var result = await _handler.Handle(_query, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Pledges,Is.Not.Null);
            Assert.That(result.TotalPledges, Is.EqualTo(_pledgeResponse.TotalPledges));
            Assert.That(result.TotalPages, Is.EqualTo(_pledgeResponse.TotalPages));
            Assert.That(result.Page, Is.EqualTo(_pledgeResponse.Page));
            Assert.That(result.PageSize, Is.EqualTo(_pledgeResponse.PageSize));
            result.Pledges.Should().NotBeEmpty();
            Assert.That(!result.Pledges.Any(x => x.Id == 0));
            Assert.That(!result.Pledges.Any(x => x.Amount == 0));
            Assert.That(!result.Pledges.Any(x => x.RemainingAmount == 0));
            Assert.That(!result.Pledges.Any(x => x.ApplicationCount == 0));
            Assert.That(!result.Pledges.Any(x => x.Status == string.Empty));
        }

        [Test]
        public async Task Verify_Query_Is_Mapped_With_Default_Values()
        {
            await _handler.Handle(_query, new CancellationToken());

            _levyTransferMatchingService.Verify(x =>
                x.GetPledges(It.Is<GetPledgesRequest>(p =>
                    p.AccountId == _query.AccountId && p.GetUrl.Contains($"page={_query.Page}") &&
                    !p.GetUrl.Contains("pageSize="))));
        }

        [Test]
        public async Task Verify_Query_Is_Mapped_With_Explicit_Values()
        {
            _query.Page = 2;
            _query.PageSize = 10;
            await _handler.Handle(_query, new CancellationToken());

            _levyTransferMatchingService.Verify(x =>
                x.GetPledges(It.Is<GetPledgesRequest>(p =>
                    p.AccountId == _query.AccountId && p.GetUrl.Contains($"page={_query.Page}") &&
                    p.GetUrl.Contains($"pageSize={_query.PageSize}"))));
        }
    }
}
