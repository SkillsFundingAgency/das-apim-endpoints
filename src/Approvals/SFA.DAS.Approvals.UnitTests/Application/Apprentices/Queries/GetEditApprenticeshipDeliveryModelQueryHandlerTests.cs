using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    [TestFixture]
    public class GetEditApprenticeshipDeliveryModelQueryHandlerTests
    {
        private GetEditApprenticeshipDeliveryModelQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private static readonly Fixture _fixture = new Fixture();
        private readonly GetEditApprenticeshipDeliveryModelQuery _query = _fixture.Create<GetEditApprenticeshipDeliveryModelQuery>();
        private readonly GetApprenticeshipResponse _apprenticeshipResponse = _fixture.Create<GetApprenticeshipResponse>();
        private readonly List<string> _deliveryModels = _fixture.Create<List<string>>();

        [SetUp]
        public void Setup()
        {
            var serviceParameters = new ServiceParameters(Approvals.Application.Shared.Enums.Party.Employer, _apprenticeshipResponse.EmployerAccountId);

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _commitmentsApiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(() => new ApiResponse<GetApprenticeshipResponse>(_apprenticeshipResponse, HttpStatusCode.OK, string.Empty));

            _deliveryModelService = new Mock<IDeliveryModelService>();
            _deliveryModelService.Setup(x =>
                    x.GetDeliveryModels(It.Is<GetApprenticeshipResponse>(r => r == _apprenticeshipResponse)))
                .ReturnsAsync(_deliveryModels);

            _handler = new GetEditApprenticeshipDeliveryModelQueryHandler(_commitmentsApiClient.Object,
                _deliveryModelService.Object, serviceParameters);
        }

        [Test]
        public async Task Handle_Returns_DeliveryModels_For_Apprenticeship()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.That(result.DeliveryModels, Is.EqualTo(_deliveryModels));
        }

        [Test]
        public async Task Handle_Returns_LegalEntityName_For_Apprenticeship()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.That(result.LegalEntityName, Is.EqualTo(_apprenticeshipResponse.EmployerName));
        }

        [Test]
        public async Task Handle_Returns_Null_If_Apprentice_Is_Not_Found()
        {
            _commitmentsApiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                    .ReturnsAsync(() => new ApiResponse<GetApprenticeshipResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Handle_Returns_Null_If_Apprentice_Does_Not_Belong_To_Provider()
        {
            _apprenticeshipResponse.EmployerAccountId++;
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.That(result, Is.Null);
        }
    }
}
