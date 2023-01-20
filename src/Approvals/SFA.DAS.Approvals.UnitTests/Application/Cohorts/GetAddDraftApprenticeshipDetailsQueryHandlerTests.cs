using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDetails;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

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
        private GetAddDraftApprenticeshipDetailsQuery _query;
        private List<string> _deliveryModels;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _provider = fixture.Create<GetProviderResponse>();
            _accountLegalEntity = fixture.Create<GetAccountLegalEntityResponse>();

            _query = fixture.Create<GetAddDraftApprenticeshipDetailsQuery>();
            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                x.GetWithResponseCode<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(new ApiResponse<GetProviderResponse>(_provider, HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.Is<GetAccountLegalEntityRequest>(r => r.AccountLegalEntityId == _query.AccountLegalEntityId)))
                .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(_accountLegalEntity, HttpStatusCode.OK, string.Empty));

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
            Assert.AreEqual(expectedHasMultiple, result.HasMultipleDeliveryModelOptions);
        }

        [Test]
        public async Task Handle_ProviderName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_provider.Name, result.ProviderName);
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_accountLegalEntity.LegalEntityName, result.LegalEntityName);
        }
       
    }
}
