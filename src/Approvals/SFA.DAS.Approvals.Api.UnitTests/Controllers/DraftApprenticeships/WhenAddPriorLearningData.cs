using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships
{
    [TestFixture]
    public class WhenAddPriorLearningData
    {
        private DraftApprenticeshipController _controller;
        private Mock<IMediator> _mediator;
        private AddPriorLearningDataRequest _request;
        private AddPriorLearningDataCommandResult _result;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<AddPriorLearningDataRequest>();
            _result = _fixture.Create<AddPriorLearningDataCommandResult>();
            _mediator = new Mock<IMediator>();
            _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task PriorLearningDataIsSubmittedCorrectly()
        {
            var cohortId = _fixture.Create<long>();
            var draftApprenticeshipId = _fixture.Create<long>();

            _mediator.Setup(x => x.Send(It.Is<AddPriorLearningDataCommand>(y =>
                y.CohortId == cohortId &&
                y.DraftApprenticeshipId == draftApprenticeshipId &&
                y.DurationReducedBy == _request.DurationReducedBy &&
                y.DurationReducedByHours == _request.DurationReducedByHours &&
                y.IsDurationReducedByRpl == _request.IsDurationReducedByRpl &&
                y.PriceReducedBy == _request.PriceReducedBy &&
                y.TrainingTotalHours == _request.TrainingTotalHours
            ), It.IsAny<CancellationToken>())).ReturnsAsync(_result);

            var result = await _controller.AddPriorLearningData(cohortId, draftApprenticeshipId, _request);

            Assert.That(result, Is.lnstanceOf<OkOBjectResult>());
            var okObjectResult = (OkObjectResult)result;
            Assert.IsInstanceOf<AddPriorLearningDataCommandResult>(okObjectResult.Value);
            var objectResult = (AddPriorLearningDataCommandResult)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_result, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
