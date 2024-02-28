using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.Apprentices.ChangeEmployer;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ConfirmEmployer;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenGettingChangeEmployerConfirmEmployer
    {
        private ApprenticesController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private GetConfirmEmployerQueryResult _queryResult;
        private long _providerId;
        private long _apprenticeshipId;
        private long _accountLegalEntityId;

        [SetUp]
        public void Setup()
        {
            _providerId = _fixture.Create<long>();
            _apprenticeshipId = _fixture.Create<long>();
            _accountLegalEntityId = _fixture.Create<long>();

            _queryResult = _fixture.Create<GetConfirmEmployerQueryResult>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.Is<GetConfirmEmployerQuery>(q => q.ProviderId == _providerId && q.ApprenticeshipId == _apprenticeshipId && q.AccountLegalEntityId == _accountLegalEntityId), It.IsAny<CancellationToken>())).ReturnsAsync(_queryResult);

            _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object, Mock.Of<IMapper>());
        }

        [Test]
        public async Task Then_Response_Is_Returned()
        {
            var result = await _controller.ChangeEmployerConfirmEmployer(_providerId, _apprenticeshipId, _accountLegalEntityId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as GetConfirmEmployerResponse;

            Assert.That(response, Is.Not.Null);

            Assert.That(_queryResult.LegalEntityName, Is.EqualTo(response.LegalEntityName));
            Assert.That(_queryResult.AccountName,  Is.EqualTo(response.AccountName));
            Assert.That(_queryResult.AccountLegalEntityName, Is.EqualTo(response.AccountLegalEntityName));
            Assert.That(_queryResult.IsFlexiJobAgency, Is.EqualTo(response.IsFlexiJobAgency));
            Assert.That(_queryResult.DeliveryModel, Is.EqualTo(response.DeliveryModel));
        }

        [Test]
        public async Task Then_NotFoundResponse_Is_Returned_If_Apprenticeship_Is_Not_Found()
        {
            _mediator.Setup(x => x.Send(It.Is<GetConfirmEmployerQuery>(q => q.ProviderId == _providerId && q.ApprenticeshipId == _apprenticeshipId && q.AccountLegalEntityId == _accountLegalEntityId), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            var result = await _controller.ChangeEmployerConfirmEmployer(_providerId, _apprenticeshipId, _accountLegalEntityId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
