using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipDetails;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships
{
    [TestFixture]
    public class WhenGettingAddDraftApprenticeshipDetails
    {
        private DraftApprenticeshipController _controller;
        private Mock<IMediator> _mediator;
        private GetAddDraftApprenticeshipDetailsQueryResult _queryResult;

        private long _cohortId;
        private string _courseCode;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetAddDraftApprenticeshipDetailsQueryResult>();

            _cohortId = fixture.Create<long>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetAddDraftApprenticeshipDetailsQuery>(q =>
                        q.CohortId == _cohortId
                        && q.CourseCode == _courseCode),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task AddDraftApprenticeshipDetailsResponseIsReturned()
        {
            var result = await _controller.GetAddDraftApprenticeshipDetails(_cohortId, _courseCode);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult) result;
            Assert.IsInstanceOf<GetAddDraftApprenticeshipDetailsResponse>(okObjectResult.Value);
            var objectResult = (GetAddDraftApprenticeshipDetailsResponse) okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
