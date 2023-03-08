using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddDraftApprenticeship;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships
{
    [TestFixture]
    public class WhenAddDraftApprenticeship
    {
        private DraftApprenticeshipController _controller;
        private Mock<IMediator> _mediator;
        private AddDraftApprenticeshipRequest _request;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<AddDraftApprenticeshipRequest>();

            _mediator = new Mock<IMediator>();
            _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task DraftApprenticeshipIsSubmittedCorrectly()
        {
            var cohortId = _fixture.Create<long>();
            var expectedResult = _fixture.Create<AddDraftApprenticeshipResult>();

            _mediator.Setup(x => x.Send(It.Is<AddDraftApprenticeshipCommand>(y =>
                        y.CohortId == cohortId &&
                        y.ActualStartDate == _request.ActualStartDate &&
                        y.StartDate == _request.StartDate &&
                        y.Cost == _request.Cost &&
                        y.CourseCode == _request.CourseCode &&
                        y.DateOfBirth == _request.DateOfBirth &&
                        y.DeliveryModel == _request.DeliveryModel &&
                        y.Email == _request.Email &&
                        y.ReservationId == _request.ReservationId &&
                        y.EmploymentEndDate == _request.EmploymentEndDate &&
                        y.EmploymentPrice == _request.EmploymentPrice &&
                        y.EndDate == _request.EndDate &&
                        y.FirstName == _request.FirstName &&
                        y.IgnoreStartDateOverlap == _request.IgnoreStartDateOverlap &&
                        y.LastName == _request.LastName &&
                        y.IsOnFlexiPaymentPilot == _request.IsOnFlexiPaymentPilot &&
                        y.OriginatorReference == _request.OriginatorReference &&
                        y.ProviderId == _request.ProviderId &&
                        y.Uln == _request.Uln &&
                        y.UserInfo == _request.UserInfo &&
                        y.UserId == _request.UserId
                        ), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var result = await _controller.AddDraftApprenticeship(cohortId, _request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
