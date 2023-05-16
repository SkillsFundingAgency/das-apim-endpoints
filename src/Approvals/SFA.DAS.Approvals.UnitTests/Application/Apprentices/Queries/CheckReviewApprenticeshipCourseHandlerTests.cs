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
using SFA.DAS.Approvals.Application.Apprentices.Queries.CheckReviewApprenticeshipCourse;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    [TestFixture]
    public class WhenHandlingCheckReviewApprenticeshipCourse
    {
        private WhenHandlingCheckReviewApprenticeshipCourseFixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new WhenHandlingCheckReviewApprenticeshipCourseFixture();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_With_Valid_CourseCode_Returned_True()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithCourseCode("123");
            await _fixture.CheckReviewApprenticeshipCourse();
            _fixture.AssertResultIsTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_With_InvalidValid_CourseCode_Returned_False()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithCourseCode("1234");
            await _fixture.CheckReviewApprenticeshipCourse();
            _fixture.AssertResultIsFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_When_Apprenticeship_Is_Not_Found_Then_Result_Is_Null()
        {
            _fixture.WithInvalidValidApprenticeship();
            await _fixture.CheckReviewApprenticeshipCourse();
            _fixture.AssertResultIsNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_When_CallingParty_Does_Not_Own_Apprenticeship_Then_Result_Is_Null()
        {
            _fixture.WithEmptyApprenticeship();
            await _fixture.CheckReviewApprenticeshipCourse();
            _fixture.AssertResultIsNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_When_ApprenticeshipUpdate_Is_Not_Found_Then_Result_Is_Null()
        {
            _fixture.WithValidApprenticeship();
            _fixture.WithNoApprenticeshipUpdate();
            await _fixture.CheckReviewApprenticeshipCourse();
            _fixture.AssertResultIsNull();
        }
    }

    public class WhenHandlingCheckReviewApprenticeshipCourseFixture
    {
        private readonly Mock<CheckReviewApprenticeshipCourseQuery> _query;
        private readonly GetApprenticeshipResponse _apprenticeship;
        private readonly GetApprenticeshipUpdatesResponse _apprenticeshipUpdate;
        private readonly ProviderStandardsData _providerStandardsData;
        private readonly Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private readonly Mock<IProviderStandardsService> _providerStandardsService;
        private CheckReviewApprenticeshipCourseQueryHandler _handler;
        private readonly Fixture _fixture;
        private CheckReviewApprenticeshipCourseQueryResult _result;
        private ServiceParameters _serviceParameters;

        public WhenHandlingCheckReviewApprenticeshipCourseFixture()
        {
            _fixture = new Fixture();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _providerStandardsService = new Mock<IProviderStandardsService>();
            _query = new Mock<CheckReviewApprenticeshipCourseQuery>();

            _providerStandardsData = _fixture.Build<ProviderStandardsData>()
                .Create();

            _apprenticeship = _fixture.Build<GetApprenticeshipResponse>()
                .Create();


            _apprenticeshipUpdate = _fixture.Build<GetApprenticeshipUpdatesResponse>()
                .Create();

            _providerStandardsData.Standards = new List<Standard> { new Standard("123", "123") };

            _serviceParameters = new ServiceParameters(Party.Provider, _apprenticeship.ProviderId);

            _handler = new CheckReviewApprenticeshipCourseQueryHandler(_apiClient.Object,
                _providerStandardsService.Object, _serviceParameters);

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(
                        It.Is<GetApprenticeshipUpdatesRequest>(r =>
                            r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipUpdatesResponse>(_apprenticeshipUpdate,
                    HttpStatusCode.OK, string.Empty));

            _providerStandardsService.Setup(x => x.GetStandardsData(_apprenticeship.ProviderId))
                .ReturnsAsync(_providerStandardsData);

        }

        public async Task CheckReviewApprenticeshipCourse()
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

        public void AssertResultIsFalse()
        {
            _result.IsValidCourseCode.IsSameOrEqualTo(false);
        }
        public WhenHandlingCheckReviewApprenticeshipCourseFixture WithCourseCode(String courseCode)
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

        public WhenHandlingCheckReviewApprenticeshipCourseFixture WithValidApprenticeship()
        {
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(
                    new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));

            return this;
        }

        public WhenHandlingCheckReviewApprenticeshipCourseFixture WithInvalidValidApprenticeship()
        {
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(null, HttpStatusCode.NotFound,
                    "Test - not found"));


            return this;
        }

        public WhenHandlingCheckReviewApprenticeshipCourseFixture WithEmptyApprenticeship()
        {
            _serviceParameters = new ServiceParameters(Party.Provider, _apprenticeship.ProviderId + 1);

            _handler = new CheckReviewApprenticeshipCourseQueryHandler(_apiClient.Object,
                _providerStandardsService.Object, _serviceParameters);

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(
                    new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));


            return this;
        }

        public WhenHandlingCheckReviewApprenticeshipCourseFixture WithNoApprenticeshipUpdate()
        {
            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(
                        It.Is<GetApprenticeshipUpdatesRequest>(r =>
                            r.ApprenticeshipId == _query.Object.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipUpdatesResponse>(_apprenticeshipUpdate, HttpStatusCode.NotFound,
                    "Test - not found"));


            return this;
        }
    }
}