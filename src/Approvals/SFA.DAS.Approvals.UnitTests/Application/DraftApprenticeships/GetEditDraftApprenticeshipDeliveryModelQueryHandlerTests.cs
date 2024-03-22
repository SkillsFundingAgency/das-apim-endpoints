using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships
{
    [TestFixture]
    public class GetEditDraftApprenticeshipDeliveryModelQueryHandlerTests
    {
        private GetEditDraftApprenticeshipDeliveryModelQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private ServiceParameters _serviceParameters;

        private GetCohortResponse _cohort;
        private GetDraftApprenticeshipResponse _draftApprenticeship;
        private GetEditDraftApprenticeshipDeliveryModelQuery _query;
        private List<string> _deliveryModels;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _cohort = fixture.Build<GetCohortResponse>()
                .With(x => x.WithParty, Party.Employer)
                .Create();
            _draftApprenticeship = fixture.Create<GetDraftApprenticeshipResponse>();
            _query = fixture.Create<GetEditDraftApprenticeshipDeliveryModelQuery>();
            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(_cohort, HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(r => r.CohortId == _query.CohortId && r.DraftApprenticeshipId == _query.DraftApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(_draftApprenticeship, HttpStatusCode.OK, string.Empty));

            _deliveryModelService = new Mock<IDeliveryModelService>();
            _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _cohort.ProviderId),
                It.Is<string>(s => s == _query.CourseCode),
                It.Is<long>(ale => ale == _cohort.AccountLegalEntityId),
                It.Is<long?>(a => a == _draftApprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);

            _serviceParameters = new ServiceParameters((Approvals.Application.Shared.Enums.Party)_cohort.WithParty, _cohort.AccountId);

            _handler = new GetEditDraftApprenticeshipDeliveryModelQueryHandler(_deliveryModelService.Object, _serviceParameters, _apiClient.Object);
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Returned()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.LegalEntityName, Is.EqualTo(result.EmployerName));
        }

        [Test]
        public async Task Handle_DeliveryModel_Is_Returned()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.DeliveryModel.ToString(), Is.EqualTo(result.DeliveryModel));
        }

        [Test]
        public async Task Handle_DeliveryModels_Is_Returned()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_deliveryModels, Is.EqualTo(result.DeliveryModels));
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Handle_HasUnavailableDeliveryModel_Is_Returned(bool hasUnavailableDeliveryModel)
        {
            _deliveryModels.Clear();
            if (!hasUnavailableDeliveryModel)
            {
                _deliveryModels.Add(_draftApprenticeship.DeliveryModel.ToString());
            }

            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(hasUnavailableDeliveryModel, Is.EqualTo(result.HasUnavailableDeliveryModel));
        }
    }
}
