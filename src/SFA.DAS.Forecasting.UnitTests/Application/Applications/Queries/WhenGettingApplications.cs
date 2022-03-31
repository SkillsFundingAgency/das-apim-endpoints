using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
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

        [Test]
        public async Task Then_Applications_Are_Retrieved()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.AreEqual(_apiResponse.Page, result.Page);
            Assert.AreEqual(_apiResponse.PageSize, result.PageSize);
            Assert.AreEqual(_apiResponse.TotalPages, result.TotalPages);
            Assert.AreEqual(_apiResponse.TotalApplications, result.TotalApplications);


            Assert.AreEqual(_apiResponse.Applications.Count(), result.Applications.Count());

            var i = 0;

            foreach (var application in result.Applications)
            {
                var expected = _apiResponse.Applications.ToArray()[i];
                Assert.AreEqual(expected.Id, application.Id);
                Assert.AreEqual(expected.Id, application.Id);
                Assert.AreEqual(expected.EmployerAccountId, application.EmployerAccountId);
                Assert.AreEqual(expected.PledgeId, application.PledgeId);
                Assert.AreEqual(expected.StandardId, application.StandardId);
                Assert.AreEqual(expected.StandardTitle, application.StandardTitle);
                Assert.AreEqual(expected.StandardLevel, application.StandardLevel);
                Assert.AreEqual(expected.StandardDuration, application.StandardDuration);
                Assert.AreEqual(expected.StandardMaxFunding, application.StandardMaxFunding);
                Assert.AreEqual(expected.StartDate, application.StartDate);
                Assert.AreEqual(expected.NumberOfApprentices, application.NumberOfApprentices);
                Assert.AreEqual(expected.NumberOfApprenticesUsed, application.NumberOfApprenticesUsed);
                i++;
            }
        }

        [Test]
        public async Task Then_Paging_Options_Are_Honoured()
        {
            await _handler.Handle(_query, CancellationToken.None);

            _apiClient.Verify(x =>
                x.Get<GetApplicationsResponse>(It.Is<GetApplicationsRequest>(r =>
                    r.GetUrl == $"applications?page={_query.Page}&pageSize={_query.PageSize}")));
        }
    }
}
