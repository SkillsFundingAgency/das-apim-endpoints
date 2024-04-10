using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDeliveryModel;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts
{
    [TestFixture]
    public class GetAddDraftApprenticeshipDeliveryModelQueryHandlerTests
    {
        private GetAddDraftApprenticeshipDeliveryModelQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private ServiceParameters _serviceParameters;

        private long _providerId;
        private GetAccountLegalEntityResponse _accountLegalEntity;
        private GetAddDraftApprenticeshipDeliveryModelQuery _query;
        private List<string> _deliveryModels;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _providerId = fixture.Create<long>();
            _serviceParameters = new ServiceParameters(Party.Provider, _providerId);

            _query = fixture.Create<GetAddDraftApprenticeshipDeliveryModelQuery>();
            _deliveryModels = fixture.Create<List<string>>();

            _accountLegalEntity = fixture.Create<GetAccountLegalEntityResponse>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.Is<GetAccountLegalEntityRequest>(r => r.AccountLegalEntityId == _query.AccountLegalEntityId)))
                .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(_accountLegalEntity, HttpStatusCode.OK, string.Empty));

            _deliveryModelService = new Mock<IDeliveryModelService>();
            _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _providerId),
                It.Is<string>(s => s == _query.CourseCode),
                It.Is<long>(ale => ale == _query.AccountLegalEntityId),
                It.Is<long?>(a => a == null)))
            .ReturnsAsync(_deliveryModels);

            _handler = new GetAddDraftApprenticeshipDeliveryModelQueryHandler(_deliveryModelService.Object, _serviceParameters, _apiClient.Object);
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Returned()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_accountLegalEntity.LegalEntityName, Is.EqualTo(result.EmployerName));
        }

        [Test]
        public async Task Handle_DeliveryModels_Is_Returned()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_deliveryModels, Is.EqualTo(result.DeliveryModels));
        }
    }
}
