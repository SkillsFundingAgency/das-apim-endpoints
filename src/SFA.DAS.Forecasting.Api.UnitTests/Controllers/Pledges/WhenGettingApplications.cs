using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Api.Controllers;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers.Applications
{
    [TestFixture]
    public class WhenGettingApplications
    {
        private ApplicationsController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetApplicationsQueryResult _queryResult;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _queryResult = _fixture.Create<GetApplicationsQueryResult>();
            _mediator.Setup(x => x.Send(It.IsAny<GetApplicationsQuery>(), CancellationToken.None))
                .ReturnsAsync(_queryResult);

            _controller = new ApplicationsController(_mediator.Object, Mock.Of<ILogger<ApplicationsController>>());
        }

        [Test]
        public async Task Then_Applications_Are_Returned_Correctly()
        {
            var result = await _controller.GetApplications(0, 100) as ObjectResult;

            Assert.IsNotNull(result);
            var response = result.Value as GetApplicationsResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(_queryResult.Page, response.Page);
            Assert.AreEqual(_queryResult.PageSize, response.PageSize);
            Assert.AreEqual(_queryResult.TotalPages, response.TotalPages);
            Assert.AreEqual(_queryResult.TotalApplications, response.TotalApplications);

            Assert.AreEqual(_queryResult.Applications.Count(), response.Applications.Count());

            var i = 0;

            foreach (var application in response.Applications)
            {
                var expected = _queryResult.Applications.ToArray()[i];
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
            var page = _fixture.Create<int>();
            var pageSize = _fixture.Create<int>();

            await _controller.GetApplications(page, pageSize);

            _mediator.Verify(x =>
                x.Send(It.Is<GetApplicationsQuery>(q => q.Page == page && q.PageSize == pageSize),
                    It.IsAny<CancellationToken>()));
        }
    }
}
