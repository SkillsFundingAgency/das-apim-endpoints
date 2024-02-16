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
using FluentAssertions;
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
            _result.IsValidCourseCode.Should().BeTrue();
        }

        public void AssertResultIsNull()
        {
            Assert.That(_result, Is.Null);
        }

        public void AssertResultIsOriginalApprentice()
        {
            Assert.That(_apprenticeship.EmployerName, Is.EqualTo(_result.EmployerName));
            Assert.That(_apprenticeship.ProviderName, Is.EqualTo(_result.ProviderName));

            Assert.That(_apprenticeship.FirstName, Is.EqualTo(_result.OriginalApprenticeship.FirstName));
            Assert.That(_apprenticeship.LastName, Is.EqualTo(_result.OriginalApprenticeship.LastName));
            Assert.That(_apprenticeship.Email, Is.EqualTo(_result.OriginalApprenticeship.Email));
            Assert.That(_apprenticeship.Uln,Is.EqualTo( _result.OriginalApprenticeship.Uln));
            Assert.That(_apprenticeship.DateOfBirth, Is.EqualTo(_result.OriginalApprenticeship.DateOfBirth));
            Assert.That(_apprenticeship.StartDate,Is.EqualTo( _result.OriginalApprenticeship.StartDate));
            Assert.That(_apprenticeship.EndDate, Is.EqualTo(_result.OriginalApprenticeship.EndDate));
            Assert.That(_apprenticeship.CourseCode, Is.EqualTo(_result.OriginalApprenticeship.CourseCode));
            Assert.That(_apprenticeship.CourseName, Is.EqualTo(_result.OriginalApprenticeship.CourseName));
            Assert.That(_apprenticeship.Version, Is.EqualTo(_result.OriginalApprenticeship.Version));
            Assert.That(_apprenticeship.Option,Is.EqualTo( _result.OriginalApprenticeship.Option));
            Assert.That((DeliveryModel)Enum.Parse(typeof(DeliveryModel), _apprenticeship.DeliveryModel), Is.EqualTo(_result.OriginalApprenticeship.DeliveryModel));
            Assert.That(_apprenticeship.EmploymentEndDate, Is.EqualTo(_result.OriginalApprenticeship.EmploymentEndDate));
            Assert.That(_apprenticeship.EmploymentPrice, Is.EqualTo(_result.OriginalApprenticeship.EmploymentPrice));
            Assert.That(_apprenticeship.Uln, Is.EqualTo(_result.OriginalApprenticeship.Uln));
            Assert.That(_priceEpisodesResponse.PriceEpisodes.GetPrice(), Is.EqualTo(_result.OriginalApprenticeship.Cost));

        }
        public void AssertResultIsApprenticeUpdates()
        {
            var update = _apprenticeshipUpdate.ApprenticeshipUpdates.First();

            Assert.That(update.FirstName, Is.EqualTo(_result.ApprenticeshipUpdates.FirstName));
            Assert.That(update.LastName, Is.EqualTo(_result.ApprenticeshipUpdates.LastName));
            Assert.That(update.Email, Is.EqualTo(_result.ApprenticeshipUpdates.Email));
            Assert.That(update.DateOfBirth, Is.EqualTo(_result.ApprenticeshipUpdates.DateOfBirth));
            Assert.That(update.StartDate, Is.EqualTo(_result.ApprenticeshipUpdates.StartDate));
            Assert.That(update.EndDate, Is.EqualTo(_result.ApprenticeshipUpdates.EndDate));
            Assert.That(update.TrainingCode, Is.EqualTo(_result.ApprenticeshipUpdates.CourseCode));
            Assert.That(update.TrainingName, Is.EqualTo(_result.ApprenticeshipUpdates.CourseName));
            Assert.That(update.Version, Is.EqualTo(_result.ApprenticeshipUpdates.Version));
            Assert.That(update.Option, Is.EqualTo(_result.ApprenticeshipUpdates.Option));
            Assert.That(update.DeliveryModel, Is.EqualTo(_result.ApprenticeshipUpdates.DeliveryModel));
            Assert.That(update.EmploymentEndDate, Is.EqualTo(_result.ApprenticeshipUpdates.EmploymentEndDate));
            Assert.That(update.EmploymentPrice, Is.EqualTo(_result.ApprenticeshipUpdates.EmploymentPrice));
            Assert.That(update.Cost, Is.EqualTo(_result.ApprenticeshipUpdates.Cost));
        }

        public void AssertResultIsFalse()
        {
            _result.IsValidCourseCode.Should().BeFalse();
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