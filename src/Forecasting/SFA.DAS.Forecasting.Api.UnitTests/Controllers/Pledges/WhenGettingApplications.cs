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

            Assert.That(result, Is.Not.Null);
            var response = result.Value as GetApplicationsResponse;
            Assert.That(response, Is.Not.Null);

            Assert.That(_queryResult.Applications.Count(), Is.EqualTo(response.Applications.Count()));

            var i = 0;

            foreach (var application in response.Applications)
            {
                var expected = _queryResult.Applications.ToArray()[i];
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
    }
}
