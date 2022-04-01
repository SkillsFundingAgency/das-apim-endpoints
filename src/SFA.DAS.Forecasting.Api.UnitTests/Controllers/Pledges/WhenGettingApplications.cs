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
        private int _pledgeId;

        [SetUp]
        public void Setup()
        {
            _pledgeId = _fixture.Create<int>();

            _mediator = new Mock<IMediator>();

            _queryResult = _fixture.Create<GetApplicationsQueryResult>();
            _mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(q => q.PledgeId == _pledgeId), CancellationToken.None))
                .ReturnsAsync(_queryResult);

            _controller = new ApplicationsController(_mediator.Object);
        }

        [Test]
        public async Task Then_Applications_Are_Returned_Correctly()
        {
            var result = await _controller.GetApplications(_pledgeId) as ObjectResult;

            Assert.IsNotNull(result);
            var response = result.Value as GetApplicationsResponse;
            Assert.IsNotNull(response);

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
                Assert.AreEqual(expected.Status, application.Status);
                i++;
            }
        }
    }
}
