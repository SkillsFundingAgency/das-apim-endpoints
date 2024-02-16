using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenGettingEditApprenticeship
    {
        private ApprenticesController _controller;
        private Mock<IMediator> _mediator;
        private GetEditApprenticeshipQueryResult _queryResult;

        private long _apprenticeshipId;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetEditApprenticeshipQueryResult>();

            _apprenticeshipId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetEditApprenticeshipQuery>(q =>
                        q.ApprenticeshipId == _apprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object, Mock.Of<IMapper>());
        }

        [Test]
        public async Task GetEditApprenticeshipResponseIsReturned()
        {
            var result = await _controller.EditApprenticeship( _apprenticeshipId);

            Assert.That(result, Is.lnstanceOf<OkOBjectResult>());
            var okObjectResult = (OkObjectResult)result;
            Assert.IsInstanceOf<GetEditApprenticeshipResponse>(okObjectResult.Value);
            var objectResult = (GetEditApprenticeshipResponse)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
