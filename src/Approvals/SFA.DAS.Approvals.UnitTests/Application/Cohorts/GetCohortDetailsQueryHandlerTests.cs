using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts
{
    [TestFixture]
    public class GetCohortDetailsQueryHandlerTests
    {
        private GetCohortDetailsQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private ServiceParameters _serviceParameters;

        private GetCohortResponse _cohort;
        private GetCohortDetailsQuery _query;

        private GetDraftApprenticeshipResponse _draftApprenticeship;

        private GetEditDraftApprenticeshipDeliveryModelQuery _queryEditDraft;

        private GetEditDraftApprenticeshipDeliveryModelQueryResult _queryEditDraftResult;


        private List<string> _deliveryModels;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private Mock<IFjaaService> _fjaaService;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _cohort = fixture.Build<GetCohortResponse>()
                .With(x => x.WithParty, Party.Employer)
                .Create();

            _query = fixture.Create<GetCohortDetailsQuery>();

            _queryEditDraft = fixture.Create<GetEditDraftApprenticeshipDeliveryModelQuery>();
            _queryEditDraftResult = fixture.Create<GetEditDraftApprenticeshipDeliveryModelQueryResult>();


            _draftApprenticeship = fixture.Create<GetDraftApprenticeshipResponse>();

            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(_cohort, HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetDraftApprenticeshipResponse>(It.Is<GetDraftApprenticeshipRequest>(r => r.CohortId == _queryEditDraft.CohortId && r.DraftApprenticeshipId == _queryEditDraft.DraftApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipResponse>(_draftApprenticeship, HttpStatusCode.OK, string.Empty));


            _deliveryModelService = new Mock<IDeliveryModelService>();
            _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _cohort.ProviderId),
                It.Is<string>(s => s == _draftApprenticeship.CourseCode),
                It.Is<long>(ale => ale == _cohort.AccountLegalEntityId),
                It.Is<long?>(a => a == _draftApprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);

            _fjaaService = new Mock<IFjaaService>();

            _serviceParameters = new ServiceParameters(_cohort.WithParty, _cohort.AccountId);

            _handler = new GetCohortDetailsQueryHandler(_deliveryModelService.Object, _apiClient.Object, _serviceParameters, _fjaaService.Object);
        }

        [Test]
        public async Task Handle_ProviderName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_cohort.ProviderName, result.ProviderName);
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_cohort.LegalEntityName, result.LegalEntityName);
        }
    }
}
