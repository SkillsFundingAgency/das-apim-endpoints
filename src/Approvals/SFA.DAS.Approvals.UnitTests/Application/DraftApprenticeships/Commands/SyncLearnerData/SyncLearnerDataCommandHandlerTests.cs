using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
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
    }
}
