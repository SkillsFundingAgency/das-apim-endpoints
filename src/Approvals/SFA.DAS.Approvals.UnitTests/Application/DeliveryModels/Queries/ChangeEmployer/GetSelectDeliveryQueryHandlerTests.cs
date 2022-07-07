using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.SelectDeliveryModel;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.DeliveryModels.Queries.ChangeEmployer
{
    [TestFixture]
    public class GetSelectDeliveryQueryHandlerTests
    {
        private GetSelectDeliveryModelQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private static readonly Fixture _fixture = new Fixture();
        private readonly GetSelectDeliveryModelQuery _query = _fixture.Create<GetSelectDeliveryModelQuery>();
        private readonly GetApprenticeshipResponse _apprenticeshipResponse = _fixture.Create<GetApprenticeshipResponse>();
        private readonly List<string> _deliveryModels = _fixture.Create<List<string>>();

        [SetUp]
        public void Setup()
        {
            _apprenticeshipResponse.ProviderId = _query.ProviderId;
            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _commitmentsApiClient.Setup(x =>
                    x.Get<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(_apprenticeshipResponse);

            _deliveryModelService = new Mock<IDeliveryModelService>();
            _deliveryModelService.Setup(x =>
                    x.GetDeliveryModels(It.Is<long>(p => p == _apprenticeshipResponse.ProviderId),
                        It.Is<string>(c => c == _apprenticeshipResponse.CourseCode),
                        It.Is<long>(ale => ale == _apprenticeshipResponse.AccountLegalEntityId)))
                .ReturnsAsync(_deliveryModels);

            _handler = new GetSelectDeliveryModelQueryHandler(_commitmentsApiClient.Object,
                _deliveryModelService.Object);
        }

        [Test]
        public async Task Handle_Returns_DeliveryModels_For_Apprenticeship()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.AreEqual(_deliveryModels, result.DeliveryModels);
        }

        [Test]
        public async Task Handle_Returns_Current_DeliveryModel_For_Apprenticeship()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.AreEqual(_apprenticeshipResponse.DeliveryModel, result.CurrentDeliveryModel);
        }

        [Test]
        public async Task Handle_Returns_LegalEntityName_For_Apprenticeship()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.AreEqual(_apprenticeshipResponse.EmployerName, result.LegalEntityName);
        }

        [Test]
        public async Task Handle_Returns_Null_If_Apprentice_Is_Not_Found()
        {
            _commitmentsApiClient.Setup(x =>
                    x.Get<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(() => null);

            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.IsNull(result);
        }

        [Test]
        public async Task Handle_Returns_Null_If_Apprentice_Does_Not_Belong_To_Provider()
        {
            _apprenticeshipResponse.ProviderId = _query.ProviderId + 1;
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.IsNull(result);
        }
    }
}
