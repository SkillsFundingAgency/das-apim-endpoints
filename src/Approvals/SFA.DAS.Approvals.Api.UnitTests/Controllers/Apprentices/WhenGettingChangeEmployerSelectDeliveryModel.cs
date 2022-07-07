using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.Apprentices.ChangeEmployer;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.SelectDeliveryModel;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenGettingChangeEmployerSelectDeliveryModel
    {
        private ApprenticesController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetSelectDeliveryModelResult _queryResult;
        private long _providerId;
        private long _apprenticeshipId;

        [SetUp]
        public void Setup()
        {
            _providerId = _fixture.Create<long>();
            _apprenticeshipId = _fixture.Create<long>();

            _queryResult = _fixture.Create<GetSelectDeliveryModelResult>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetSelectDeliveryModelQuery>(q => q.ProviderId == _providerId && q.ApprenticeshipId == _apprenticeshipId), It.IsAny<CancellationToken>())).ReturnsAsync(_queryResult);

            _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object);
        }

        [Test]
        public async Task Then_Response_Is_Returned()
        {
            var result = await _controller.ChangeEmployerSelectDeliveryModel(_providerId, _apprenticeshipId);

            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as GetSelectDeliveryModelResponse;

            Assert.IsNotNull(response);

            Assert.AreEqual(_queryResult.DeliveryModels, response.DeliveryModels);
            Assert.AreEqual(_queryResult.LegalEntityName, response.LegalEntityName);
            Assert.AreEqual(_queryResult.CurrentDeliveryModel, response.CurrentDeliveryModel);
        }

        [Test]
        public async Task Then_NotFoundResponse_Is_Returned_If_Apprenticeship_Is_Not_Found()
        {
            _mediator.Setup(x => x.Send(It.Is<GetSelectDeliveryModelQuery>(q => q.ProviderId == _providerId && q.ApprenticeshipId == _apprenticeshipId), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            var result = await _controller.ChangeEmployerSelectDeliveryModel(_providerId, _apprenticeshipId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
