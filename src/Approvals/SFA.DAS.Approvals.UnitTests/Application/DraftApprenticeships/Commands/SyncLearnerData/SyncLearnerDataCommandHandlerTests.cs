using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands.SyncLearnerData
{
    [TestFixture]
    public class SyncLearnerDataCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_WhenIsFlexiJobIsTrue_SetsDeliveryModelToFlexiJobAgency(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler,
            SyncLearnerDataCommand request,
            GetDraftApprenticeshipResponse draftApprenticeshipResponse,
            GetLearnerForProviderResponse learnerDataResponse)
        {
            // Arrange
            draftApprenticeshipResponse.LearnerDataId = 123;
            learnerDataResponse.IsFlexiJob = true;
            learnerDataResponse.FirstName = "John";
            learnerDataResponse.LastName = "Doe";
            learnerDataResponse.TrainingPrice = 1000;
            learnerDataResponse.EpaoPrice = 500;

            var draftApprenticeshipApiResponse = new ApiResponse<GetDraftApprenticeshipResponse>(draftApprenticeshipResponse, HttpStatusCode.OK, null);
            var learnerDataApiResponse = new ApiResponse<GetLearnerForProviderResponse>(learnerDataResponse, HttpStatusCode.OK, null);

            commitmentsApiClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipResponse>(
                It.IsAny<GetDraftApprenticeshipRequest>()))
                .ReturnsAsync(draftApprenticeshipApiResponse);

            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnerForProviderResponse>(
                It.IsAny<GetLearnerForProviderRequest>()))
                .ReturnsAsync(learnerDataApiResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.DeliveryModel.Should().Be(DeliveryModel.FlexiJobAgency);
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenIsFlexiJobIsFalse_SetsDeliveryModelToRegular(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler,
            SyncLearnerDataCommand request,
            GetDraftApprenticeshipResponse draftApprenticeshipResponse,
            GetLearnerForProviderResponse learnerDataResponse)
        {
            // Arrange
            draftApprenticeshipResponse.LearnerDataId = 123;
            learnerDataResponse.IsFlexiJob = false;
            learnerDataResponse.FirstName = "John";
            learnerDataResponse.LastName = "Doe";
            learnerDataResponse.TrainingPrice = 1000;
            learnerDataResponse.EpaoPrice = 500;

            var draftApprenticeshipApiResponse = new ApiResponse<GetDraftApprenticeshipResponse>(draftApprenticeshipResponse, HttpStatusCode.OK, null);
            var learnerDataApiResponse = new ApiResponse<GetLearnerForProviderResponse>(learnerDataResponse, HttpStatusCode.OK, null);

            commitmentsApiClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipResponse>(
                It.IsAny<GetDraftApprenticeshipRequest>()))
                .ReturnsAsync(draftApprenticeshipApiResponse);

            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnerForProviderResponse>(
                It.IsAny<GetLearnerForProviderRequest>()))
                .ReturnsAsync(learnerDataApiResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.DeliveryModel.Should().Be(DeliveryModel.Regular);
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenNormalDraft_AndLearnerHasDifferentStandardCode_UpdatesCourseFromLearnerData(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
            [Frozen] Mock<ITrainingProgrammeResolutionService> trainingProgrammeResolutionService,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler,
            SyncLearnerDataCommand request,
            GetDraftApprenticeshipResponse draftApprenticeshipResponse,
            GetLearnerForProviderResponse learnerDataResponse,
            GetTrainingProgrammeResponse trainingProgrammeResponse)
        {
            draftApprenticeshipResponse.LearnerDataId = 123;
            draftApprenticeshipResponse.CourseCode = "1";
            draftApprenticeshipResponse.TrainingCourseName = "Old Course";
            draftApprenticeshipResponse.StandardUId = "STL1_1";
            learnerDataResponse.FirstName = "John";
            learnerDataResponse.LastName = "Doe";
            learnerDataResponse.TrainingPrice = 1000;
            learnerDataResponse.EpaoPrice = 500;
            learnerDataResponse.StandardCode = 555;
            learnerDataResponse.TrainingName = "New Standard Name";
            learnerDataResponse.StartDate = new System.DateTime(2024, 9, 1);
            trainingProgrammeResponse.TrainingProgramme = new TrainingProgramme
            {
                CourseCode = "555",
                Name = "Resolved Standard",
                StandardUId = "STL555_1",
                Version = "1.0",
                Options = new System.Collections.Generic.List<string> { "Option A" }
            };

            commitmentsApiClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.IsAny<GetDraftApprenticeshipRequest>()))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(draftApprenticeshipResponse, HttpStatusCode.OK, null));
            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnerForProviderResponse>(It.IsAny<GetLearnerForProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetLearnerForProviderResponse>(learnerDataResponse, HttpStatusCode.OK, null));
            trainingProgrammeResolutionService.Setup(x => x.GetTrainingProgrammeAsync("555", learnerDataResponse.StartDate))
                .ReturnsAsync(trainingProgrammeResponse);

            var result = await handler.Handle(request, CancellationToken.None);

            result.CourseCode.Should().Be("555");
            result.TrainingCourseName.Should().Be("New Standard Name");
            result.StandardUId.Should().Be("STL555_1");
            result.TrainingCourseVersion.Should().Be("1.0");
            result.TrainingCourseOption.Should().Be("Option A");
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenNormalDraft_AndTrainingProgrammeNotFound_UpdatesCourseFromLearnerDataOnly(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
            [Frozen] Mock<ITrainingProgrammeResolutionService> trainingProgrammeResolutionService,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler,
            SyncLearnerDataCommand request,
            GetDraftApprenticeshipResponse draftApprenticeshipResponse,
            GetLearnerForProviderResponse learnerDataResponse)
        {
            draftApprenticeshipResponse.LearnerDataId = 123;
            draftApprenticeshipResponse.CourseCode = "1";
            draftApprenticeshipResponse.TrainingCourseName = "Original Course";
            learnerDataResponse.StandardCode = 555;
            learnerDataResponse.TrainingName = "Learner Course Name";
            learnerDataResponse.FirstName = "John";
            learnerDataResponse.LastName = "Doe";
            learnerDataResponse.TrainingPrice = 1000;
            learnerDataResponse.EpaoPrice = 500;

            commitmentsApiClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.IsAny<GetDraftApprenticeshipRequest>()))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(draftApprenticeshipResponse, HttpStatusCode.OK, null));
            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnerForProviderResponse>(It.IsAny<GetLearnerForProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetLearnerForProviderResponse>(learnerDataResponse, HttpStatusCode.OK, null));
            trainingProgrammeResolutionService.Setup(x => x.GetTrainingProgrammeAsync("555", It.IsAny<DateTime?>()))
                .ReturnsAsync((GetTrainingProgrammeResponse)null);

            var result = await handler.Handle(request, CancellationToken.None);

            result.CourseCode.Should().Be("555");
            result.TrainingCourseName.Should().Be("Learner Course Name");
            result.TrainingCourseVersion.Should().Be("");
            result.TrainingCourseOption.Should().Be("");
            result.StandardUId.Should().Be("");
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenNormalDraft_AndLearnerStandardCodeZero_UpdatesCourseFromLearnerDataOnly(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
            [Frozen] Mock<ITrainingProgrammeResolutionService> trainingProgrammeResolutionService,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler,
            SyncLearnerDataCommand request,
            GetDraftApprenticeshipResponse draftApprenticeshipResponse,
            GetLearnerForProviderResponse learnerDataResponse)
        {
            draftApprenticeshipResponse.LearnerDataId = 123;
            draftApprenticeshipResponse.CourseCode = "1";
            draftApprenticeshipResponse.TrainingCourseName = "Original Course";
            learnerDataResponse.StandardCode = 0;
            learnerDataResponse.TrainingName = "Zero Standard Name";
            learnerDataResponse.FirstName = "John";
            learnerDataResponse.LastName = "Doe";
            learnerDataResponse.TrainingPrice = 1000;
            learnerDataResponse.EpaoPrice = 500;

            commitmentsApiClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.IsAny<GetDraftApprenticeshipRequest>()))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(draftApprenticeshipResponse, HttpStatusCode.OK, null));
            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnerForProviderResponse>(It.IsAny<GetLearnerForProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetLearnerForProviderResponse>(learnerDataResponse, HttpStatusCode.OK, null));

            var result = await handler.Handle(request, CancellationToken.None);

            result.CourseCode.Should().Be("0");
            result.TrainingCourseName.Should().Be("Zero Standard Name");
            result.TrainingCourseVersion.Should().Be("");
            result.TrainingCourseOption.Should().Be("");
            result.StandardUId.Should().Be("");
            trainingProgrammeResolutionService.Verify(x => x.GetTrainingProgrammeAsync(It.IsAny<string>(), It.IsAny<DateTime?>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenNormalDraft_AndLearningTypeIsApprenticeshipUnit_UpdatesCourseFromTrainingCode_WithNoVersionOptionOrStandardUId(
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
            [Frozen] Mock<ITrainingProgrammeResolutionService> trainingProgrammeResolutionService,
            [Frozen] Mock<ILogger<SyncLearnerDataCommandHandler>> logger,
            SyncLearnerDataCommandHandler handler,
            SyncLearnerDataCommand request,
            GetDraftApprenticeshipResponse draftApprenticeshipResponse,
            GetLearnerForProviderResponse learnerDataResponse,
            GetTrainingProgrammeResponse trainingProgrammeResponse)
        {
            draftApprenticeshipResponse.LearnerDataId = 123;
            draftApprenticeshipResponse.CourseCode = "1";
            draftApprenticeshipResponse.TrainingCourseName = "Old Course";
            draftApprenticeshipResponse.StandardUId = "STL1_1";
            learnerDataResponse.FirstName = "John";
            learnerDataResponse.LastName = "Doe";
            learnerDataResponse.TrainingPrice = 1000;
            learnerDataResponse.EpaoPrice = 500;
            learnerDataResponse.LearningType = "ApprenticeshipUnit";
            learnerDataResponse.TrainingCode = "550-2-1";
            learnerDataResponse.TrainingName = "Framework Unit Name";
            learnerDataResponse.StandardCode = 0;
            trainingProgrammeResponse.TrainingProgramme = new TrainingProgramme
            {
                CourseCode = "550-2-1",
                Name = "Resolved Framework",
                StandardUId = null,
                Version = null,
                Options = null
            };

            commitmentsApiClient.Setup(x => x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.IsAny<GetDraftApprenticeshipRequest>()))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(draftApprenticeshipResponse, HttpStatusCode.OK, null));
            learnerDataClient.Setup(x => x.GetWithResponseCode<GetLearnerForProviderResponse>(It.IsAny<GetLearnerForProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetLearnerForProviderResponse>(learnerDataResponse, HttpStatusCode.OK, null));
            trainingProgrammeResolutionService.Setup(x => x.GetTrainingProgrammeAsync("550-2-1", It.IsAny<DateTime?>()))
                .ReturnsAsync(trainingProgrammeResponse);

            var result = await handler.Handle(request, CancellationToken.None);

            result.CourseCode.Should().Be("550-2-1");
            result.TrainingCourseName.Should().Be("Framework Unit Name");
            result.TrainingCourseVersion.Should().Be("");
            result.TrainingCourseOption.Should().Be("");
            result.StandardUId.Should().Be("");
            trainingProgrammeResolutionService.Verify(x => x.GetTrainingProgrammeAsync("550-2-1", It.IsAny<DateTime?>()), Times.Once);
        }
    }
}
