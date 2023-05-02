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
using SFA.DAS.Approvals.Api.Models.Cohorts;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDeliveryModel;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts
{
    [TestFixture]
    public class WhenGettingAddDraftApprenticeshipDeliveryModel
    {
        private CohortController _controller;
        private Mock<IMediator> _mediator;
        private GetAddDraftApprenticeshipDeliveryModelQueryResult _queryResult;

        private long _accountLegalEntityId;
        private long _providerId;
        private string _courseCode;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _queryResult = fixture.Create<GetAddDraftApprenticeshipDeliveryModelQueryResult>();

            _accountLegalEntityId = fixture.Create<long>();
            _providerId = fixture.Create<long>();
            _courseCode = fixture.Create<string>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetAddDraftApprenticeshipDeliveryModelQuery>(q =>
                        q.ProviderId == _providerId
                        && q.AccountLegalEntityId == _accountLegalEntityId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_queryResult);

            _controller = new CohortController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task GetAddDraftApprenticeshipDeliveryModelResponseIsReturned()
        {
            var result = await _controller.GetAddDraftApprenticeshipDeliveryModel(_accountLegalEntityId, _providerId, _courseCode);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.IsInstanceOf<GetAddDraftApprenticeshipDeliveryModelResponse>(okObjectResult.Value);
            var objectResult = (GetAddDraftApprenticeshipDeliveryModelResponse)okObjectResult.Value;

            var compare = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });

            var comparisonResult = compare.Compare(_queryResult, objectResult);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
