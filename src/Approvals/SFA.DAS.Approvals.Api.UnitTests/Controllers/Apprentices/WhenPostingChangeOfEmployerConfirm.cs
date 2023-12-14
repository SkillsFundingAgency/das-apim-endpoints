using System;
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
using SFA.DAS.Approvals.Application.Apprentices.Commands.ChangeEmployer.Confirm;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenPostingChangeOfEmployerConfirm
    {
        private ApprenticesController _controller;
        private Mock<IMediator> _mediator;
        private readonly Fixture _fixture = new Fixture();
        private ConfirmRequest _request;
        private long _providerId;
        private long _apprenticeshipId;

        [SetUp]
        public void Setup()
        {
            _providerId = _fixture.Create<long>();
            _apprenticeshipId = _fixture.Create<long>();

            _request = _fixture.Create<ConfirmRequest>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(x =>
                    x.Send(It.IsAny<ConfirmCommand>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new Unit());

            _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), _mediator.Object, Mock.Of<IMapper>());
        }

        [Test]
        public async Task Then_ChangeOfEmployerRequest_Is_Created()
        {
            await _controller.ChangeEmployerConfirm(_providerId, _apprenticeshipId, _request);

           _mediator.Verify(x =>
               x.Send(It.Is<ConfirmCommand>(c =>
                   c.AccountLegalEntityId == _request.AccountLegalEntityId &&
                   c.DeliveryModel == _request.DeliveryModel &&
                   c.EmploymentEndDate == _request.EmploymentEndDate &&
                   c.EmploymentPrice == _request.EmploymentPrice &&
                   c.EndDate == _request.EndDate &&
                   c.Price == _request.Price &&
                   c.StartDate == _request.StartDate &&
                   c.UserInfo.UserId == _request.UserInfo.UserId &&
                   c.UserInfo.UserDisplayName == _request.UserInfo.UserDisplayName &&
                   c.UserInfo.UserEmail == _request.UserInfo.UserEmail &&
                   c.ApprenticeshipId == _apprenticeshipId &&
                   c.HasOverlappingTrainingDates == _request.HasOverlappingTrainingDates &&
                   c.ProviderId == _providerId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Then_Ok_Result_Is_Returned()
        {
            var result = await _controller.ChangeEmployerConfirm(_providerId, _apprenticeshipId, _request);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Then_BadRequest_Result_Is_Returned_If_Apprenticeship_Is_Not_Found()
        {
            _mediator.Setup(x => x.Send(It.IsAny<ConfirmCommand>(), It.IsAny<CancellationToken>())).Throws<InvalidOperationException>();
            var result = await _controller.ChangeEmployerConfirm(_providerId, _apprenticeshipId+1, _request);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}
