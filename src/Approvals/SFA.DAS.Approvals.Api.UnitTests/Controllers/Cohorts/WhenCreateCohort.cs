using System.Threading;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Api.Models.Cohorts;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Cohorts
{
    [TestFixture]
    public class WhenCreateCohort
    {
        private CohortController _controller;
        private Mock<IMediator> _mediator;
        private CreateCohortRequest _request;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<CreateCohortRequest>();

            _mediator = new Mock<IMediator>();
            _controller = new CohortController(Mock.Of<ILogger<DraftApprenticeshipController>>(), _mediator.Object);
        }

        [Test]
        public async Task CohortIsSubmittedCorrectly()
        {
            var expectedResult = _fixture.Create<CreateCohortResult>();

            _mediator.Setup(x => x.Send(It.Is<CreateCohortCommand>(y =>
                        y.ActualStartDate == _request.ActualStartDate &&
                        y.StartDate == _request.StartDate &&
                        y.AccountId == _request.AccountId &&
                        y.AccountLegalEntityId == _request.AccountLegalEntityId &&
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
                        y.PledgeApplicationId == _request.PledgeApplicationId &&
                        y.ProviderId == _request.ProviderId &&
                        y.TransferSenderId == _request.TransferSenderId &&
                        y.Uln == _request.Uln &&
                        y.UserInfo == _request.UserInfo
                        ), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var result = await _controller.Create(_request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
