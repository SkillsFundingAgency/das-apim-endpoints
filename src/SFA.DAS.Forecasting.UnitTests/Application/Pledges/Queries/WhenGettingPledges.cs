using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.UnitTests.Application.Pledges.Queries
{
    [TestFixture]
    public class WhenGettingPledges
    {
        private GetPledgesQueryHandler _handler;
        private Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> _apiClient;
        private GetPledgesResponse _apiResponse;
        private readonly Fixture _fixture = new Fixture();
        private GetPledgesQuery _query;
        private long _accountId;

        [SetUp]
        public void Setup()
        {
            _accountId = _fixture.Create<long>();

            _apiResponse = _fixture.Create<GetPledgesResponse>();
            _apiClient = new Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>>();
            _apiClient.Setup(x => x.Get<GetPledgesResponse>(It.Is<GetPledgesRequest>(r => r.AccountId == _accountId))).ReturnsAsync(_apiResponse);

            _handler = new GetPledgesQueryHandler(_apiClient.Object);

            _query = new GetPledgesQuery { AccountId = _accountId };
        }

        [Test]
        public async Task Then_Pledges_Are_Retrieved()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.AreEqual(_apiResponse.Pledges.Count(), result.Pledges.Count());

            var i = 0;

            foreach (var pledge in result.Pledges)
            {
                var expected = _apiResponse.Pledges.ToArray()[i];
                Assert.AreEqual(expected.Id, pledge.Id);
                Assert.AreEqual(expected.AccountId, pledge.AccountId);
                i++;
            }
        }
    }
}
