﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task Returns_MyPledges()
        {
            var result = await _handler.Handle(_query, new CancellationToken());

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Pledges);
            CollectionAssert.IsNotEmpty(result.Pledges);
            Assert.That(!result.Pledges.Any(x => x.Id == 0));
            Assert.That(!result.Pledges.Any(x => x.Amount == 0));
            Assert.That(!result.Pledges.Any(x => x.RemainingAmount == 0));
            Assert.That(!result.Pledges.Any(x => x.ApplicationCount == 0));
        }
    }
}
