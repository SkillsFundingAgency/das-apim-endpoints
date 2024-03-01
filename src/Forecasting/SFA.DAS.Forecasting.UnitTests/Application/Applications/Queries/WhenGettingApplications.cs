using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.UnitTests.Application.Applications.Queries
{
    [TestFixture]
    public class WhenGettingApplications
    {
        private GetApplicationsQueryHandler _handler;
        private Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> _apiClient;
        private GetApplicationsResponse _apiResponse;
        private readonly Fixture _fixture = new Fixture();
        private GetApplicationsQuery _query;

        [SetUp]
        public void Setup()
        {
            _apiResponse = _fixture.Create<GetApplicationsResponse>();
            _apiClient = new Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>>();
            _apiClient.Setup(x => x.Get<GetApplicationsResponse>(It.IsAny<GetApplicationsRequest>())).ReturnsAsync(_apiResponse);

            _handler = new GetApplicationsQueryHandler(_apiClient.Object);

            _query = _fixture.Create<GetApplicationsQuery>();
        }

        [TestCase("Approved")]
        [TestCase("Accepted")]
        public async Task Then_Applications_Are_Retrieved(string status)
        {
            _apiResponse.Applications.ToList().ForEach(a => a.Status = status);

            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_apiResponse.Applications.Count(), Is.EqualTo(result.Applications.Count()));

            var i = 0;

            foreach (var application in result.Applications)
            {
                var expected = _apiResponse.Applications.ToArray()[i];
                Assert.That(expected.Id, Is.EqualTo(application.Id));
                Assert.That(expected.Id, Is.EqualTo(application.Id));
                Assert.That(expected.EmployerAccountId, Is.EqualTo(application.EmployerAccountId));
                Assert.That(expected.PledgeId, Is.EqualTo(application.PledgeId));
                Assert.That(expected.StandardId, Is.EqualTo(application.StandardId));
                Assert.That(expected.StandardTitle, Is.EqualTo(application.StandardTitle));
                Assert.That(expected.StandardLevel, Is.EqualTo(application.StandardLevel));
                Assert.That(expected.StandardDuration, Is.EqualTo(application.StandardDuration));
                Assert.That(expected.StandardMaxFunding, Is.EqualTo(application.StandardMaxFunding));
                Assert.That(expected.StartDate, Is.EqualTo(application.StartDate));
                Assert.That(expected.NumberOfApprentices, Is.EqualTo(application.NumberOfApprentices));
                Assert.That(expected.NumberOfApprenticesUsed, Is.EqualTo(application.NumberOfApprenticesUsed));
                Assert.That(expected.Status, Is.EqualTo(application.Status));
                i++;
            }
        }

        [Test]
        public async Task Then_Applications__Not_Approved_Or_Accepted_Are_Not_Retrieved()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(0, Is.EqualTo(result.Applications.Count()));
            
        }
    }
}
