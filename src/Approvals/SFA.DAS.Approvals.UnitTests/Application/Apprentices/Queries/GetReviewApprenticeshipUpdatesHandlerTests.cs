using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions.Common;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;
using Standard = SFA.DAS.Approvals.Types.Standard;
using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetReviewApprenticeshipUpdates;
using SFA.DAS.Approvals.Extensions;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    [TestFixture]
    public class WhenHandlingGetReviewApprenticeshipUpdates
    {
        private WhenHandlingGetReviewApprenticeshipUpdatesFixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new WhenHandlingGetReviewApprenticeshipUpdatesFixture();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_OriginalApprentice_Is_Returned()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithCourseCode("123");
            await _fixture.GetReviewApprenticeshipUpdates();
            _fixture.AssertResultIsOriginalApprentice();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_ApprenticeUpdates_Is_Returned()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithCourseCode("123");
            await _fixture.GetReviewApprenticeshipUpdates();
            _fixture.AssertResultIsTrue();
            _fixture.AssertResultIsApprenticeUpdates();

        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_With_Valid_CourseCode_Returned_True()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithCourseCode("123");
            await _fixture.GetReviewApprenticeshipUpdates();
            _fixture.AssertResultIsTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_With_InvalidValid_CourseCode_Returned_False()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithCourseCode("1234");
            await _fixture.GetReviewApprenticeshipUpdates();
            _fixture.AssertResultIsFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_When_Apprenticeship_Is_Not_Found_Then_Result_Is_Null()
        {
            _fixture.WithInvalidValidApprenticeship();
            await _fixture.GetReviewApprenticeshipUpdates();
            _fixture.AssertResultIsNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_When_CallingParty_Does_Not_Own_Apprenticeship_Then_Result_Is_Null()
        {
            _fixture.WithEmptyApprenticeship();
            await _fixture.GetReviewApprenticeshipUpdates();
            _fixture.AssertResultIsNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_When_ApprenticeshipUpdate_Is_Not_Found_Then_Result_Is_Null()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithNoApprenticeshipUpdate();
            await _fixture.GetReviewApprenticeshipUpdates();
            _fixture.AssertResultIsNull();
        }

     [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_When_PriceEpisodes_Is_Not_Found_Then_Result_Is_Null()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithCourseCode("123");
            _fixture.WithNoPriceEpisodesResponse();
            await _fixture.GetReviewApprenticeshipUpdates();
            _fixture.AssertResultIsNull();
        }
    }

    public class WhenHandlingGetReviewApprenticeshipUpdatesFixture
    {
        private readonly Mock<GetReviewApprenticeshipUpdatesQuery> _query;
        private readonly GetApprenticeshipResponse _apprenticeship;
        private readonly GetApprenticeshipUpdatesResponse _apprenticeshipUpdate;
        private readonly GetPriceEpisodesResponse _priceEpisodesResponse;
        private readonly ProviderStandardsData _providerStandardsData;
        private readonly Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private readonly Mock<IProviderStandardsService> _providerStandardsService;
        private GetReviewApprenticeshipUpdatesQueryHandler _handler;
        private readonly Fixture _fixture;
        private GetReviewApprenticeshipUpdatesQueryResult _result;
        private ServiceParameters _serviceParameters;

        public WhenHandlingGetReviewApprenticeshipUpdatesFixture()
        {
            _fixture = new Fixture();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _providerStandardsService = new Mock<IProviderStandardsService>();
            _query = new Mock<GetReviewApprenticeshipUpdatesQuery>();

            _providerStandardsData = _fixture.Build<ProviderStandardsData>()
                .Create();

            _apprenticeship = _fixture.Build<GetApprenticeshipResponse>()
                .Create();

            _apprenticeship.DeliveryModel = "Regular";

            _apprenticeshipUpdate = _fixture.Build<GetApprenticeshipUpdatesResponse>()
                .Create();

            _priceEpisodesResponse = _fixture.Build<GetPriceEpisodesResponse>()
                .Create();

            _providerStandardsData.Standards = new List<Standard> { new Standard("123", "123") };

            _serviceParameters = new ServiceParameters(Party.Provider, _apprenticeship.ProviderId);

            _handler = new GetReviewApprenticeshipUpdatesQueryHandler(_apiClient.Object,
                _providerStandardsService.Object, _serviceParameters);

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(
                        It.Is<GetApprenticeshipUpdatesRequest>(r =>
                            r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipUpdatesResponse>(_apprenticeshipUpdate,
                    HttpStatusCode.OK, string.Empty));

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetPriceEpisodesResponse>(
                        It.Is<GetPriceEpisodesRequest>(r =>
                            r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetPriceEpisodesResponse>(_priceEpisodesResponse,
                    HttpStatusCode.OK, string.Empty));

            _providerStandardsService.Setup(x => x.GetStandardsData(_apprenticeship.ProviderId))
                .ReturnsAsync(_providerStandardsData);

        }

        public async Task GetReviewApprenticeshipUpdates()
        {
            _result = await _handler.Handle(_query.Object, CancellationToken.None);
        }

        public void AssertResultIsTrue()
        {
            _result.IsValidCourseCode.IsSameOrEqualTo(true);
        }

        public void AssertResultIsNull()
        {
            Assert.IsNull(_result);
        }

        public void AssertResultIsOriginalApprentice()
        {
            Assert.AreEqual(_apprenticeship.EmployerName, _result.EmployerName);
            Assert.AreEqual(_apprenticeship.ProviderName, _result.ProviderName);

            Assert.AreEqual(_apprenticeship.FirstName, _result.OriginalApprenticeship.FirstName);
            Assert.AreEqual(_apprenticeship.LastName, _result.OriginalApprenticeship.LastName);
            Assert.AreEqual(_apprenticeship.Email, _result.OriginalApprenticeship.Email);
            Assert.AreEqual(_apprenticeship.Uln, _result.OriginalApprenticeship.Uln);
            Assert.AreEqual(_apprenticeship.DateOfBirth, _result.OriginalApprenticeship.DateOfBirth);
            Assert.AreEqual(_apprenticeship.StartDate, _result.OriginalApprenticeship.StartDate);
            Assert.AreEqual(_apprenticeship.EndDate, _result.OriginalApprenticeship.EndDate);
            Assert.AreEqual(_apprenticeship.CourseCode, _result.OriginalApprenticeship.CourseCode);
            Assert.AreEqual(_apprenticeship.CourseName, _result.OriginalApprenticeship.CourseName);
            Assert.AreEqual(_apprenticeship.Version, _result.OriginalApprenticeship.Version);
            Assert.AreEqual(_apprenticeship.Option, _result.OriginalApprenticeship.Option);
            Assert.AreEqual((DeliveryModel)Enum.Parse(typeof(DeliveryModel), _apprenticeship.DeliveryModel), _result.OriginalApprenticeship.DeliveryModel);
            Assert.AreEqual(_apprenticeship.EmploymentEndDate, _result.OriginalApprenticeship.EmploymentEndDate);
            Assert.AreEqual(_apprenticeship.EmploymentPrice, _result.OriginalApprenticeship.EmploymentPrice);
            Assert.AreEqual(_apprenticeship.Uln, _result.OriginalApprenticeship.Uln);
            Assert.AreEqual(_priceEpisodesResponse.PriceEpisodes.GetPrice(), _result.OriginalApprenticeship.Cost);

        }
        public void AssertResultIsApprenticeUpdates()
        {
            var update = _apprenticeshipUpdate.ApprenticeshipUpdates.First();

            Assert.AreEqual(update.FirstName, _result.ApprenticeshipUpdates.FirstName);
            Assert.AreEqual(update.LastName, _result.ApprenticeshipUpdates.LastName);
            Assert.AreEqual(update.Email, _result.ApprenticeshipUpdates.Email);
            Assert.AreEqual(update.DateOfBirth, _result.ApprenticeshipUpdates.DateOfBirth);
            Assert.AreEqual(update.StartDate, _result.ApprenticeshipUpdates.StartDate);
            Assert.AreEqual(update.EndDate, _result.ApprenticeshipUpdates.EndDate);
            Assert.AreEqual(update.TrainingCode, _result.ApprenticeshipUpdates.CourseCode);
            Assert.AreEqual(update.TrainingName, _result.ApprenticeshipUpdates.CourseName);
            Assert.AreEqual(update.Version, _result.ApprenticeshipUpdates.Version);
            Assert.AreEqual(update.Option, _result.ApprenticeshipUpdates.Option);
            Assert.AreEqual(update.DeliveryModel, _result.ApprenticeshipUpdates.DeliveryModel);
            Assert.AreEqual(update.EmploymentEndDate, _result.ApprenticeshipUpdates.EmploymentEndDate);
            Assert.AreEqual(update.EmploymentPrice, _result.ApprenticeshipUpdates.EmploymentPrice);
            Assert.AreEqual(update.Cost, _result.ApprenticeshipUpdates.Cost);
        }

        public void AssertResultIsFalse()
        {
            _result.IsValidCourseCode.IsSameOrEqualTo(false);
        }
        public WhenHandlingGetReviewApprenticeshipUpdatesFixture WithCourseCode(String courseCode)
        {
            var apprenticeshipUpdateComposer = _fixture
                .Build<ApprenticeshipUpdate>()
                .With(au => au.TrainingCode, courseCode);

            var apprenticeshipUpdates =
                apprenticeshipUpdateComposer
                    .CreateMany(1)
                    .ToList();

            _apprenticeshipUpdate.ApprenticeshipUpdates = apprenticeshipUpdates;

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(
                        It.Is<GetApprenticeshipUpdatesRequest>(r =>
                            r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipUpdatesResponse>(_apprenticeshipUpdate,
                    HttpStatusCode.OK, string.Empty));

            return this;
        }

        public WhenHandlingGetReviewApprenticeshipUpdatesFixture WithValidApprenticeship()
        {
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(
                    new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));

            return this;
        }

        public WhenHandlingGetReviewApprenticeshipUpdatesFixture WithInvalidValidApprenticeship()
        {
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(null, HttpStatusCode.NotFound,
                    "Test - not found"));


            return this;
        }

        public WhenHandlingGetReviewApprenticeshipUpdatesFixture WithEmptyApprenticeship()
        {
            _serviceParameters = new ServiceParameters(Party.Provider, _apprenticeship.ProviderId + 1);

            _handler = new GetReviewApprenticeshipUpdatesQueryHandler(_apiClient.Object,
                _providerStandardsService.Object, _serviceParameters);

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(
                    new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));


            return this;
        }

        public WhenHandlingGetReviewApprenticeshipUpdatesFixture WithNoApprenticeshipUpdate()
        {
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(
                        It.Is<GetApprenticeshipUpdatesRequest>(r =>
                            r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipUpdatesResponse>(_apprenticeshipUpdate, HttpStatusCode.NotFound,
                    "Test - not found"));


            return this;
        }
        public WhenHandlingGetReviewApprenticeshipUpdatesFixture WithNoPriceEpisodesResponse()
        {
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetPriceEpisodesResponse>(
                        It.Is<GetPriceEpisodesRequest>(r =>
                            r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetPriceEpisodesResponse>(_priceEpisodesResponse, HttpStatusCode.NotFound,
                    "Test - not found"));
            return this;
        }
    }
}