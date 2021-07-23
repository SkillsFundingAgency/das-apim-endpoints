using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetMyPledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetMyPledges
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private GetMyPledgesQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private GetMyPledgesQuery _query;
        private IEnumerable<Pledge> _pledges;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _pledges = _fixture.Create<IEnumerable<Pledge>>();

            var accountId = _fixture.Create<int>();
            _query = new GetMyPledgesQuery(accountId);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetPledges()).ReturnsAsync(_pledges);

            _handler = new GetMyPledgesQueryHandler(_levyTransferMatchingService.Object);
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
        }
    }
}
