using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships
{
    [TestFixture]
    public class GetEditDraftApprenticeshipQueryHandlerTests
    {
        private GetEditDraftApprenticeshipQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private ServiceParameters _serviceParameters;

        private GetCohortResponse _cohort;
        private GetDraftApprenticeshipResponse _draftApprenticeship;
        private GetEditDraftApprenticeshipQuery _query;
        private List<string> _deliveryModels;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _cohort = fixture.Build<GetCohortResponse>()
                .With(x => x.WithParty, Party.Employer)
                .Create();
            _draftApprenticeship = fixture.Create<GetDraftApprenticeshipResponse>();
            _query = fixture.Create<GetEditDraftApprenticeshipQuery>();
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
                It.Is<string>(s => s == _draftApprenticeship.CourseCode),
                It.Is<long>(ale => ale == _cohort.AccountLegalEntityId),
                It.Is<long?>(a => a == _draftApprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);

            _serviceParameters = new ServiceParameters(_cohort.WithParty, _cohort.AccountId);

            _handler = new GetEditDraftApprenticeshipQueryHandler(_apiClient.Object, _deliveryModelService.Object, _serviceParameters);
        }

        [Test]
        public async Task Handle_FirstName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.FirstName, result.FirstName);
        }

        [Test]
        public async Task Handle_LastName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.LastName, result.LastName);
        }

        [Test]
        public async Task Handle_ReservationId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.ReservationId, result.ReservationId);
        }

        [Test]
        public async Task Handle_Email_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.Email, result.Email);
        }

        [Test]
        public async Task Handle_Uln_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.Uln, result.Uln);
        }

        [Test]
        public async Task Handle_DeliveryModel_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.DeliveryModel, result.DeliveryModel);
        }

        [Test]
        public async Task Handle_CourseCode_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.CourseCode, result.CourseCode);
        }

        [Test]
        public async Task Handle_StandardUId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.StandardUId, result.StandardUId);
        }

        [Test]
        public async Task Handle_CourseName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.TrainingCourseName, result.CourseName);
        }

        [Test]
        public async Task Handle_Cost_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.Cost, result.Cost);
        }

        [Test]
        public async Task HandleEmploymentPrice_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.EmploymentPrice, result.EmploymentPrice);
        }

        [Test]
        public async Task Handle_EmploymentEndDate_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.EmploymentEndDate, result.EmploymentEndDate);
        }

        [Test]
        public async Task Handle_ProviderReference_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.ProviderReference, result.ProviderReference);
        }

        [Test]
        public async Task Handle_EmployerReference_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.EmployerReference, result.EmployerReference);
        }

        [Test]
        public async Task Handle_IsContinuation_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_draftApprenticeship.IsContinuation, result.IsContinuation);
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
        public async Task Handle_ProviderId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_cohort.ProviderId, result.ProviderId);
        }

        [Test]
        public async Task Handle_ProviderName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_cohort.ProviderName, result.ProviderName);
        }

        [Test]
        public async Task Handle_AccountLegalEntityId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_cohort.AccountLegalEntityId, result.AccountLegalEntityId);
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_cohort.LegalEntityName, result.LegalEntityName);
        }
    }
}
