using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Roatp;
using GetAllStandardsRequest = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses.GetAllStandardsRequest;

namespace SFA.DAS.Approvals.UnitTests.Services.ProviderCoursesService
{
    [TestFixture]
    public class WhenGettingCoursesWithAppUnitsForAProvider
    {
        private ProviderCoursesWithAppUnitsServiceTestFixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new ProviderCoursesWithAppUnitsServiceTestFixture();
        }

        [Test]
        public async Task When_Calling_Party_Is_Employer_Then_All_Courses_Are_Returned()
        {
            _fixture.WithEmployerAsCallingParty();
            await _fixture.GetCoursesData();
            _fixture.AssertResultIsAllCourses();
        }

        [Test]
        public async Task When_Provider_Is_Main_Provider_Then_Courses_Defined_In_Managing_Standards_Are_Returned()
        {
            await _fixture.GetCoursesData();
            _fixture.AssertResultIsAsDefinedInManagingCourses();
        }

        [TestCase(ProviderType.Main, true)]
        [TestCase(ProviderType.Employer, false)]
        [TestCase(ProviderType.Supporting, false)]
        public async Task When_Provider_Is_Main_Provider_Then_IsMainProvider_Is_True(ProviderType providerType, bool expectIsMainProvider)
        {
            _fixture.WithProviderType(providerType);
            await _fixture.GetCoursesData();
            _fixture.AssertResultIndicatesIsMainProvider(expectIsMainProvider);
        }

        [TestCase(ProviderType.Employer)]
        [TestCase(ProviderType.Supporting)]
        public async Task When_Provider_Is_Not_Main_Provider_Then_All_Courses_Returned(ProviderType providerType)
        {
            _fixture.WithProviderType(providerType);
            await _fixture.GetCoursesData();
            _fixture.AssertResultIsAllCourses();
        }

        [Test]
        public async Task When_Cached_Then_All_Standards_Are_Returned_From_Cache()
        {
            _fixture.WithEmployerAsCallingParty()
                .WithAllStandardsInCache();
            await _fixture.GetCoursesData();
            _fixture.AssertResultIsAllCoursesFromCache();
        }

        [Test]
        public async Task When_Cached_Provider_Is_Main_Provider_Then_Courses_Defined_In_Managing_Standards_Are_Returned()
        {
            _fixture.WithMainProviderDetailsInCache();
            await _fixture.GetCoursesData();
            _fixture.AssertResultIsAsDefinedInManagingCourses();
        }

        public class ProviderCoursesWithAppUnitsServiceTestFixture
        {
            private Approvals.Services.ProviderStandardsService _providerStandardsService;
            private ServiceParameters _serviceParameters;
            private Mock<ITrainingProviderService> _trainingProviderService;
            private Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> _managingCoursesApiClient;
            private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsV2ApiClient;
            private Mock<ICacheStorageService> _cacheStorageService;
            private Fixture _autoFixture = new();
            private long _trainingProviderId;
            private ProviderDetailsModel _trainingProviderResponse;
            private ProviderDetailsModel _trainingProviderCacheResponse;
            private ProviderStandardsData _result;
            private GetAllStandardsResponse _allStandardsResponse;
            private GetCoursesForProviderResponse _getProviderCoursesResponse;
            private GetAllStandardsResponse _allStandardsCacheResponse;

            public ProviderCoursesWithAppUnitsServiceTestFixture()
            {
                _trainingProviderService = new Mock<ITrainingProviderService>();
                _managingCoursesApiClient = new Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>>();
                _commitmentsV2ApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
                _cacheStorageService = new Mock<ICacheStorageService>();
                _serviceParameters = new ServiceParameters(Party.Provider, 123);

                _trainingProviderId = _autoFixture.Create<long>();
                _trainingProviderResponse = _autoFixture.Create<ProviderDetailsModel>();
                _trainingProviderResponse.ProviderType = ProviderType.Main;

                _trainingProviderService.Setup(x =>
                        x.GetProviderDetails(It.Is<int>(id => id == _trainingProviderId)))
                    .ReturnsAsync(_trainingProviderResponse);

                _allStandardsResponse = _autoFixture.Create<GetAllStandardsResponse>();
                _commitmentsV2ApiClient.Setup(x => x.Get<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
                    .ReturnsAsync(_allStandardsResponse);

                _getProviderCoursesResponse = _autoFixture.Create<GetCoursesForProviderResponse>();
                _managingCoursesApiClient.Setup(x =>
                        x.Get<GetCoursesForProviderResponse>(It.IsAny<GetCoursesForProviderRequest>()))
                    .ReturnsAsync(_getProviderCoursesResponse);

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<GetAllStandardsResponse>(ProviderStandardsService.AllStandardsCacheKey))
                    .ReturnsAsync(() => null);

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<ProviderDetailsModel>(
                            $"{ProviderStandardsService.ProviderDetailsCacheKey}-{_trainingProviderId}"))
                    .ReturnsAsync(() => null);

                _providerStandardsService = new ProviderStandardsService(_serviceParameters, _trainingProviderService.Object,
                    _managingCoursesApiClient.Object,
                    _commitmentsV2ApiClient.Object,
                _cacheStorageService.Object,
                    Mock.Of<ILogger<ProviderStandardsService>>());
            }

            public ProviderCoursesWithAppUnitsServiceTestFixture WithProviderType(ProviderType providerType)
            {
                _trainingProviderResponse.ProviderType = providerType;
                return this;
            }

            public ProviderCoursesWithAppUnitsServiceTestFixture WithEmployerAsCallingParty()
            {
                _serviceParameters = new ServiceParameters(Party.Employer, 456);

                _providerStandardsService = new Approvals.Services.ProviderStandardsService(_serviceParameters, _trainingProviderService.Object,
                    _managingCoursesApiClient.Object,
                    _commitmentsV2ApiClient.Object,
                    _cacheStorageService.Object,
                    Mock.Of<ILogger<ProviderStandardsService>>());

                return this;
            }

            public ProviderCoursesWithAppUnitsServiceTestFixture WithAllStandardsInCache()
            {
                _allStandardsCacheResponse = _autoFixture.Create<GetAllStandardsResponse>();
                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<GetAllStandardsResponse>(Approvals.Services.ProviderStandardsService.AllStandardsCacheKey))
                    .ReturnsAsync(_allStandardsCacheResponse);

                _commitmentsV2ApiClient.Setup(x => x.Get<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
                    .ThrowsAsync(new InvalidOperationException("GetAllStandardsRequest Api Request is invalid when response is cached"));

                return this;
            }

            public ProviderCoursesWithAppUnitsServiceTestFixture WithMainProviderDetailsInCache()
            {
                _trainingProviderCacheResponse = _autoFixture.Build<ProviderDetailsModel>()
                    .With(x => x.ProviderType, ProviderType.Main)
                    .Create();

                _trainingProviderService.Setup(x =>
                        x.GetProviderDetails(It.Is<int>(id => id == _trainingProviderId)))
                    .ThrowsAsync(new InvalidOperationException("GetTrainingProviderDetails API call is invalid when response is cached"));

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<ProviderDetailsModel>(
                            $"{Approvals.Services.ProviderStandardsService.ProviderDetailsCacheKey}-{_trainingProviderId}"))
                    .ReturnsAsync(_trainingProviderCacheResponse);

                return this;
            }

            public async Task GetCoursesData()
            {
                _result = await _providerStandardsService.GetCoursesData(_trainingProviderId);
            }

            public void AssertResultIsAllCourses()
            {
                var expected = _allStandardsResponse.TrainingProgrammes.ToDictionary(x => x.CourseCode, y => y.Name);
                expected.Should().BeEquivalentTo(_result.Standards.ToDictionary(x => x.CourseCode, y => y.Name));
            }

            public void AssertResultIsAllCoursesFromCache()
            {
                var expected = _allStandardsCacheResponse.TrainingProgrammes.Select(x => x.CourseCode);
                expected.Should().BeEquivalentTo(_result.Standards.Select(x => x.CourseCode));
            }

            public void AssertResultIsAsDefinedInManagingCourses()
            {
                var expected = _getProviderCoursesResponse.CourseTypes.SelectMany(ct => ct.Courses).Select(x => x.LarsCode.ToString());
                expected.Should().BeEquivalentTo(_result.Standards.Select(x => x.CourseCode));
            }

            public void AssertResultIndicatesIsMainProvider(bool expectIsMainProvider)
            {
                Assert.That(_result.IsMainProvider, Is.EqualTo(expectIsMainProvider));
            }
        }
    }
}
