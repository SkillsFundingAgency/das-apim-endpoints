using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ConfirmEmployer;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.DeliveryModels.Queries.ChangeEmployer
{
    [TestFixture]
    public class GetConfirmEmployerQueryHandlerTests
    {
        private GetConfirmEmployerQueryHandler _handler;
        private GetConfirmEmployerQuery _query;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Mock<IFjaaService> _fjaaService;
        private GetAccountLegalEntityResponse _accountLegalEntityApiResponse;
        private GetApprenticeshipResponse _apprenticeshipApiResponse;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _query = fixture.Create<GetConfirmEmployerQuery>();
            
            _accountLegalEntityApiResponse = fixture.Create<GetAccountLegalEntityResponse>();
            _apprenticeshipApiResponse = fixture.Build<GetApprenticeshipResponse>()
                .With(x => x.ProviderId, _query.ProviderId)
                .Create();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _commitmentsApiClient.Setup(x =>
                    x.Get<GetAccountLegalEntityResponse>(
                        It.Is<GetAccountLegalEntityRequest>(x =>
                            x.AccountLegalEntityId == _query.AccountLegalEntityId)))
                .ReturnsAsync(_accountLegalEntityApiResponse);

            _commitmentsApiClient.Setup(x =>
                    x.Get<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(x =>
                            x.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(_apprenticeshipApiResponse);

            _fjaaService = new Mock<IFjaaService>();
            _fjaaService.Setup(x =>
                    x.IsAccountLegalEntityOnFjaaRegister(It.Is<long>(ale => ale == _query.AccountLegalEntityId)))
                .ReturnsAsync(false);

            _handler = new GetConfirmEmployerQueryHandler(_commitmentsApiClient.Object, _fjaaService.Object);
        }

        [Test]
        public async Task Handle_Returns_Current_LegalEntityName_For_Apprenticeship()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_apprenticeshipApiResponse.EmployerName, result.LegalEntityName);
        }

        [Test]
        public async Task Handle_Returns_Current_DeliveryModel_For_Apprenticeship()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_apprenticeshipApiResponse.DeliveryModel, result.DeliveryModel);
        }

        [Test]
        public async Task Handle_Returns_New_AccountLegalEntityName()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_accountLegalEntityApiResponse.AccountName, result.AccountName);
        }

        [Test]
        public async Task Handle_Returns_New_AccountName()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_accountLegalEntityApiResponse.LegalEntityName, result.AccountLegalEntityName);
        }

        [Test]
        public async Task Handle_Returns_IsFlexiJobAgency_True_When_On_Register()
        {
            _fjaaService.Setup(x =>
                    x.IsAccountLegalEntityOnFjaaRegister(It.Is<long>(ale => ale == _query.AccountLegalEntityId)))
                .ReturnsAsync(true);

            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.IsTrue(result.IsFlexiJobAgency);
        }

        [Test]
        public async Task Handle_Returns_IsFlexiJobAgency_False_When_Not_On_Register()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.IsFalse(result.IsFlexiJobAgency);
        }

        [Test]
        public async Task Handle_Returns_Null_If_Apprenticehip_Does_Not_Belong_To_Provider()
        {
            _apprenticeshipApiResponse.ProviderId = _query.ProviderId + 1;
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(result, Is.Null);
        }
    }
}
