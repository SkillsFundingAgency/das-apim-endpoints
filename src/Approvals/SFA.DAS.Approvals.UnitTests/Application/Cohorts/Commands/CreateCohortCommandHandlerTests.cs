﻿using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts.Commands
{
    [TestFixture]
    public class CreateCohortCommandHandlerTests
    {
        private CreateCohortCommandHandler _handler;
        private CreateCohortCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Mock<IAutoReservationsService> _autoReservationService;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _request = _fixture.Create<CreateCohortCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _autoReservationService = new Mock<IAutoReservationsService>();

            _handler = new CreateCohortCommandHandler(_commitmentsApiClient.Object, _autoReservationService.Object);
        }

        [Test]
        public async Task Handle_Cohort_Created()
        {
            var expectedResponse = _fixture.Create<CreateCohortResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                    It.Is<PostCreateCohortRequest>(r =>
                            ((CreateCohortRequest)r.Data).AccountId == _request.AccountId &&
                            ((CreateCohortRequest)r.Data).AccountLegalEntityId == _request.AccountLegalEntityId &&
                            ((CreateCohortRequest)r.Data).ActualStartDate == _request.ActualStartDate &&
                            ((CreateCohortRequest)r.Data).Cost == _request.Cost &&
                            ((CreateCohortRequest)r.Data).TrainingPrice == _request.TrainingPrice &&
                            ((CreateCohortRequest)r.Data).EndPointAssessmentPrice == _request.EndPointAssessmentPrice &&
                            ((CreateCohortRequest)r.Data).CourseCode == _request.CourseCode &&
                            ((CreateCohortRequest)r.Data).DateOfBirth == _request.DateOfBirth &&
                            ((CreateCohortRequest)r.Data).DeliveryModel == _request.DeliveryModel &&
                            ((CreateCohortRequest)r.Data).Email == _request.Email &&
                            ((CreateCohortRequest)r.Data).EmploymentEndDate == _request.EmploymentEndDate &&
                            ((CreateCohortRequest)r.Data).EmploymentPrice == _request.EmploymentPrice &&
                            ((CreateCohortRequest)r.Data).EndDate == _request.EndDate &&
                            ((CreateCohortRequest)r.Data).FirstName == _request.FirstName &&
                            ((CreateCohortRequest)r.Data).IgnoreStartDateOverlap == _request.IgnoreStartDateOverlap &&
                            ((CreateCohortRequest)r.Data).IsOnFlexiPaymentPilot == _request.IsOnFlexiPaymentPilot &&
                            ((CreateCohortRequest)r.Data).LastName == _request.LastName &&
                            ((CreateCohortRequest)r.Data).OriginatorReference == _request.OriginatorReference &&
                            ((CreateCohortRequest)r.Data).PledgeApplicationId == _request.PledgeApplicationId &&
                            ((CreateCohortRequest)r.Data).ProviderId == _request.ProviderId &&
                            ((CreateCohortRequest)r.Data).ReservationId == _request.ReservationId &&
                            ((CreateCohortRequest)r.Data).StartDate == _request.StartDate &&
                            ((CreateCohortRequest)r.Data).TransferSenderId == _request.TransferSenderId &&
                            ((CreateCohortRequest)r.Data).Uln == _request.Uln &&
                            ((CreateCohortRequest)r.Data).UserInfo == _request.UserInfo &&
                            ((CreateCohortRequest)r.Data).RequestingParty == _request.RequestingParty &&
                            ((CreateCohortRequest)r.Data).LearnerDataId == _request.LearnerDataId
                        ), true
                )).ReturnsAsync(new ApiResponse<CreateCohortResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task Handle_AutoReservation_Creation_When_No_ReservationID_In_Request()
        {
            var reservationId = Guid.NewGuid();
            _request.ReservationId = null;
            _request.TransferSenderId = null;

            _autoReservationService.Setup(x => x.CreateReservation(It.IsAny<AutoReservation>()))
                .ReturnsAsync(reservationId);

            var expectedResponse = _fixture.Create<CreateCohortResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                    It.Is<PostCreateCohortRequest>(r =>
                            ((CreateCohortRequest)r.Data).ReservationId == _request.ReservationId
                        ), true
                )).ReturnsAsync(new ApiResponse<CreateCohortResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task Delete_Reservation_When_No_ReservationID_In_Request()
        {
            var reservationId = Guid.NewGuid();
            _request.ReservationId = null;
            _request.TransferSenderId = null;

            _autoReservationService.Setup(x => x.CreateReservation(It.IsAny<AutoReservation>()))
                .ReturnsAsync(reservationId);

            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.Is<PostCreateCohortRequest>(r =>
                    ((CreateCohortRequest)r.Data).ReservationId == _request.ReservationId
                ), true
            )).ThrowsAsync(new Exception("Some Error"));

            var act = async () => await _handler.Handle(_request, CancellationToken.None);
            await act.Should().ThrowAsync<Exception>();
            _autoReservationService.Verify(x=>x.DeleteReservation(reservationId));
        }

        [Test]
        public async Task Does_Not_Create_Or_Delete_Reservation_When_ReservationID_In_Request()
        {
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.Is<PostCreateCohortRequest>(r =>
                    ((CreateCohortRequest)r.Data).ReservationId == _request.ReservationId
                ), true
            )).ThrowsAsync(new Exception("Some Error"));

            var act = async () => await _handler.Handle(_request, CancellationToken.None);
            await act.Should().ThrowAsync<Exception>();
            _autoReservationService.Verify(x => x.DeleteReservation(It.IsAny<Guid>()), Times.Never);
            _autoReservationService.Verify(x => x.CreateReservation(It.IsAny<AutoReservation>()), Times.Never);
        }

        [Test]
        public async Task Throw_ApplicationException_When_No_ReservationID_In_Request_But_TransferSenderId_Is_Present()
        {
            _request.ReservationId = null;

            var act = async () => await _handler.Handle(_request, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("When creating a auto reservation, the TransferSenderId must be null");
            _autoReservationService.Verify(x => x.CreateReservation(It.IsAny<AutoReservation>()), Times.Never);
        }

    }
}