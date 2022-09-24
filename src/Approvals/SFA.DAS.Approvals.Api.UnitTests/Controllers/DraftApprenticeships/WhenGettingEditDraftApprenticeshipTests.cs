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
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships
{
    [TestFixture]
    public class WhenGettingEditDraftApprenticeshipTests
    {
        private DraftApprenticeshipController _controller;
        private Mock<IMediator> _mediator;
        private GetEditDraftApprenticeshipQueryResult _queryResult;

        private long _cohortId;
        private long _draftApprenticeshipId;
        private string _courseCode;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetEditDraftApprenticeshipQueryResult>();

            _cohortId = fixture.Create<long>();
            _draftApprenticeshipId = fixture.Create<long>();
            _courseCode = fixture.Create<string>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetEditDraftApprenticeshipQuery>(q =>
                        q.CohortId == _cohortId &&
                        q.CourseCode == _courseCode &&
                        q.DraftApprenticeshipId == _draftApprenticeshipId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task EditDraftApprenticeshipResponseIsReturned()
        {
            var result = await _controller.GetEditDraftApprenticeship(_cohortId, _draftApprenticeshipId, _courseCode);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult) result;
            Assert.IsInstanceOf<GetEditDraftApprenticeshipResponse>(okObjectResult.Value);
            var objectResult = (GetEditDraftApprenticeshipResponse) okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
