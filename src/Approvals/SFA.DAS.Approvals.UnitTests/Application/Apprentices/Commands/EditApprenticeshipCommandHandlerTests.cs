using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Apprentices.Commands.EditApprenticeship;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Commands;

[TestFixture]
public class EditApprenticeshipCommandHandlerTests
{
    private const string ApprenticeshipType = "FoundationApprenticeship";

    [Test, MoqAutoData]
    public async Task Handle_WhenApprenticeshipNotFound_ShouldThrowException(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        EditApprenticeshipCommand request,
        EditApprenticeshipCommandHandler handler)
    {
        // Arrange
        commitmentsApiClient
            .Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync((GetApprenticeshipResponse)null);

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Apprenticeship {request.ApprenticeshipId} not found");
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenApprenticeshipFound_ShouldValidateApprenticeship(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        EditApprenticeshipCommand request,
        EditApprenticeshipCommandHandler handler)
    {
        // Arrange
        apprenticeshipResponse.CourseCode = request.CourseCode;
        apprenticeshipResponse.StartDate = request.StartDate.Value.AddDays(-1);

        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = new GetStandardsListItem { ApprenticeshipType = ApprenticeshipType },
                LearnerAgeRules = learnerAgeResponse
            });

        commitmentsApiClient
            .Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);

        commitmentsApiClient
            .Setup(x => x.Get<GetTrainingProgrammeResponse>(It.IsAny<GetTrainingProgrammeRequest>()))
            .ReturnsAsync(new GetTrainingProgrammeResponse
            {
                TrainingProgramme = new TrainingProgramme
                {
                    Version = request.Version,
                    Options = []
                }
            });

        commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<ValidateApprenticeshipForEditApiRequest>(),  true))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        commitmentsApiClient.Verify(x => x.PostWithResponseCode<NullResponse>(It.Is<ValidateApprenticeshipForEditApiRequest>(r =>
            ((ValidateApprenticeshipForEditRequest)r.Data).ApprenticeshipId == request.ApprenticeshipId &&
            ((ValidateApprenticeshipForEditRequest)r.Data).EmployerAccountId == request.EmployerAccountId &&
            ((ValidateApprenticeshipForEditRequest)r.Data).ProviderId == request.ProviderId &&
            ((ValidateApprenticeshipForEditRequest)r.Data).FirstName == request.FirstName &&
            ((ValidateApprenticeshipForEditRequest)r.Data).LastName == request.LastName &&
            ((ValidateApprenticeshipForEditRequest)r.Data).Cost == request.Cost &&
            ((ValidateApprenticeshipForEditRequest)r.Data).EmployerReference == request.EmployerReference &&
            ((ValidateApprenticeshipForEditRequest)r.Data).DateOfBirth == request.DateOfBirth &&
            ((ValidateApprenticeshipForEditRequest)r.Data).Email == request.Email &&
            ((ValidateApprenticeshipForEditRequest)r.Data).ULN == request.ULN &&
            ((ValidateApprenticeshipForEditRequest)r.Data).TrainingCode == request.CourseCode &&
            ((ValidateApprenticeshipForEditRequest)r.Data).StartDate == request.StartDate &&
            ((ValidateApprenticeshipForEditRequest)r.Data).EndDate == request.EndDate &&
            ((ValidateApprenticeshipForEditRequest)r.Data).EmploymentEndDate == request.EmploymentEndDate &&
            ((ValidateApprenticeshipForEditRequest)r.Data).DeliveryModel == request.DeliveryModel &&
            ((ValidateApprenticeshipForEditRequest)r.Data).ProviderReference == request.ProviderReference &&
            ((ValidateApprenticeshipForEditRequest)r.Data).Version == request.Version &&
            ((ValidateApprenticeshipForEditRequest)r.Data).Option == request.Option &&
            ((ValidateApprenticeshipForEditRequest)r.Data).EmploymentPrice == request.EmploymentPrice &&
            ((ValidateApprenticeshipForEditRequest)r.Data).MinimumAgeAtApprenticeshipStart == learnerAgeResponse.MinimumAge &&
            ((ValidateApprenticeshipForEditRequest)r.Data).MaximumAgeAtApprenticeshipStart == learnerAgeResponse.MaximumAge
        ), true), Times.Once);

        result.Should().NotBeNull();
        result.ApprenticeshipId.Should().Be(request.ApprenticeshipId);
        result.HasOptions.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenCourseCodeChanged_ShouldGetTrainingProgramme(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        EditApprenticeshipCommand request,
        EditApprenticeshipCommandHandler handler)
    {
        // Arrange
        apprenticeshipResponse.CourseCode = "987";

        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = new GetStandardsListItem { ApprenticeshipType = ApprenticeshipType },
                LearnerAgeRules = learnerAgeResponse
            });

        commitmentsApiClient
            .Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);

        commitmentsApiClient
            .Setup(x => x.Get<GetTrainingProgrammeResponse>(It.IsAny<GetTrainingProgrammeRequest>()))
            .ReturnsAsync(new GetTrainingProgrammeResponse
            {
                TrainingProgramme = new TrainingProgramme
                {
                    Version = "1.0",
                    Options = ["Option1", "Option2"]
                }
            });

        commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<ValidateApprenticeshipForEditApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        commitmentsApiClient.Verify(x => x.Get<GetTrainingProgrammeResponse>(It.Is<GetTrainingProgrammeRequest>(r =>
            r.CourseCode == request.CourseCode)), Times.Once);

        result.Should().NotBeNull();
        result.ApprenticeshipId.Should().Be(request.ApprenticeshipId);
        result.HasOptions.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenStandardCourseCode_ShouldGetCalculatedTrainingProgrammeVersion(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        EditApprenticeshipCommand request,
        EditApprenticeshipCommandHandler handler)
    {
        // Arrange
        request.CourseCode = "123";
        apprenticeshipResponse.CourseCode = "987";

        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = new GetStandardsListItem { ApprenticeshipType = ApprenticeshipType },
                LearnerAgeRules = learnerAgeResponse
            });

        commitmentsApiClient
            .Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);

        commitmentsApiClient
            .Setup(x => x.Get<GetTrainingProgrammeResponse>(It.IsAny<GetCalculatedTrainingProgrammeVersionRequest>()))
            .ReturnsAsync(new GetTrainingProgrammeResponse
            {
                TrainingProgramme = new TrainingProgramme
                {
                    Version = "1.0",
                    Options = new List<string>()
                }
            });

        commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<ValidateApprenticeshipForEditApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        commitmentsApiClient.Verify(x => x.Get<GetTrainingProgrammeResponse>(It.Is<GetCalculatedTrainingProgrammeVersionRequest>(r =>
            r.GetUrl.Contains(int.Parse(request.CourseCode).ToString()) &&
            r.GetUrl.Contains(request.StartDate.Value.ToString("O", System.Globalization.CultureInfo.InvariantCulture)))), Times.Once);

        result.Should().NotBeNull();
        result.ApprenticeshipId.Should().Be(request.ApprenticeshipId);
        result.HasOptions.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenNoChanges_ShouldNotGetTrainingProgramme(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        EditApprenticeshipCommand request,
        EditApprenticeshipCommandHandler handler)
    {
        // Arrange
        apprenticeshipResponse.CourseCode = request.CourseCode;
        apprenticeshipResponse.StartDate = request.StartDate.Value;

        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = new GetStandardsListItem { ApprenticeshipType = ApprenticeshipType },
                LearnerAgeRules = learnerAgeResponse
            });

        commitmentsApiClient
            .Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);

        commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<ValidateApprenticeshipForEditApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        commitmentsApiClient.Verify(x => x.Get<GetTrainingProgrammeResponse>(It.IsAny<GetTrainingProgrammeRequest>()), Times.Never);
        commitmentsApiClient.Verify(x => x.Get<GetTrainingProgrammeResponse>(It.IsAny<GetCalculatedTrainingProgrammeVersionRequest>()), Times.Never);

        result.Should().NotBeNull();
        result.ApprenticeshipId.Should().Be(request.ApprenticeshipId);
        result.HasOptions.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenValidationFails_ShouldThrowException(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        EditApprenticeshipCommand request,
        EditApprenticeshipCommandHandler handler)
    {
        // Arrange
        courseTypeRulesService
            .Setup(x => x.GetCourseTypeRulesAsync(request.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult
            {
                Standard = new GetStandardsListItem { ApprenticeshipType = ApprenticeshipType },
                LearnerAgeRules = learnerAgeResponse
            });

        commitmentsApiClient
            .Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);

        commitmentsApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<ValidateApprenticeshipForEditApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.BadRequest, "Validation failed"));

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<Exception>();
    }
}