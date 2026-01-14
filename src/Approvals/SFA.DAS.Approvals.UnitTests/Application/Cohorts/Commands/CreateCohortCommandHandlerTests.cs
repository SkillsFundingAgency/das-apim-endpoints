using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts.Commands;

[TestFixture]
public class CreateCohortCommandHandlerTests
{
    private const string ApprenticeshipType = "Foundation";
    private const int MaximumAge = 25;

    [Test, MoqAutoData]
    public async Task Handle_WhenNoReservationId_ButTransferSender_ShouldNotAutoReserve(
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        request.ReservationId = null;

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("When creating a auto reservation, the TransferSenderId must be null");
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenNoReservationId_AndNoTransferSender_ShouldAutoReserve(
        [Frozen] Mock<IAutoReservationsService> autoReservationsService,
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        request.ReservationId = null;
        request.TransferSenderId = null;

        // Act
        _ = handler.Handle(request, CancellationToken.None);

        // Assert
        autoReservationsService
            .Verify(x => x.CreateReservation(It.Is<AutoReservation>(a => a.AccountId == request.AccountId
                                                                         && a.AccountLegalEntityId ==
                                                                         request.AccountLegalEntityId
                                                                         && a.CourseCode == request.CourseCode
                                                                         && a.StartDate == request.StartDate
                                                                         && a.UserInfo == request.UserInfo)));
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenReservationId__ShouldNotAutoReserve(
        [Frozen] Mock<IAutoReservationsService> autoReservationsService,
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange

        // Act
        _ = handler.Handle(request, CancellationToken.None);

        // Assert
        autoReservationsService.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenHandled_ShouldCreateCohort(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        CreateCohortResponse expectedResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        learnerAgeResponse.MaximumAge = MaximumAge;
        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = new GetStandardsListItem { ApprenticeshipType = ApprenticeshipType },
                LearnerAgeRules = learnerAgeResponse
            });

        commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.Is<PostCreateCohortRequest>(r =>
                    ((CreateCohortRequest)r.Data).AccountId == request.AccountId &&
                    ((CreateCohortRequest)r.Data).AccountLegalEntityId == request.AccountLegalEntityId &&
                    ((CreateCohortRequest)r.Data).ActualStartDate == request.ActualStartDate &&
                    ((CreateCohortRequest)r.Data).Cost == request.Cost &&
                    ((CreateCohortRequest)r.Data).TrainingPrice == request.TrainingPrice &&
                    ((CreateCohortRequest)r.Data).EndPointAssessmentPrice == request.EndPointAssessmentPrice &&
                    ((CreateCohortRequest)r.Data).CourseCode == request.CourseCode &&
                    ((CreateCohortRequest)r.Data).DateOfBirth == request.DateOfBirth &&
                    ((CreateCohortRequest)r.Data).DeliveryModel == request.DeliveryModel &&
                    ((CreateCohortRequest)r.Data).Email == request.Email &&
                    ((CreateCohortRequest)r.Data).EmploymentEndDate == request.EmploymentEndDate &&
                    ((CreateCohortRequest)r.Data).EmploymentPrice == request.EmploymentPrice &&
                    ((CreateCohortRequest)r.Data).EndDate == request.EndDate &&
                    ((CreateCohortRequest)r.Data).FirstName == request.FirstName &&
                    ((CreateCohortRequest)r.Data).IgnoreStartDateOverlap == request.IgnoreStartDateOverlap &&
                    ((CreateCohortRequest)r.Data).IsOnFlexiPaymentPilot == request.IsOnFlexiPaymentPilot &&
                    ((CreateCohortRequest)r.Data).LastName == request.LastName &&
                    ((CreateCohortRequest)r.Data).OriginatorReference == request.OriginatorReference &&
                    ((CreateCohortRequest)r.Data).PledgeApplicationId == request.PledgeApplicationId &&
                    ((CreateCohortRequest)r.Data).ProviderId == request.ProviderId &&
                    ((CreateCohortRequest)r.Data).ReservationId == request.ReservationId &&
                    ((CreateCohortRequest)r.Data).StartDate == request.StartDate &&
                    ((CreateCohortRequest)r.Data).TransferSenderId == request.TransferSenderId &&
                    ((CreateCohortRequest)r.Data).Uln == request.Uln &&
                    ((CreateCohortRequest)r.Data).UserInfo == request.UserInfo &&
                    ((CreateCohortRequest)r.Data).RequestingParty == request.RequestingParty &&
                    ((CreateCohortRequest)r.Data).LearnerDataId == request.LearnerDataId &&
                    ((CreateCohortRequest)r.Data).MinimumAgeAtApprenticeshipStart == learnerAgeResponse.MinimumAge &&
                    ((CreateCohortRequest)r.Data).MaximumAgeAtApprenticeshipStart == learnerAgeResponse.MaximumAge
                ), true
            )).ReturnsAsync(new ApiResponse<CreateCohortResponse>(expectedResponse, HttpStatusCode.OK, string.Empty))
            .Verifiable();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        commitmentsApiClient.Verify();
        result.Should().NotBeNull();
        result.CohortId.Should().Be(expectedResponse.CohortId);
        result.CohortReference.Should().Be(expectedResponse.CohortReference);
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenNoStandardDetails_DoNotCreateCohort(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ThrowsAsync(new Exception($"Standard not found for course ID {request.CourseCode}"));

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<Exception>().WithMessage($"Standard not found for course ID {request.CourseCode}");
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenNoCourseType_DoNotCreateCohort(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetStandardsListItem standardResponse,
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = ApprenticeshipType;
        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ThrowsAsync(new Exception($"Learner age rules not found for apprenticeship type {ApprenticeshipType}"));

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<Exception>()
            .WithMessage($"Learner age rules not found for apprenticeship type {ApprenticeshipType}");
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenHandled_ShouldGetCourse(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetStandardsListItem standardResponse,
        CreateCohortCommand request,
        CreateCohortResponse createCohortResponse,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = ApprenticeshipType;
        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = standardResponse,
                LearnerAgeRules = new GetLearnerAgeResponse()
            });

        commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.Is<PostCreateCohortRequest>(r =>
                    ((CreateCohortRequest)r.Data).CourseCode == request.CourseCode
                ), true
            )).ReturnsAsync(new ApiResponse<CreateCohortResponse>(createCohortResponse, HttpStatusCode.OK, string.Empty))
            .Verifiable();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        commitmentsApiClient.Verify();
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenHandled_ShouldGetLearnerAgeRules(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetStandardsListItem standardResponse,
        GetLearnerAgeResponse getLearnerAgeResponse,
        CreateCohortCommand request,
        CreateCohortResponse createCohortResponse,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = ApprenticeshipType;
        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = standardResponse,
                LearnerAgeRules = getLearnerAgeResponse
            });

        commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.Is<PostCreateCohortRequest>(r =>
                    ((CreateCohortRequest)r.Data).MinimumAgeAtApprenticeshipStart == getLearnerAgeResponse.MinimumAge &&
                    ((CreateCohortRequest)r.Data).MaximumAgeAtApprenticeshipStart == getLearnerAgeResponse.MaximumAge
                ), true
            )).ReturnsAsync(new ApiResponse<CreateCohortResponse>(createCohortResponse, HttpStatusCode.OK, string.Empty))
            .Verifiable();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        commitmentsApiClient.Verify();
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenCohortCreationFails_ShouldDeleteReservation(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<IAutoReservationsService> autoReservationsService,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        CreateCohortCommand request,
        Guid reservationId,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        request.ReservationId = null;
        request.TransferSenderId = null;

        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = new GetStandardsListItem(),
                LearnerAgeRules = new GetLearnerAgeResponse()
            });

        autoReservationsService.Setup(x => x.CreateReservation(It.IsAny<AutoReservation>()))
            .ReturnsAsync(reservationId);

        commitmentsApiClient.Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.IsAny<PostCreateCohortRequest>(), true
            )).ThrowsAsync(new Exception("Some error"));

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Some error");
        autoReservationsService.Verify(x => x.DeleteReservation(reservationId), Times.Once);
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(request.CourseCode), Times.Once);
    }
}