using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
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
        [Test, MoqAutoData]
        public async Task Handle_Cohort_Created(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
            GetStandardsListItem standardResponse,
            GetLearnerAgeResponse learnerAgeResponse,
            AddDraftApprenticeshipCommand request,
            AddDraftApprenticeshipResponse expectedResponse,
            GetCohortResponse cohort,
            AddDraftApprenticeshipCommandHandler handler)
        {
            // Arrange
            standardResponse.ApprenticeshipType = "FoundationApprenticeship";
            commitmentsApiClient
                .Setup(x => x.Get<GetCohortResponse>(It.Is<GetCohortRequest>(p => p.CohortId == request.CohortId)))
                .ReturnsAsync(cohort);

            courseTypeRulesService
                .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
                .ReturnsAsync(new CourseTypeRulesResult
                {
                    Standard = standardResponse,
                    LearnerAgeRules = learnerAgeResponse
                });

            commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                    It.Is<PostAddDraftApprenticeshipRequest>(r =>
                            ((AddDraftApprenticeshipRequest)r.Data).ActualStartDate == request.ActualStartDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).Cost == request.Cost &&
                            ((AddDraftApprenticeshipRequest)r.Data).TrainingPrice == request.TrainingPrice &&
                            ((AddDraftApprenticeshipRequest)r.Data).EndPointAssessmentPrice == request.EndPointAssessmentPrice &&
                            ((AddDraftApprenticeshipRequest)r.Data).CourseCode == request.CourseCode &&
                            ((AddDraftApprenticeshipRequest)r.Data).DateOfBirth == request.DateOfBirth &&
                            ((AddDraftApprenticeshipRequest)r.Data).DeliveryModel == request.DeliveryModel &&
                            ((AddDraftApprenticeshipRequest)r.Data).Email == request.Email &&
                            ((AddDraftApprenticeshipRequest)r.Data).EmploymentEndDate == request.EmploymentEndDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).EmploymentPrice == request.EmploymentPrice &&
                            ((AddDraftApprenticeshipRequest)r.Data).EndDate == request.EndDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).FirstName == request.FirstName &&
                            ((AddDraftApprenticeshipRequest)r.Data).IgnoreStartDateOverlap == request.IgnoreStartDateOverlap &&
                            ((AddDraftApprenticeshipRequest)r.Data).IsOnFlexiPaymentPilot == request.IsOnFlexiPaymentPilot &&
                            ((AddDraftApprenticeshipRequest)r.Data).LastName == request.LastName &&
                            ((AddDraftApprenticeshipRequest)r.Data).OriginatorReference == request.OriginatorReference &&
                            ((AddDraftApprenticeshipRequest)r.Data).ProviderId == request.ProviderId &&
                            ((AddDraftApprenticeshipRequest)r.Data).ReservationId == request.ReservationId &&
                            ((AddDraftApprenticeshipRequest)r.Data).StartDate == request.StartDate &&
                            ((AddDraftApprenticeshipRequest)r.Data).Uln == request.Uln &&
                            ((AddDraftApprenticeshipRequest)r.Data).UserInfo == request.UserInfo &&
                            ((AddDraftApprenticeshipRequest)r.Data).UserId == request.UserId &&
                            ((AddDraftApprenticeshipRequest)r.Data).RequestingParty == request.RequestingParty &&
                            ((AddDraftApprenticeshipRequest)r.Data).LearnerDataId == request.LearnerDataId &&
                            ((AddDraftApprenticeshipRequest)r.Data).MinimumAgeAtApprenticeshipStart == learnerAgeResponse.MinimumAge &&
                            ((AddDraftApprenticeshipRequest)r.Data).MaximumAgeAtApprenticeshipStart == learnerAgeResponse.MaximumAge
                        ), true
                )).ReturnsAsync(new ApiResponse<AddDraftApprenticeshipResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(expectedResponse);
            courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Handle_AutoReservation_Creation_When_No_ReservationID_In_Request(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IAutoReservationsService> autoReservationService,
            [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
            GetStandardsListItem standardResponse,
            GetLearnerAgeResponse learnerAgeResponse,
            AddDraftApprenticeshipCommand request,
            GetCohortResponse cohort,
            AddDraftApprenticeshipCommandHandler handler)
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            request.ReservationId = null;
            cohort.TransferSenderId = null;
            standardResponse.ApprenticeshipType = "FoundationApprenticeship";

            commitmentsApiClient
                .Setup(x => x.Get<GetCohortResponse>(It.Is<GetCohortRequest>(p => p.CohortId == request.CohortId)))
                .ReturnsAsync(cohort)
                .Verifiable();

            courseTypeRulesService
                .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
                .ReturnsAsync(new CourseTypeRulesResult
                {
                    Standard = standardResponse,
                    LearnerAgeRules = learnerAgeResponse
                });

            autoReservationService.Setup(x => x.CreateReservation(It.IsAny<AutoReservation>()))
                .ReturnsAsync(reservationId);

            commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                It.Is<PostAddDraftApprenticeshipRequest>(r =>
                    ((AddDraftApprenticeshipRequest)r.Data).ReservationId == reservationId
                ), true
            )).ReturnsAsync(new ApiResponse<AddDraftApprenticeshipResponse>(new AddDraftApprenticeshipResponse(), HttpStatusCode.OK, string.Empty));

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.DraftApprenticeshipId.Should().Be(0);
            autoReservationService.Verify(x => x.CreateReservation(It.IsAny<AutoReservation>()), Times.Once);
            commitmentsApiClient.Verify();
            courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Throw_ApplicationException_When_No_ReservationID_In_Request_But_TransferSenderId_Is_Present(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IAutoReservationsService> autoReservationService,
            [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
            GetStandardsListItem standardResponse,
            GetLearnerAgeResponse learnerAgeResponse,
            AddDraftApprenticeshipCommand request,
            GetCohortResponse cohort,
            AddDraftApprenticeshipCommandHandler handler)
        {
            // Arrange
            request.ReservationId = null;
            cohort.TransferSenderId = 123;
            standardResponse.ApprenticeshipType = "Standard";
            commitmentsApiClient.Setup(x => x.Get<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
                .ReturnsAsync(cohort)
                .Verifiable();
            courseTypeRulesService
                .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
                .ReturnsAsync(new CourseTypeRulesResult
                {
                    Standard = standardResponse,
                    LearnerAgeRules = learnerAgeResponse
                });

            // Act
            var act = () => handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("When creating an auto reservation, the TransferSenderId must not present");
            commitmentsApiClient.Verify();
        }

        [Test, MoqAutoData]
        public async Task Delete_Reservation_When_No_ReservationID_In_Request(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IAutoReservationsService> autoReservationService,
            [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
            GetStandardsListItem standardResponse,
            GetLearnerAgeResponse learnerAgeResponse,
            AddDraftApprenticeshipCommand request,
            GetCohortResponse cohort,
            AddDraftApprenticeshipCommandHandler handler)
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            request.ReservationId = null;
            cohort.TransferSenderId = null;
            standardResponse.ApprenticeshipType = "FoundationApprenticeship";

            commitmentsApiClient
                .Setup(x => x.Get<GetCohortResponse>(It.Is<GetCohortRequest>(p => p.CohortId == request.CohortId)))
                .ReturnsAsync(cohort);

            courseTypeRulesService
                .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
                .ReturnsAsync(new CourseTypeRulesResult
                {
                    Standard = standardResponse,
                    LearnerAgeRules = learnerAgeResponse
                });

            autoReservationService.Setup(x => x.CreateReservation(It.IsAny<AutoReservation>()))
                .ReturnsAsync(reservationId);

            commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                It.Is<PostAddDraftApprenticeshipRequest>(r =>
                    ((AddDraftApprenticeshipRequest)r.Data).ReservationId == request.ReservationId
                ), true
            )).ThrowsAsync(new Exception("When creating an auto reservation, the TransferSenderId must not present"));

            // Act
            var act = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("When creating an auto reservation, the TransferSenderId must not present");
            autoReservationService.Verify(x=>x.DeleteReservation(It.IsAny<Guid>()), Times.Once);
            courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Does_Not_Create_Or_Delete_Reservation_When_ReservationID_In_Request(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IAutoReservationsService> autoReservationService,
            [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
            GetStandardsListItem standardResponse,
            GetLearnerAgeResponse learnerAgeResponse,
            AddDraftApprenticeshipCommand request,
            GetCohortResponse cohort,
            AddDraftApprenticeshipCommandHandler handler)
        {
            // Arrange
            standardResponse.ApprenticeshipType = "FoundationApprenticeship";

            commitmentsApiClient
                .Setup(x => x.Get<GetCohortResponse>(It.Is<GetCohortRequest>(p => p.CohortId == request.CohortId)))
                .ReturnsAsync(cohort);

            courseTypeRulesService
                .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
                .ReturnsAsync(new CourseTypeRulesResult
                {
                    Standard = standardResponse,
                    LearnerAgeRules = learnerAgeResponse
                });

            commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddDraftApprenticeshipResponse>(
                It.Is<PostAddDraftApprenticeshipRequest>(r =>
                    ((AddDraftApprenticeshipRequest)r.Data).ReservationId == request.ReservationId
                ), true
            )).ThrowsAsync(new Exception("Some error"));

            // Act
            var act = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Some error");
            autoReservationService.Verify(x => x.DeleteReservation(It.IsAny<Guid>()), Times.Never);
            autoReservationService.Verify(x => x.CreateReservation(It.IsAny<AutoReservation>()), Times.Never);
            courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenNoStandardDetails_DoNotCreateDraftApprenticeship(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
            AddDraftApprenticeshipCommand request,
            GetCohortResponse cohort,
            AddDraftApprenticeshipCommandHandler handler)
        {
            // Arrange
            cohort.TransferSenderId = null;
            commitmentsApiClient.Setup(x => x.Get<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
                .ReturnsAsync(cohort)
                .Verifiable();
            courseTypeRulesService.Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
                .ThrowsAsync(new Exception($"Standard not found for course ID {request.CourseCode}"));

            // Act
            var act = () => handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Standard not found for course ID {request.CourseCode}");
            commitmentsApiClient.Verify();
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenNoLearnerAgeRules_DoNotCreateDraftApprenticeship(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
            GetStandardsListItem standardResponse,
            AddDraftApprenticeshipCommand request,
            GetCohortResponse cohort,
            AddDraftApprenticeshipCommandHandler handler)
        {
            // Arrange
            cohort.TransferSenderId = null;
            standardResponse.ApprenticeshipType = "Standard";
            commitmentsApiClient.Setup(x => x.Get<GetCohortResponse>(It.IsAny<GetCohortRequest>()))
                .ReturnsAsync(cohort)
                .Verifiable();
            courseTypeRulesService.Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
                .ThrowsAsync(new Exception("Learner age rules not found for apprenticeship type Standard"));

            // Act
            var act = () => handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Learner age rules not found for apprenticeship type Standard");
            commitmentsApiClient.Verify();
        }
    }
}