using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ApprenticeData;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    [TestFixture]
    public class GetChangeOfEmployerApprenticeDataQueryHandlerTests
    {
        private GetChangeOfEmployerApprenticeDataQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;

        private GetApprenticeshipResponse _apprenticeship;
        private GetChangeOfEmployerApprenticeDataQuery _query;
        private GetPriceEpisodesResponse _priceEpisodesResponse;
        private GetAccountLegalEntityResponse _accountLegalEntityResponse;
        private GetTrainingProgrammeResponse _getTrainingProgrammeResponse;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _query = fixture.Create<GetChangeOfEmployerApprenticeDataQuery>();
            _apprenticeship = fixture.Build<GetApprenticeshipResponse>()
                .With(x => x.EmployerAccountId, 123)
                .With(x => x.Id, _query.ApprenticeshipId)
                .Create();

            _priceEpisodesResponse = fixture.Create<GetPriceEpisodesResponse>();
            _accountLegalEntityResponse = fixture.Create<GetAccountLegalEntityResponse>();
            _getTrainingProgrammeResponse = fixture.Create<GetTrainingProgrammeResponse>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetPriceEpisodesResponse>(It.Is<GetPriceEpisodesRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetPriceEpisodesResponse>(_priceEpisodesResponse, HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                       x.GetWithResponseCode<GetAccountLegalEntityResponse>(It.Is<GetAccountLegalEntityRequest>(r => r.AccountLegalEntityId == _query.AccountLegalEntityId)))
                   .ReturnsAsync(new ApiResponse<GetAccountLegalEntityResponse>(_accountLegalEntityResponse, HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetTrainingProgrammeResponse>(It.Is<GetTrainingProgrammeRequest>(r => r.CourseCode == _apprenticeship.CourseCode)))
                .ReturnsAsync(new ApiResponse<GetTrainingProgrammeResponse>(_getTrainingProgrammeResponse, HttpStatusCode.OK, string.Empty));

            _handler = new GetChangeOfEmployerApprenticeDataQueryHandler(_apiClient.Object);
        }

        [Test]
        public async Task Handle_Returns_objects_From_Commitments_Api()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_apprenticeship, result.Apprenticeship);
            Assert.AreEqual(_priceEpisodesResponse, result.PriceEpisodes);
            Assert.AreEqual(_accountLegalEntityResponse, result.AccountLegalEntity);
            Assert.AreEqual(_getTrainingProgrammeResponse, result.TrainingProgrammeResponse);
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
