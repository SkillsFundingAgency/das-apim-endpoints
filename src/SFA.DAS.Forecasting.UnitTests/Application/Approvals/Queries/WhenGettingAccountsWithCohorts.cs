using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Approvals.Queries;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.UnitTests.Application.Approvals.Queries
{
    [TestFixture]
    public class WhenGettingAccountsWithCohorts
    {
        private GetAccountsWithCohortsQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private GetAccountsWithCohortsResponse _apiResponse;
        private readonly Fixture _fixture = new Fixture();
        private GetAccountsWithCohortsQuery _query;

        [SetUp]
        public void Setup()
        {
            _apiResponse = _fixture.Create<GetAccountsWithCohortsResponse>();
            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _apiClient.Setup(x => x.Get<GetAccountsWithCohortsResponse>(It.IsAny<GetAccountsWithCohortsRequest>())).ReturnsAsync(_apiResponse);

            _handler = new GetAccountsWithCohortsQueryHandler(_apiClient.Object);

            _query = _fixture.Create<GetAccountsWithCohortsQuery>();
        }

        [Test]
        public async Task Then_AccountsWithCohorts_Are_Retrieved()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            CollectionAssert.AreEqual(_apiResponse.AccountIds, result.AccountIds);
        }
    }
}
