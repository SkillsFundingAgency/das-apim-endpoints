using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Services.DeliveryModelService
{
    public class WhenGettingDeliveryModels
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_DeliveryModels_Returned(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            GetDeliveryModelsResponse apiResponse,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler
        )
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = true;

            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(apiResponse);

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId);

            actual.Should().BeEquivalentTo(apiResponse.DeliveryModels);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_No_Response_Returned_We_Create_Default(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler
        )
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = true;

            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync((GetDeliveryModelsResponse)null);

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId);

            actual.Should().BeEquivalentTo(DeliveryModelStringTypes.Regular);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_Null_Response_Returned_We_Create_Default(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler
        )
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = true;

            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse { DeliveryModels = null });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId);

            actual.Should().BeEquivalentTo(DeliveryModelStringTypes.Regular);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_Empty_Response_Returned_We_Create_Default(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler)
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = true;

            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse
                {
                    DeliveryModels = new System.Collections.Generic.List<string>() { DeliveryModelStringTypes.Regular }
                });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId);

            actual.Should().BeEquivalentTo(DeliveryModelStringTypes.Regular);
        }

        [Test, MoqAutoData]
        public async Task Then_FlexiJobAgency_Is_Returned_For_LegalEntity_On_FJAA_Register(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler)
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = true;
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse
                {
                    DeliveryModels = new System.Collections.Generic.List<string>() { DeliveryModelStringTypes.Regular }
                });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(new GetAgencyResponse(), HttpStatusCode.OK,
                    string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId);

            var expected = new List<string>
                { DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency };

            actual.Should().BeEquivalentTo(expected);
        }

        [Test, MoqAutoData]
        public async Task Then_FlexiJobAgency_Is_Not_Returned_For_LegalEntity_On_FJAA_Register_When_Toggle_Is_Off(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler)
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = false;

            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse
                {
                    DeliveryModels = new System.Collections.Generic.List<string>() { DeliveryModelStringTypes.Regular }
                });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(new GetAgencyResponse(), HttpStatusCode.OK,
                    string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId);

            var expected = new List<string> { DeliveryModelStringTypes.Regular };

            actual.Should().BeEquivalentTo(expected);
        }

        [Test, MoqAutoData]
        public async Task Then_PortableFlexiJob_Is_Not_Returned_For_LegalEntity_On_FJAA_Register(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler)
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = true;

            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse
                {
                    DeliveryModels = new List<string>()
                        { DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.PortableFlexiJob }
                });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(new GetAgencyResponse(), HttpStatusCode.OK,
                    string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId);

            var expected = new List<string>
                { DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency };

            actual.Should().BeEquivalentTo(expected);
        }

        [Test, MoqAutoData]
        public async Task Then_PortableFlexiJob_Is_The_Only_Option_When_Current_Apprenticeship_Is_PortableFlexiJob(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            long apprenticeshipId,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler)
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = true;

            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse
                {
                    DeliveryModels = new List<string>()
                        { DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.PortableFlexiJob }
                });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(x => x.ApprenticeshipId == apprenticeshipId)))
                .ReturnsAsync(new GetApprenticeshipResponse
                    { DeliveryModel = DeliveryModelStringTypes.PortableFlexiJob });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound,
                    string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId, apprenticeshipId);

            var expected = new List<string> { DeliveryModelStringTypes.PortableFlexiJob };

            actual.Should().BeEquivalentTo(expected);
        }

        [Test, MoqAutoData]
        public async Task Then_No_Options_Are_Available_When_Current_Apprenticeship_Is_PortableFlexiJob_And_Employer_Is_On_Flexijob_Register(
            long providerId,
            string trainingCode,
            long accountLegalEntityId,
            long apprenticeshipId,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            [Frozen] FeatureToggles featureToggles,
            long maLegalEntityId,
            Approvals.Services.DeliveryModelService handler)
        {
            featureToggles.ApprovalsFeatureToggleFjaaEnabled = true;

            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse
                {
                    DeliveryModels = new List<string>()
                        { DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.PortableFlexiJob }
                });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            commitmentsApiClient.Setup(x =>
                    x.Get<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(x => x.ApprenticeshipId == apprenticeshipId)))
                .ReturnsAsync(new GetApprenticeshipResponse
                { DeliveryModel = DeliveryModelStringTypes.PortableFlexiJob });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(new GetAgencyResponse(), HttpStatusCode.OK,
                    string.Empty));

            var actual = await handler.GetDeliveryModels(providerId, trainingCode, accountLegalEntityId, apprenticeshipId);

            actual.Should().BeEmpty();
        }
    }
}