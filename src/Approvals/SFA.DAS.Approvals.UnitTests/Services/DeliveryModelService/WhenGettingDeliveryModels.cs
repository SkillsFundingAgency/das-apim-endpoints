﻿using System.Collections.Generic;
using System.Linq;
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
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Services.DeliveryModelService
{
    public class WhenGettingDeliveryModels
    {
        [TestCase(ApprenticeshipApprovalStatus.Draft)]
        [TestCase(ApprenticeshipApprovalStatus.Approved)]
        public async Task Then_DeliveryModels_Are_Returned_From_ProviderCoursesApi(ApprenticeshipApprovalStatus approvalStatus)
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithApprovalStatus(approvalStatus);
            
            await fixture.GetDeliveryModels();
            
            fixture.VerifyResult();
        }

        [TestCase(ProviderCoursesApiResponse.Null)]
        [TestCase(ProviderCoursesApiResponse.NullResponse)]
        [TestCase(ProviderCoursesApiResponse.EmptyList)]
        public async Task Then_Default_Is_Returned_When_ProviderCoursesApi_Returns_Unexpected_Result(ProviderCoursesApiResponse apiResponse)
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseFromProviderCoursesApi(apiResponse);
            
            await fixture.GetDeliveryModels();
            
            fixture.VerifyResult(DeliveryModelStringTypes.Regular);
        }

        [TestCase(ApprenticeshipApprovalStatus.Draft)]
        [TestCase(ApprenticeshipApprovalStatus.Approved)]
        public async Task Then_FlexiJobAgency_Is_Added_To_Result_When_Employer_Is_A_FlexiJobAgency(ApprenticeshipApprovalStatus approvalStatus)
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithApprovalStatus(approvalStatus)
                .WithResponseHasPortableFlexiJobOptionFromProviderCoursesApi(false)
                .WithFlexiJobAgencyEmployer();

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency);
        }

        [TestCase(ApprenticeshipApprovalStatus.Draft)]
        [TestCase(ApprenticeshipApprovalStatus.Approved)]
        public async Task Then_FlexiJobAgency_Replaces_Portable_Option_When_Employer_Is_A_FlexiJobAgency(ApprenticeshipApprovalStatus approvalStatus)
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithApprovalStatus(approvalStatus)
                .WithResponseHasPortableFlexiJobOptionFromProviderCoursesApi(true)
                .WithFlexiJobAgencyEmployer();

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency);
        }

        [Test]
        public async Task Then_PortableFlexiJob_Is_The_Only_Option_When_Current_Apprenticeship_Is_PortableFlexiJob()
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithApprovalStatus(ApprenticeshipApprovalStatus.Draft)
                .WithResponseHasPortableFlexiJobOptionFromProviderCoursesApi(false)
                .WithContinuationFrom(DeliveryModelStringTypes.PortableFlexiJob);

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.PortableFlexiJob);
        }

        [Test]
        public async Task Then_No_Options_Are_Available_When_Current_Apprenticeship_Is_PortableFlexiJob_And_Employer_Is_FlexiJobAgency()
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithApprovalStatus(ApprenticeshipApprovalStatus.Draft)
                .WithResponseHasPortableFlexiJobOptionFromProviderCoursesApi(false)
                .WithFlexiJobAgencyEmployer()
                .WithContinuationFrom(DeliveryModelStringTypes.PortableFlexiJob);

            await fixture.GetDeliveryModels();

            fixture.VerifyEmptyResult();
        }

        [TestCase(DeliveryModelStringTypes.Regular)]
        [TestCase(DeliveryModelStringTypes.FlexiJobAgency)]
        public async Task Then_Regular_Is_The_Only_Option_When_Current_Apprenticeship_Is_Not_Portable_And_Employer_Is_Not_FlexiJobAgency(string currentDeliveryModel)
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithResponseHasPortableFlexiJobOptionFromProviderCoursesApi(true)
                .WithContinuationFrom(currentDeliveryModel);

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task Then_Default_Is_Returned_When_TrainingCode_Is_Empty(string trainingCode)
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithTrainingCode(trainingCode);

            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular);
        }

        [Test]
        public async Task Then_Post_Approval_FlexiJobAgency_Option_Remains_Even_After_Removal_From_Register()
        {
            var fixture = new DeliveryModelServiceTestFixture()
                .WithApprovalStatus(ApprenticeshipApprovalStatus.Approved)
                .WithDeliveryModel(DeliveryModelStringTypes.FlexiJobAgency)
                .WithDataLockSuccess(false);
                
            await fixture.GetDeliveryModels();

            fixture.VerifyResult(DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency);
        }

        private class DeliveryModelServiceTestFixture
        {
            private readonly Approvals.Services.DeliveryModelService _handler;
            private readonly Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> _apiClient;
            private readonly Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
            private readonly Mock<IFjaaService> _fjaaService;

            private readonly GetHasPortableFlexiJobOptionResponse _apiResponse;
            private readonly GetAccountLegalEntityResponse _accountLegalEntityResponse;
            private readonly long _providerId;
            private readonly long _accountLegalEntityId;
            private long? _apprenticeshipId;
            private string _trainingCode;
            private string _currentDeliveryModel;
            private bool _hasHadDataLockSuccess;
            private bool _isPostApproval;

            private List<string> _result;

            public DeliveryModelServiceTestFixture()
            {
                var fixture = new Fixture();

                _apiClient = new Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>>();
                _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
                _fjaaService = new Mock<IFjaaService>();

                _apiResponse = fixture.Build<GetHasPortableFlexiJobOptionResponse>()
                    .With(x => x.HasPortableFlexiJobOption, false).Create();
                _accountLegalEntityResponse = fixture.Create<GetAccountLegalEntityResponse>();
                _apprenticeshipId = null;
                _trainingCode = "trainingCode";
                _currentDeliveryModel = DeliveryModelStringTypes.Regular;
                _providerId = 1;
                _accountLegalEntityId = 2;

                _apiClient
                    .Setup(x => x.Get<GetHasPortableFlexiJobOptionResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                    .ReturnsAsync(_apiResponse);

                _commitmentsApiClient.Setup(x =>
                        x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                    .ReturnsAsync(_accountLegalEntityResponse);

                _fjaaService = new Mock<IFjaaService>();

                _handler = new Approvals.Services.DeliveryModelService(_apiClient.Object,
                    _commitmentsApiClient.Object,
                    Mock.Of<ILogger<Approvals.Services.DeliveryModelService>>(),
                    _fjaaService.Object
                    );
            }

            public DeliveryModelServiceTestFixture WithContinuationFrom(string deliveryModel)
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
                _fjaaService.Setup(x => x.IsAccountLegalEntityOnFjaaRegister(_accountLegalEntityId))
                    .ReturnsAsync(() => true);

                return this;
            }

            public DeliveryModelServiceTestFixture WithResponseHasPortableFlexiJobOptionFromProviderCoursesApi(bool hasPortableFlexiJobOption)
            {
                _apiResponse.HasPortableFlexiJobOption = hasPortableFlexiJobOption;
                return this;
            }

            public DeliveryModelServiceTestFixture WithTrainingCode(string trainingCode)
            {
                _trainingCode = trainingCode;
                return this;
            }

            public DeliveryModelServiceTestFixture WithDeliveryModel(string deliveryModel)
            {
                _currentDeliveryModel = deliveryModel;
                return this;
            }

            public DeliveryModelServiceTestFixture WithResponseFromProviderCoursesApi(ProviderCoursesApiResponse response)
            {
                if (response == ProviderCoursesApiResponse.NullResponse)
                {
                    _apiClient
                        .Setup(x => x.Get<GetHasPortableFlexiJobOptionResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                        .ReturnsAsync((GetHasPortableFlexiJobOptionResponse)null);
                }

                if (response == ProviderCoursesApiResponse.Null)
                {
                    _apiClient
                        .Setup(x => x.Get<GetHasPortableFlexiJobOptionResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                        .ReturnsAsync(new GetHasPortableFlexiJobOptionResponse { HasPortableFlexiJobOption = false});
                }

                return this;
            }

            public DeliveryModelServiceTestFixture WithDataLockSuccess(bool hasDataLockSuccess)
            {
                _hasHadDataLockSuccess = hasDataLockSuccess;
                return this;
            }

            public DeliveryModelServiceTestFixture WithApprovalStatus(ApprenticeshipApprovalStatus status)
            {
                _isPostApproval = status == ApprenticeshipApprovalStatus.Approved;
                return this;
            }

            public async Task GetDeliveryModels()
            {
                if (_isPostApproval)
                {
                    await GetDeliveryModelsPostApproval();
                }
                else
                {
                    await GetDeliveryModelsPreApproval();
                }
            }

            private async Task GetDeliveryModelsPreApproval()
            {
                _result = await _handler.GetDeliveryModels(_providerId, _trainingCode, _accountLegalEntityId, _apprenticeshipId);
            }

            private async Task GetDeliveryModelsPostApproval()
            {
                var apprenticeship = new GetApprenticeshipResponse
                {
                    ProviderId = _providerId,
                    AccountLegalEntityId = _accountLegalEntityId,
                    CourseCode = _trainingCode,
                    HasHadDataLockSuccess = _hasHadDataLockSuccess,
                    ContinuationOfId = _apprenticeshipId,
                    DeliveryModel = _currentDeliveryModel
                };

                _result = await _handler.GetDeliveryModels(apprenticeship);
            }

            public void VerifyResult()
            {
                var deliveryModels = new List<string> { DeliveryModelStringTypes.Regular };
                
                if (_apiResponse.HasPortableFlexiJobOption)
                    deliveryModels.Add(DeliveryModelStringTypes.PortableFlexiJob);
                
                Assert.That(_result, Is.EqualTo(deliveryModels));
            }

            public void VerifyResult(params string[] expectedResult)
            {
                Assert.That(_result, Is.EqualTo(expectedResult.ToList()));
            }

            public void VerifyEmptyResult()
            {
                Assert.That(_result, Is.Empty);
            }
        }

        public enum ProviderCoursesApiResponse
        {
            NullResponse,
            EmptyList,
            Null
        }

        public enum ApprenticeshipApprovalStatus
        {
            Draft,
            Approved
        }
    }
}