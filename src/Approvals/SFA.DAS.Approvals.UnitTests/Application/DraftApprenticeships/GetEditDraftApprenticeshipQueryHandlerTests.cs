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
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using Party = SFA.DAS.Approvals.InnerApi.Responses.Party;

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
                It.Is<string>(s => s == _query.CourseCode),
                It.Is<long>(ale => ale == _cohort.AccountLegalEntityId),
                It.Is<long?>(a => a == _draftApprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);

            _serviceParameters = new ServiceParameters((Approvals.Application.Shared.Enums.Party)_cohort.WithParty, _cohort.AccountId);

            _handler = new GetEditDraftApprenticeshipQueryHandler(_apiClient.Object, _deliveryModelService.Object, _serviceParameters);
        }

        [Test]
        public async Task Handle_FirstName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.FirstName, Is.EqualTo(result.FirstName));
        }

        [Test]
        public async Task Handle_LastName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.LastName, Is.EqualTo(result.LastName));
        }

        [Test]
        public async Task Handle_ReservationId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.ReservationId, Is.EqualTo(result.ReservationId));
        }

        [Test]
        public async Task Handle_Email_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.Email, Is.EqualTo(result.Email));
        }

        [Test]
        public async Task Handle_Uln_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.Uln, Is.EqualTo(result.Uln));
        }

        [Test]
        public async Task Handle_DeliveryModel_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.DeliveryModel, Is.EqualTo(result.DeliveryModel));
        }

        [Test]
        public async Task Handle_CourseCode_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.CourseCode, Is.EqualTo(result.CourseCode));
        }

        [Test]
        public async Task Handle_StandardUId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.StandardUId, Is.EqualTo(result.StandardUId));
        }

        [Test]
        public async Task Handle_CourseName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.TrainingCourseName, Is.EqualTo(result.CourseName));
        }

        [Test]
        public async Task Handle_Cost_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.Cost, Is.EqualTo(result.Cost));
        }

        [Test]
        public async Task Handle_TrainingPrice_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.TrainingPrice, Is.EqualTo(result.TrainingPrice));
        }

        [Test]
        public async Task Handle_EndPointAssessmentPrice_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.EndPointAssessmentPrice, Is.EqualTo(result.EndPointAssessmentPrice));
        }

        [Test]
        public async Task HandleEmploymentPrice_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.EmploymentPrice, Is.EqualTo(result.EmploymentPrice));
        }

        [Test]
        public async Task Handle_EmploymentEndDate_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.EmploymentEndDate, Is.EqualTo(result.EmploymentEndDate));
        }

        [Test]
        public async Task Handle_ProviderReference_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.ProviderReference, Is.EqualTo(result.ProviderReference));
        }

        [Test]
        public async Task Handle_EmployerReference_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.EmployerReference, Is.EqualTo(result.EmployerReference));
        }

        [Test]
        public async Task Handle_IsContinuation_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.IsContinuation, Is.EqualTo(result.IsContinuation));
        }

        [Test]
        public async Task Handle_StartDate_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.StartDate, Is.EqualTo(result.StartDate));
        }

        [Test]
        public async Task Handle_ActualStartDate_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.ActualStartDate, Is.EqualTo(result.ActualStartDate));
        }

        [Test]
        public async Task Handle_IsOnFlexiPaymentPilot_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.IsOnFlexiPaymentPilot, Is.EqualTo(result.IsOnFlexiPaymentPilot));
        }

        [Test]
        public async Task Handle_EmployerHasEditedCost_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.EmployerHasEditedCost, Is.EqualTo(result.EmployerHasEditedCost));
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
            Assert.That(expectedHasMultiple, Is.EqualTo(result.HasMultipleDeliveryModelOptions));
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Handle_HasUnavailableDeliveryModel_Is_Mapped(bool hasUnavailableDeliveryModel)
        {
            _deliveryModels.Clear();
            if (!hasUnavailableDeliveryModel)
            {
                _deliveryModels.Add(_draftApprenticeship.DeliveryModel.ToString());
            }

            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(hasUnavailableDeliveryModel, Is.EqualTo(result.HasUnavailableDeliveryModel));
        }

        [Test]
        public async Task Handle_ProviderId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.ProviderId, Is.EqualTo(result.ProviderId));
        }

        [Test]
        public async Task Handle_ProviderName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.ProviderName, Is.EqualTo(result.ProviderName));
        }

        [Test]
        public async Task Handle_AccountLegalEntityId_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.AccountLegalEntityId, Is.EqualTo(result.AccountLegalEntityId));
        }

        [Test]
        public async Task Handle_LegalEntityName_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_cohort.LegalEntityName, Is.EqualTo(result.LegalEntityName));
        }

        [Test]
        public async Task Handle_EmailAddressConfirmed_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.EmailAddressConfirmed, Is.EqualTo(result.EmailAddressConfirmed));
        }

        [Test]
        public async Task Handle_RplStillRequired_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.RecognisingPriorLearningStillNeedsToBeConsidered, Is.EqualTo(result.RecognisingPriorLearningStillNeedsToBeConsidered));
        }

        [Test]
        public async Task Handle_RplExtendedStillRequired_Is_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(_draftApprenticeship.RecognisingPriorLearningExtendedStillNeedsToBeConsidered, Is.EqualTo(result.RecognisingPriorLearningExtendedStillNeedsToBeConsidered));
        }
    }
}
