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

        private GetDraftApprenticeshipsResponse _draftApprenticeship;

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

            _queryEditDraftResult = fixture.Create<GetEditDraftApprenticeshipDeliveryModelQueryResult>();


            _draftApprenticeship = fixture.Create<GetDraftApprenticeshipsResponse>();

            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(_cohort, HttpStatusCode.OK, string.Empty));


            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetDraftApprenticeshipsResponse>(It.Is<GetDraftApprenticeshipsRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipsResponse>(_draftApprenticeship, HttpStatusCode.OK, string.Empty));


            _deliveryModelService = new Mock<IDeliveryModelService>();

            _fjaaService = new Mock<IFjaaService>();

            _serviceParameters = new ServiceParameters((Approvals.Application.Shared.Enums.Party)_cohort.WithParty, _cohort.AccountId);

            _handler = new GetCohortDetailsQueryHandler(_apiClient.Object, _serviceParameters, _fjaaService.Object);
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
