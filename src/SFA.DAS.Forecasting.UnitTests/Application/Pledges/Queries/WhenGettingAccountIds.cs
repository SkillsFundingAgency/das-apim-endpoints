using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetAccountsWithPledges;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.UnitTests.Application.Pledges.Queries
{
    [TestFixture]
    public class WhenGettingAccountsWithPledges
    {
        private GetAccountsWithPledgesQueryHandler _handler;
        private Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> _apiClient;
        private GetPledgesResponse _apiResponse;
        private readonly Fixture _fixture = new Fixture();
        private GetAccountsWithPledgesQuery _query;

        [SetUp]
        public void Setup()
        {
            _apiResponse = _fixture.Create<GetPledgesResponse>();
            _apiClient = new Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>>();
            _apiClient.Setup(x => x.Get<GetPledgesResponse>(It.IsAny<GetPledgesRequest>())).ReturnsAsync(_apiResponse);

            _handler = new GetAccountsWithPledgesQueryHandler(_apiClient.Object);

            _query = _fixture.Create<GetAccountsWithPledgesQuery>();
        }

        [Test]
        public async Task Then_AccountIds_Are_Retrieved()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            CollectionAssert.AreEquivalent(_apiResponse.Pledges.Select(x => x.AccountId).Distinct(), result.AccountIds);
        }
    }
}