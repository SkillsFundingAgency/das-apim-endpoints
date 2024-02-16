using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using Standard = SFA.DAS.Approvals.Types.Standard;

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

        private GetApprenticeshipEmailOverlapResponse _emailOverlaps;

        private GetPriorLearningErrorResponse _rplErrors;


        private GetEditDraftApprenticeshipDeliveryModelQueryResult _queryEditDraftResult;
        private List<Standard> _providerStandards;

        private List<string> _deliveryModels;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private Mock<IFjaaService> _fjaaService;
        private Mock<IProviderStandardsService> _providerCoursesService;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _cohort = fixture.Build<GetCohortResponse>()
                .With(x => x.WithParty, Party.Employer)
                .With(x => x.IsLinkedToChangeOfPartyRequest, false)
                .Create();

            _query = fixture.Create<GetCohortDetailsQuery>();

            _queryEditDraftResult = fixture.Create<GetEditDraftApprenticeshipDeliveryModelQueryResult>();


            _draftApprenticeship = fixture.Create<GetDraftApprenticeshipsResponse>();

            _emailOverlaps = fixture.Create<GetApprenticeshipEmailOverlapResponse>();

            _rplErrors = fixture.Create<GetPriorLearningErrorResponse>();


            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetCohortResponse>(_cohort, HttpStatusCode.OK, string.Empty));


            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetDraftApprenticeshipsResponse>(It.Is<GetDraftApprenticeshipsRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipsResponse>(_draftApprenticeship, HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipEmailOverlapResponse>(It.Is<GetApprenticeshipEmailOverlapRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipEmailOverlapResponse>(_emailOverlaps, HttpStatusCode.OK, string.Empty));


            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetPriorLearningErrorResponse>(It.Is<GetPriorLearningErrorRequest>(r => r.CohortId == _query.CohortId)))
                .ReturnsAsync(new ApiResponse<GetPriorLearningErrorResponse>(_rplErrors, HttpStatusCode.OK, string.Empty));


            _deliveryModelService = new Mock<IDeliveryModelService>();

            _fjaaService = new Mock<IFjaaService>();

            _serviceParameters = new ServiceParameters((Approvals.Application.Shared.Enums.Party)_cohort.WithParty, _cohort.AccountId);

            _providerStandards = fixture.Create<List<Standard>>();
            _providerCoursesService = new Mock<IProviderStandardsService>();
            _providerCoursesService.Setup(x => x.GetStandardsData(It.Is<long>(id => id == _cohort.ProviderId)))
                .ReturnsAsync(() => new ProviderStandardsData { Standards = _providerStandards });

            _handler = new GetCohortDetailsQueryHandler(_apiClient.Object, _serviceParameters, _fjaaService.Object, _providerCoursesService.Object);
        }

        [Test]
        public async Task Handle_ProviderName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.ProviderName, Is.EqualTo(result.ProviderName));
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.LegalEntityName, Is.EqualTo(result.LegalEntityName));
        }

        [Test]
        public async Task Handle_InvalidProviderCourseCodes_IsMapped_Empty()
        {
            _providerStandards.Clear();

            foreach (var courseCode in _draftApprenticeship.DraftApprenticeships.Select(x => x.CourseCode).Distinct())
            {
                _providerStandards.Add(new Standard(courseCode, $"test-{courseCode}"));
            }

            var result = await _handler.Handle(_query, CancellationToken.None);

            CollectionAssert.AreEqual(Enumerable.Empty<string>(), result.InvalidProviderCourseCodes);
        }

        [Test]
        public async Task Handle_InvalidProviderCourseCodes_IsMapped_NotEmpty()
        {
            _providerStandards.Clear();

            var result = await _handler.Handle(_query, CancellationToken.None);

            var expected = _draftApprenticeship.DraftApprenticeships.Select(x => x.CourseCode).Distinct();

            CollectionAssert.AreEqual(expected, result.InvalidProviderCourseCodes);
        }

        [Test]
        public async Task Handle_CohortId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.CohortId, Is.EqualTo(result.CohortId));
        }

        [Test]
        public async Task Handle_CohortReference_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.CohortReference, Is.EqualTo(result.CohortReference));
        }

        [Test]
        public async Task Handle_AccountId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.AccountId, Is.EqualTo(result.AccountId));
        }

        [Test]
        public async Task Handle_AccountLegalEntity_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.AccountLegalEntityId, Is.EqualTo(result.AccountLegalEntityId));
        }

        [Test]
        public async Task Handle_ProviderId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.ProviderId, Is.EqualTo(result.ProviderId));
        }

        
        [Test]
        public async Task Handle_IsFundedByTransfer_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.IsFundedByTransfer, Is.EqualTo(result.IsFundedByTransfer));
        }
        
        [Test]
        public async Task Handle_TransferSenderId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.TransferSenderId, Is.EqualTo(result.TransferSenderId));
        }
        
        [Test]
        public async Task Handle_PledgeApplicationId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.PledgeApplicationId, Is.EqualTo(result.PledgeApplicationId));
        }
        
        [Test]
        public async Task Handle_WithParty_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.WithParty, Is.EqualTo(result.WithParty));
        }
        
        [Test]
        public async Task Handle_LatestMessageCreatedByEmployer_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.LatestMessageCreatedByEmployer, Is.EqualTo(result.LatestMessageCreatedByEmployer));
        }
        
        [Test]
        public async Task Handle_LatestMessageCreatedByProvider_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.LatestMessageCreatedByProvider, Is.EqualTo(result.LatestMessageCreatedByProvider));
        }
        
        [Test]
        public async Task Handle_IsApprovedByEmployer_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.IsApprovedByEmployer, Is.EqualTo(result.IsApprovedByEmployer));
        }
        
        [Test]
        public async Task Handle_IsApprovedByProvider_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.IsApprovedByProvider, Is.EqualTo(result.IsApprovedByProvider));
        }
        
        [Test]
        public async Task Handle_IsCompleteForEmployer_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.IsCompleteForEmployer, Is.EqualTo(result.IsCompleteForEmployer));
        }

        [Test]
        public async Task Handle_IsCompleteForProvider_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.IsCompleteForProvider, Is.EqualTo(result.IsCompleteForProvider));
        }
        
        [Test]
        public async Task Handle_LevyStatus_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.LevyStatus, Is.EqualTo(result.LevyStatus));
        }
        
        [Test]
        public async Task Handle_ChangeOfPartyRequestId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.ChangeOfPartyRequestId, Is.EqualTo(result.ChangeOfPartyRequestId));
        }
        
        [Test]
        public async Task Handle_IsLinkedToChangeOfPartyRequest_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.IsLinkedToChangeOfPartyRequest, Is.EqualTo(result.IsLinkedToChangeOfPartyRequest));
        }
        
        [Test]
        public async Task Handle_TransferApprovalStatus_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.TransferApprovalStatus, Is.EqualTo(result.TransferApprovalStatus));
        }

        [Test]
        public async Task Handle_LastAction_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.LastAction, Is.EqualTo(result.LastAction));
        }

        [Test]
        public async Task Handle_ApprenticeEmailIsRequired_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.ApprenticeEmailIsRequired, Is.EqualTo(result.ApprenticeEmailIsRequired));
        }

        [Test]
        public async Task Handle_DraftApprenticeships_Are_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.DraftApprenticeships, Is.EqualTo(result.DraftApprenticeships));
        }
        
        [Test]
        public async Task Handle_ApprenticeshipEmailOverlaps_Are_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_emailOverlaps.ApprenticeshipEmailOverlaps, Is.EqualTo(result.ApprenticeshipEmailOverlaps));
        }

        [Test]
        public async Task Handle_RplErrorDraftApprenticeshipIds_Are_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_rplErrors.DraftApprenticeshipIds, Is.EqualTo(result.RplErrorDraftApprenticeshipIds));
        }
    }
}
