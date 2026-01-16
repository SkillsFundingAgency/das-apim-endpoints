using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDetails;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using Party = SFA.DAS.Approvals.InnerApi.Responses.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts
{
    [TestFixture]
    public class GetAddDraftApprenticeshipDetailsQueryHandlerTests
    {
        private GetAddDraftApprenticeshipDetailsQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private ServiceParameters _serviceParameters;

        private GetProviderResponse _provider;
        private GetAccountLegalEntityResponse _accountLegalEntity;
        private GetTrainingProgrammeResponse _trainingProgrammeResponse;
        private GetAddDraftApprenticeshipDetailsQuery _query;
        private List<string> _deliveryModels;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _provider = fixture.Create<GetProviderResponse>();
            _accountLegalEntity = fixture.Create<GetAccountLegalEntityResponse>();
            _trainingProgrammeResponse = fixture.Create<GetTrainingProgrammeResponse>();

            _query = fixture.Create<GetAddDraftApprenticeshipDetailsQuery>();
            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetProviderResponse>(_provider, HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.Is<GetAccountLegalEntityRequest>(r => r.AccountLegalEntityId == _query.AccountLegalEntityId)))
                .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(_accountLegalEntity, HttpStatusCode.OK, string.Empty));
            _apiClient.Setup(x =>
                    x.Get<GetTrainingProgrammeResponse>(It.Is<GetCalculatedVersionOfTrainingProgrammeRequest>(r =>
                        r.CourseCode == _query.CourseCode && r.StartDate == _query.StartDate)))
                .ReturnsAsync(_trainingProgrammeResponse);

            _deliveryModelService = new Mock<IDeliveryModelService>();
            _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _query.ProviderId),
                It.Is<string>(s => s == _query.CourseCode),
                It.Is<long>(ale => ale == _query.AccountLegalEntityId),
                It.Is<long?>(a => a == null)))
            .ReturnsAsync(_deliveryModels);

            _serviceParameters = new ServiceParameters((Approvals.Application.Shared.Enums.Party)Party.Employer, _accountLegalEntity.AccountId);

            _handler = new GetAddDraftApprenticeshipDetailsQueryHandler(_apiClient.Object, _deliveryModelService.Object, _serviceParameters);
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        public async Task Handle_HasMultipleDeliveryModelOptions_Reflects_Number_Of_Options_Available(int optionCount, bool expectedHasMultiple)
        {
            var fixture = new Fixture();
            _deliveryModels.Clear();
            _deliveryModels.AddRange(fixture.CreateMany<string>(optionCount));

            var result = await _handler.Handle(_query, CancellationToken.None);
            result.HasMultipleDeliveryModelOptions.Should().Be(expectedHasMultiple);
        }

        [Test]
        public async Task Handle_ProviderName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            result.ProviderName.Should().Be(_provider.Name);
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            result.LegalEntityName.Should().Be(_accountLegalEntity.LegalEntityName);
        }

        [Test]
        public async Task Handle_StandardPageUrl_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            result.StandardPageUrl.Should().Be(_trainingProgrammeResponse.TrainingProgramme.StandardPageUrl);
        }

        [Test]
        public async Task Handle_ProposedMaxFunding_Should_Be_Mapped_To_Null()
        {
            _trainingProgrammeResponse.TrainingProgramme.FundingPeriods = new List<TrainingProgrammeFundingPeriod>();

            var result = await _handler.Handle(_query, CancellationToken.None);
            result.ProposedMaxFunding.Should().BeNull();
        }

        [Test]
        public async Task Handle_ProposedMaxFunding_Should_Be_Mapped()
        {
            _trainingProgrammeResponse.TrainingProgramme.FundingPeriods =
            [
                new TrainingProgrammeFundingPeriod
                {
                    EffectiveFrom = DateTime.MinValue,
                    EffectiveTo = DateTime.MaxValue,
                    FundingCap = 10000
                }
            ];

            var result = await _handler.Handle(_query, CancellationToken.None);
            result.ProposedMaxFunding.Should().Be(10000);
        }
    }
}
