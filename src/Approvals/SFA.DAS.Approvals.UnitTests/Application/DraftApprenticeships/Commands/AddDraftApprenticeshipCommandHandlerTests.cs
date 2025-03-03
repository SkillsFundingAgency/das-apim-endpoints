using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands
{
    [TestFixture]
    public class AddDraftApprenticeshipCommandHandlerTests
    {
        private AddDraftApprenticeshipCommandHandler _handler;
        private AddDraftApprenticeshipCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Mock<IAutoReservationsService> _autoReservationService;
        private GetCohortResponse _cohort;

        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<AddDraftApprenticeshipCommand>();
            _cohort = _fixture.Build<GetCohortResponse>().Without(p=>p.TransferSenderId).Create();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _autoReservationService = new Mock<IAutoReservationsService>();
            _commitmentsApiClient
                .Setup(x => x.Get<GetCohortResponse>(It.Is<GetCohortRequest>(p => p.CohortId == _request.CohortId)))
                .ReturnsAsync(_cohort);

            _handler = new AddDraftApprenticeshipCommandHandler(_commitmentsApiClient.Object, _autoReservationService.Object);
        }

        [Test]
        public async Task Handle_Cohort_Created()
        {
            var expectedResponse = _fixture.Create<AddDraftApprenticeshipResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                    It.Is<PostAddDraftApprenticeshipRequest>(r =>
                            ((AddDraftApprenticeshipRequest)r.Data).ActualStartDate == _request.ActualStartDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).Cost == _request.Cost &&
                            ((AddDraftApprenticeshipRequest)r.Data).TrainingPrice == _request.TrainingPrice &&
                            ((AddDraftApprenticeshipRequest)r.Data).EndPointAssessmentPrice == _request.EndPointAssessmentPrice &&
                            ((AddDraftApprenticeshipRequest)r.Data).CourseCode == _request.CourseCode &&
                            ((AddDraftApprenticeshipRequest)r.Data).DateOfBirth == _request.DateOfBirth &&
                            ((AddDraftApprenticeshipRequest)r.Data).DeliveryModel == _request.DeliveryModel &&
                            ((AddDraftApprenticeshipRequest)r.Data).Email == _request.Email &&
                            ((AddDraftApprenticeshipRequest)r.Data).EmploymentEndDate == _request.EmploymentEndDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).EmploymentPrice == _request.EmploymentPrice &&
                            ((AddDraftApprenticeshipRequest)r.Data).EndDate == _request.EndDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).FirstName == _request.FirstName &&
                            ((AddDraftApprenticeshipRequest)r.Data).IgnoreStartDateOverlap == _request.IgnoreStartDateOverlap &&
                            ((AddDraftApprenticeshipRequest)r.Data).IsOnFlexiPaymentPilot == _request.IsOnFlexiPaymentPilot &&
                            ((AddDraftApprenticeshipRequest)r.Data).LastName == _request.LastName &&
                            ((AddDraftApprenticeshipRequest)r.Data).OriginatorReference == _request.OriginatorReference &&
                            ((AddDraftApprenticeshipRequest)r.Data).ProviderId == _request.ProviderId &&
                            ((AddDraftApprenticeshipRequest)r.Data).ReservationId == _request.ReservationId &&
                            ((AddDraftApprenticeshipRequest)r.Data).StartDate == _request.StartDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).Uln == _request.Uln &&
                            ((AddDraftApprenticeshipRequest)r.Data).UserInfo == _request.UserInfo &&
                            ((AddDraftApprenticeshipRequest)r.Data).UserId == _request.UserId &&
                            ((AddDraftApprenticeshipRequest)r.Data).RequestingParty == _request.RequestingParty
                        ), true
                )).ReturnsAsync(new ApiResponse<AddDraftApprenticeshipResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task Handle_AutoReservation_Creation_When_No_ReservationID_In_Request()
        {
            var reservationId = Guid.NewGuid();
            _request.ReservationId = null;

            _autoReservationService.Setup(x => x.CreateReservation(It.IsAny<AutoReservation>()))
                .ReturnsAsync(reservationId);

            var expectedResponse = _fixture.Create<AddDraftApprenticeshipResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                It.Is<PostAddDraftApprenticeshipRequest>(r =>
                    ((AddDraftApprenticeshipRequest)r.Data).ReservationId == _request.ReservationId
                ), true
            )).ReturnsAsync(new ApiResponse<AddDraftApprenticeshipResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task Throw_ApplicationException_When_No_ReservationID_In_Request_But_TransferSenderId_Is_Present()
        {
            _request.ReservationId = null;
            _cohort.TransferSenderId = 109109;

            var act = async () => await _handler.Handle(_request, CancellationToken.None);
            await act.Should().ThrowAsync<ApplicationException>();
            _autoReservationService.Verify(x => x.CreateReservation(It.IsAny<AutoReservation>()), Times.Never);
        }

        [Test]
        public async Task Delete_Reservation_When_No_ReservationID_In_Request()
        {
            var reservationId = Guid.NewGuid();
            _request.ReservationId = null;

            _autoReservationService.Setup(x => x.CreateReservation(It.IsAny<AutoReservation>()))
                .ReturnsAsync(reservationId);

            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                It.Is<PostAddDraftApprenticeshipRequest>(r =>
                    ((AddDraftApprenticeshipRequest)r.Data).ReservationId == _request.ReservationId
                ), true
            )).ThrowsAsync(new Exception("Some error"));

            var act = async () => await _handler.Handle(_request, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Some error");

            _autoReservationService.Verify(x=>x.DeleteReservation(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task Does_Not_Create_Or_Delete_Reservation_When_ReservationID_In_Request()
        {
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                It.Is<PostAddDraftApprenticeshipRequest>(r =>
                    ((AddDraftApprenticeshipRequest)r.Data).ReservationId == _request.ReservationId
                ), true
            )).ThrowsAsync(new Exception("Some error"));

            var act = async () => await _handler.Handle(_request, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Some error");

            _autoReservationService.Verify(x => x.DeleteReservation(It.IsAny<Guid>()), Times.Never);
            _autoReservationService.Verify(x => x.CreateReservation(It.IsAny<AutoReservation>()), Times.Never);
        }
    }
}