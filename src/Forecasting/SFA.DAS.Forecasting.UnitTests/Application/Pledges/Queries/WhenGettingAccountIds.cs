using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
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
            var expected = _apiResponse.Pledges.Select(x => x.AccountId).Distinct();
            expected.Should().BeEquivalentTo(result.AccountIds);
        }

        [Test]
        public async Task Then_AccountIds_With_No_Pledges_With_Applications_Are_Not_Retrieved()
        {
            _apiResponse.Pledges.ToList().ForEach(x => x.ApplicationCount = 0);
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(0, Is.EqualTo(result.AccountIds.Count));
        }
    }
}