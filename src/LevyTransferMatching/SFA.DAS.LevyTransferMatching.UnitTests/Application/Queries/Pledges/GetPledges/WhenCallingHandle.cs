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

            result.Should().NotBeNull();
            result.Pledges.Should().NotBeNull();
            result.TotalPledges.Should().Be(_pledgeResponse.TotalPledges);
            result.TotalPages.Should().Be(_pledgeResponse.TotalPages);
            result.Page.Should().Be(_pledgeResponse.Page);
            result.PageSize.Should().Be(_pledgeResponse.PageSize);
            result.Pledges.Should().NotBeEmpty();
            result.Pledges.Any(x => x.Id == 0).Should().BeFalse();
            result.Pledges.Any(x => x.Amount == 0).Should().BeFalse();
            result.Pledges.Any(x => x.RemainingAmount == 0).Should().BeFalse();
            result.Pledges.Any(x => x.ApplicationCount == 0).Should().BeFalse();
            result.Pledges.Any(x => x.Status == string.Empty).Should().BeFalse();
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
