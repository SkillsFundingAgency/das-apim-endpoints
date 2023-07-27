using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.DraftApprenticeships;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.DraftApprenticeships
{
    [TestFixture]
    public class WhenUpdateDraftApprenticeship
    {
        private DraftApprenticeshipController _controller;
        private Mock<IMediator> _mediator;
        private UpdateDraftApprenticeshipRequest _request;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<UpdateDraftApprenticeshipRequest>();

            _mediator = new Mock<IMediator>();
            _controller = new DraftApprenticeshipController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task DraftApprenticeshipIsSubmittedCorrectly()
        {
            var cohortId = _fixture.Create<long>();
            var apprenticeshipId = _fixture.Create<long>();

            var result = await _controller.UpdateDraftApprenticeship(cohortId, apprenticeshipId, _request);

            _mediator.Verify(x => x.Send(It.Is<UpdateDraftApprenticeshipCommand>(y =>
                y.CohortId == cohortId &&
                y.ApprenticeshipId == apprenticeshipId &&
                y.ActualStartDate == _request.ActualStartDate &&
                y.StartDate == _request.StartDate &&
                y.Cost == _request.Cost &&
                y.TrainingPrice == _request.TrainingPrice &&
                y.EndPointAssessmentPrice == _request.EndPointAssessmentPrice &&
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
                y.Reference == _request.Reference &&
                y.CourseOption == _request.CourseOption &&
                y.Uln == _request.Uln &&
                y.UserInfo == _request.UserInfo &&
                y.RequestingParty == _request.RequestingParty
            ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}
