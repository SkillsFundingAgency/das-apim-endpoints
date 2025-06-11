using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Cohorts.Commands.CreateCohort;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.InnerApi.TrainingTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.TrainingTypesApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

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
        [Frozen] Mock<ITrainingTypesApiClient> trainingTypesApiClient,
        CreateCohortResponse expectedResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        learnerAgeResponse.MaximumAge = MaximumAge;
        trainingTypesApiClient
            .Setup(x => x.Get<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
            .ReturnsAsync(learnerAgeResponse);

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
                    ((CreateCohortRequest)r.Data).LearnerDataId == request.LearnerDataId
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
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenNoStandardDetails_DoNotCreateCohort(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ITrainingTypesApiClient> trainingTypesApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        coursesApiClient
            .Setup(x => x.Get<GetStandardsListItem>(
                It.Is<GetStandardDetailsByIdRequest>(x => x.GetUrl.Contains(request.CourseCode))))
            .ReturnsAsync((GetStandardsListItem)null)
            .Verifiable();

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<Exception>().WithMessage($"Standard not found for course ID {request.CourseCode}");
        coursesApiClient.Verify();
        trainingTypesApiClient.VerifyNoOtherCalls();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenNoTrainingType_DoNotCreateCohort(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ITrainingTypesApiClient> trainingTypesApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        GetStandardsListItem standardResponse,
        CreateCohortCommand request,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = ApprenticeshipType;
        coursesApiClient
            .Setup(x => x.Get<GetStandardsListItem>(
                It.Is<GetStandardDetailsByIdRequest>(x => x.GetUrl.Contains(request.CourseCode))))
            .ReturnsAsync(standardResponse)
            .Verifiable();

        trainingTypesApiClient
            .Setup(x => x.Get<GetLearnerAgeResponse>(
                It.Is<GetLearnerAgeRequest>(x => x.GetUrl.Contains(ApprenticeshipType))))
            .ReturnsAsync((GetLearnerAgeResponse)null)
            .Verifiable();

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<Exception>()
            .WithMessage($"Learner age rules not found for apprenticeship type {ApprenticeshipType}");
        coursesApiClient.Verify();
        trainingTypesApiClient.Verify();
        commitmentsApiClient.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenHandled_ShouldGetCourse(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ITrainingTypesApiClient> trainingTypesApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        GetStandardsListItem standardResponse,
        CreateCohortCommand request,
        CreateCohortResponse createCohortResponse,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = ApprenticeshipType;
        coursesApiClient
            .Setup(x => x.Get<GetStandardsListItem>(
                It.Is<GetStandardDetailsByIdRequest>(x => x.GetUrl.Contains(request.CourseCode))))
            .ReturnsAsync(standardResponse)
            .Verifiable();

        commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.IsAny<PostCreateCohortRequest>(), true))
            .ReturnsAsync(new ApiResponse<CreateCohortResponse>(createCohortResponse, HttpStatusCode.OK, string.Empty))
            .Verifiable();

        // Act
        _ = await handler.Handle(request, CancellationToken.None);

        // Assert
        coursesApiClient.Verify();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenHandled_ShouldGetLearnerAgeRules(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ITrainingTypesApiClient> trainingTypesApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        GetStandardsListItem standardResponse,
        GetLearnerAgeResponse getLearnerAgeResponse,
        CreateCohortCommand request,
        CreateCohortResponse createCohortResponse,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        standardResponse.ApprenticeshipType = ApprenticeshipType;
        coursesApiClient
            .Setup(x => x.Get<GetStandardsListItem>(
                It.Is<GetStandardDetailsByIdRequest>(x => x.GetUrl.Contains(request.CourseCode))))
            .ReturnsAsync(standardResponse)
            .Verifiable();

        commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<CreateCohortResponse>(
                It.IsAny<PostCreateCohortRequest>(), true))
            .ReturnsAsync(new ApiResponse<CreateCohortResponse>(createCohortResponse, HttpStatusCode.OK, string.Empty))
            .Verifiable();

        trainingTypesApiClient
            .Setup(x => x.Get<GetLearnerAgeResponse>(
                It.Is<GetLearnerAgeRequest>(x => x.GetUrl.Contains(ApprenticeshipType))))
            .ReturnsAsync(getLearnerAgeResponse)
            .Verifiable();

        // Act
        _ = await handler.Handle(request, CancellationToken.None);

        // Assert
        coursesApiClient.Verify();
        trainingTypesApiClient.Verify();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenCohortCreationFails_ShouldDeleteReservation(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<IAutoReservationsService> autoReservationsService,
        CreateCohortCommand request,
        Guid reservationId,
        CreateCohortCommandHandler handler)
    {
        // Arrange
        request.ReservationId = Guid.Empty;
        request.TransferSenderId = null;
        autoReservationsService.Setup(x => x.CreateReservation(It.IsAny<AutoReservation>()))
            .ReturnsAsync(reservationId);

        commitmentsApiClient.Setup(x =>
                x.PostWithResponseCode<CreateCohortResponse>(It.IsAny<PostCreateCohortRequest>(), true))
            .ThrowsAsync(new Exception("Failed to create cohort"));

        // Act
        var action = () => handler.Handle(request, CancellationToken.None);

        await action.Should().ThrowAsync<Exception>();
        autoReservationsService.Verify(x => x.DeleteReservation(reservationId), Times.Once);
    }
}