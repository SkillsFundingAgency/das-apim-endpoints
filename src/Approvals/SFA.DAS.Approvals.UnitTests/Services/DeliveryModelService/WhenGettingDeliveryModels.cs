using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
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

namespace SFA.DAS.Approvals.UnitTests.Services.DeliveryModelService
{
    public class WhenGettingDeliveryModels
    {
        [Test]
        public async Task Then_DeliveryModels_Are_Returned_From_ProviderCoursesApi()
        {
            var fixture = new DeliveryModelServiceTestFixture();
            
            await fixture.GetDeliveryModels();
            
            fixture.VerifyResult();
        }

        [TestCase(ProviderCoursesApiResponse.EmptyList)]
        [TestCase(ProviderCoursesApiResponse.Null)]
        [TestCase(ProviderCoursesApiResponse.NullResponse)]
        public async Task Then_Default_Is_Returned_When_ProviderCoursesApi_Returns_Unexpected_Result(ProviderCoursesApiResponse apiResponse)
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseFromProviderCoursesApi(apiResponse);
            
            await fixture.GetDeliveryModels();
            
            fixture.VerifyResult(DeliveryModelStringTypes.Regular);
        }

        [Test]
        public async Task Then_FlexiJobAgency_Is_Added_To_Result_When_Employer_Is_A_FlexiJobAgency()
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseFromProviderCoursesApi(DeliveryModelStringTypes.Regular)
                .WithFlexiJobAgencyEmployer();

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency);
        }

        [Test]
        public async Task Then_FlexiJobAgency_Functionality_Is_Subject_To_Toggle()
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseFromProviderCoursesApi(DeliveryModelStringTypes.Regular)
                .WithFlexiJobAgencyToggleOff()
                .WithFlexiJobAgencyEmployer();

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular);
        }

        [Test]
        public async Task Then_FlexiJobAgency_Replaces_Portable_Option_When_Employer_Is_A_FlexiJobAgency()
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseFromProviderCoursesApi(DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.PortableFlexiJob)
                .WithFlexiJobAgencyEmployer();

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency);
        }

        [Test]
        public async Task Then_PortableFlexiJob_Is_The_Only_Option_When_Current_Apprenticeship_Is_PortableFlexiJob()
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseFromProviderCoursesApi(DeliveryModelStringTypes.Regular)
                .WithCurrentApprenticeship(DeliveryModelStringTypes.PortableFlexiJob);

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.PortableFlexiJob);
        }

        [Test]
        public async Task Then_No_Options_Are_Available_When_Current_Apprenticeship_Is_PortableFlexiJob_And_Employer_Is_FlexiJobAgency()
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseFromProviderCoursesApi(DeliveryModelStringTypes.Regular)
                .WithFlexiJobAgencyEmployer()
                .WithCurrentApprenticeship(DeliveryModelStringTypes.PortableFlexiJob);

            await fixture.GetDeliveryModels();

            fixture.VerifyEmptyResult();
        }

        [TestCase(DeliveryModelStringTypes.Regular)]
        [TestCase(DeliveryModelStringTypes.FlexiJobAgency)]
        public async Task Then_Regular_Is_The_Only_Option_When_Current_Apprenticeship_Is_Not_Portable_And_Employer_Is_Not_FlexiJobAgency(string currentDeliveryModel)
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseFromProviderCoursesApi(DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.PortableFlexiJob)
                .WithCurrentApprenticeship(currentDeliveryModel);

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular);
        }

        private class DeliveryModelServiceTestFixture
        {
            private readonly Approvals.Services.DeliveryModelService _handler;
            private readonly Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> _apiClient;
            private readonly Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
            private readonly Mock<IFjaaApiClient<FjaaApiConfiguration>> _fjaaApiClient;
            private readonly FeatureToggles _featureToggles;

            private readonly GetDeliveryModelsResponse _apiResponse;
            private readonly GetAccountLegalEntityResponse _accountLegalEntityResponse;
            private ApiResponse<GetAgencyResponse> _flexiJobAgencyResponse;
            private long? _apprenticeshipId;

            private List<string> _result;

            public DeliveryModelServiceTestFixture()
            {
                var fixture = new Fixture();

                _apiClient = new Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>>();
                _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
                _fjaaApiClient = new Mock<IFjaaApiClient<FjaaApiConfiguration>>();
                _featureToggles = new FeatureToggles { ApprovalsFeatureToggleFjaaEnabled = true };

                _apiResponse = fixture.Create<GetDeliveryModelsResponse>();
                _accountLegalEntityResponse = fixture.Create<GetAccountLegalEntityResponse>();
                _flexiJobAgencyResponse = new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty);
                _apprenticeshipId = null;

                _apiClient
                    .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                    .ReturnsAsync(_apiResponse);

                _commitmentsApiClient.Setup(x =>
                        x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                    .ReturnsAsync(_accountLegalEntityResponse);

                _fjaaApiClient.Setup(x => x
                        .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                    .ReturnsAsync(_flexiJobAgencyResponse);

                _handler = new Approvals.Services.DeliveryModelService(_apiClient.Object,
                    _fjaaApiClient.Object,
                    _commitmentsApiClient.Object,
                    Mock.Of<ILogger<Approvals.Services.DeliveryModelService>>(),
                    _featureToggles);
            }

            public DeliveryModelServiceTestFixture WithCurrentApprenticeship(string deliveryModel)
            {
                var fixture = new Fixture();
                _apprenticeshipId = fixture.Create<long>();

                _commitmentsApiClient.Setup(x =>
                        x.Get<GetApprenticeshipResponse>(
                            It.Is<GetApprenticeshipRequest>(x => x.ApprenticeshipId == _apprenticeshipId)))
                    .ReturnsAsync(new GetApprenticeshipResponse
                    { DeliveryModel = deliveryModel });

                return this;
            }

            public DeliveryModelServiceTestFixture WithFlexiJobAgencyEmployer()
            {
                _flexiJobAgencyResponse =
                    new ApiResponse<GetAgencyResponse>(new GetAgencyResponse { LegalEntityId = 123 },
                        HttpStatusCode.OK, string.Empty);

                _fjaaApiClient.Setup(x => x
                        .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                    .ReturnsAsync(_flexiJobAgencyResponse);

                return this;
            }

            public DeliveryModelServiceTestFixture WithFlexiJobAgencyToggleOff()
            {
                _featureToggles.ApprovalsFeatureToggleFjaaEnabled = false;
                return this;
            }

            public DeliveryModelServiceTestFixture WithResponseFromProviderCoursesApi(params string[] responses)
            {
                _apiResponse.DeliveryModels = responses.ToList();
                return this;
            }

            public DeliveryModelServiceTestFixture WithResponseFromProviderCoursesApi(ProviderCoursesApiResponse response)
            {
                if (response == ProviderCoursesApiResponse.NullResponse)
                {
                    _apiClient
                        .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                        .ReturnsAsync((GetDeliveryModelsResponse)null);
                }

                if (response == ProviderCoursesApiResponse.EmptyList)
                {
                    _apiClient
                        .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                        .ReturnsAsync(new GetDeliveryModelsResponse
                        {
                            DeliveryModels = new System.Collections.Generic.List<string>()
                        });
                }

                if (response == ProviderCoursesApiResponse.Null)
                {
                    _apiClient
                        .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                        .ReturnsAsync(new GetDeliveryModelsResponse { DeliveryModels = null });
                }

                return this;
            }

            public async Task GetDeliveryModels()
            {
                _result = await _handler.GetDeliveryModels(1, "trainingCode", 2, _apprenticeshipId);
            }

            public void VerifyResult()
            {
                Assert.AreEqual(_apiResponse.DeliveryModels, _result);
            }

            public void VerifyResult(params string[] expectedResult)
            {
                Assert.AreEqual(expectedResult.ToList(), _result);
            }

            public void VerifyEmptyResult()
            {
                Assert.IsEmpty(_result);
            }
        }

        public enum ProviderCoursesApiResponse
        {
            NullResponse,
            EmptyList,
            Null
        }
    }
}