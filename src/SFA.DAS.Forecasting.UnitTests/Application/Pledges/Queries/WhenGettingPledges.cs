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

        [SetUp]
        public void Setup()
        {
            _apiResponse = _fixture.Create<GetPledgesResponse>();
            _apiClient = new Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>>();
            _apiClient.Setup(x => x.Get<GetPledgesResponse>(It.IsAny<GetPledgesRequest>())).ReturnsAsync(_apiResponse);

            _handler = new GetPledgesQueryHandler(_apiClient.Object);

            _query = _fixture.Create<GetPledgesQuery>();
        }

        [Test]
        public async Task Then_Pledges_Are_Retrieved()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.AreEqual(_apiResponse.Page, result.Page);
            Assert.AreEqual(_apiResponse.PageSize, result.PageSize);
            Assert.AreEqual(_apiResponse.TotalPages, result.TotalPages);
            Assert.AreEqual(_apiResponse.TotalPledges, result.TotalPledges);


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
