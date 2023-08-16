using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    [TestFixture]
    public class GetManageApprenticeshipDetailsQueryHandlerTests
    {
        private GetManageApprenticeshipDetailsQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private ServiceParameters _serviceParameters;

        private GetApprenticeshipResponse _apprenticeship;
        private GetManageApprenticeshipDetailsQuery _query;
        private List<string> _deliveryModels;
        private GetPriceEpisodesResponse _priceEpisodesResponse;
        private GetApprenticeshipUpdatesResponse _apprenticeshipUpdatesResponse;
        private GetDataLocksResponse _dataLockStatusResponse;
        private GetChangeOfPartyRequestsResponse _changeOfPartyRequestsResponse;
        private GetChangeOfProviderChainResponse _changeOfProviderChainResponse;
        private GetChangeOfEmployerChainResponse _changeOfEmployerChainResponse;
        private GetOverlappingTrainingDateResponse _overlappingTrainingDateResponse;

        
        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _query = fixture.Create<GetManageApprenticeshipDetailsQuery>();
            _apprenticeship = fixture.Build<GetApprenticeshipResponse>()
                .With(x => x.EmployerAccountId, 123)
                .With(x=>x.Id, _query.ApprenticeshipId)
                .Create();

            _priceEpisodesResponse = fixture.Create<GetPriceEpisodesResponse>();
            _apprenticeshipUpdatesResponse = fixture.Create<GetApprenticeshipUpdatesResponse>();
            _dataLockStatusResponse = fixture.Create<GetDataLocksResponse>();
            _changeOfPartyRequestsResponse = fixture.Create<GetChangeOfPartyRequestsResponse>();
            _changeOfProviderChainResponse = fixture.Create<GetChangeOfProviderChainResponse>();
            _changeOfEmployerChainResponse = fixture.Create<GetChangeOfEmployerChainResponse>();
            _overlappingTrainingDateResponse = fixture.Create<GetOverlappingTrainingDateResponse>();

            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetPriceEpisodesResponse>(It.Is<GetPriceEpisodesRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetPriceEpisodesResponse>(_priceEpisodesResponse, HttpStatusCode.OK, string.Empty));
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(It.Is<GetApprenticeshipUpdatesRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipUpdatesResponse>(_apprenticeshipUpdatesResponse, HttpStatusCode.OK, string.Empty));
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetDataLocksResponse>(It.Is<GetDataLocksRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetDataLocksResponse>(_dataLockStatusResponse, HttpStatusCode.OK, string.Empty));
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetChangeOfPartyRequestsResponse>(It.Is<GetChangeOfPartyRequestsRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetChangeOfPartyRequestsResponse>(_changeOfPartyRequestsResponse, HttpStatusCode.OK, string.Empty));
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetChangeOfProviderChainResponse>(It.Is<GetChangeOfProviderChainRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetChangeOfProviderChainResponse>(_changeOfProviderChainResponse, HttpStatusCode.OK, string.Empty));
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetChangeOfEmployerChainResponse>(It.Is<GetChangeOfEmployerChainRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetChangeOfEmployerChainResponse>(_changeOfEmployerChainResponse, HttpStatusCode.OK, string.Empty));
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetOverlappingTrainingDateResponse>(It.Is<GetOverlappingTrainingDateRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetOverlappingTrainingDateResponse>(_overlappingTrainingDateResponse, HttpStatusCode.OK, string.Empty));

            _deliveryModelService = new Mock<IDeliveryModelService>();
            _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _apprenticeship.ProviderId),
                It.Is<string>(s => s == _apprenticeship.CourseCode),
                It.Is<long>(ale => ale == _apprenticeship.AccountLegalEntityId),
                It.Is<long?>(a => a == _apprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);

            _serviceParameters = new ServiceParameters(Approvals.Application.Shared.Enums.Party.Employer, 123);

            _handler = new GetManageApprenticeshipDetailsQueryHandler(_apiClient.Object, _deliveryModelService.Object, _serviceParameters);
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
        public async Task Handle_Returns_objects_From_Commitments_Api()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_apprenticeship, result.Apprenticeship);
            Assert.AreEqual(_priceEpisodesResponse.PriceEpisodes, result.PriceEpisodes);
            Assert.AreEqual(_apprenticeshipUpdatesResponse.ApprenticeshipUpdates, result.ApprenticeshipUpdates);
            Assert.AreEqual(_changeOfPartyRequestsResponse.ChangeOfPartyRequests, result.ChangeOfPartyRequests);
            Assert.AreEqual(_changeOfProviderChainResponse.ChangeOfProviderChain, result.ChangeOfProviderChain);
            Assert.AreEqual(_changeOfEmployerChainResponse.ChangeOfEmployerChain, result.ChangeOfEmployerChain);
            Assert.AreEqual(_overlappingTrainingDateResponse.OverlappingTrainingDateRequest, result.OverlappingTrainingDateRequest);
        }

        [Test]
        public async Task Handle_Returns_null_if_apprenticeship_not_found()
        {

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.IsNull(result);
        }
    }
}
