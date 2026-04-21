using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Roatp;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.UnitTests.Services.ProviderCoursesServiceTest
{
    [TestFixture]
    public class WhenGettingCoursesForAProvider
    {
        private ProviderCoursesServiceTestFixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new ProviderCoursesServiceTestFixture();
        }

        [Test]
        public async Task When_Calling_Party_Is_Employer_Then_All_Courses_Are_Returned()
        {
            _fixture.WithEmployerAsCallingParty();
            await _fixture.GetCoursesData();
            _fixture.AssertResultIsAllCourses();
        }

        [Test]
        public async Task When_Provider_Is_Main_Provider_Then_Courses_Defined_In_Managing_Courses_Are_Returned()
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
        public async Task When_Cached_Then_All_Courses_Are_Returned_From_Cache()
        {
            _fixture.WithEmployerAsCallingParty()
                .WithAllCoursesCached();
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

        public class ProviderCoursesServiceTestFixture
        {
            private Approvals.Services.ProviderCoursesService _providerCoursesService;
            private ServiceParameters _serviceParameters;
            private Mock<ITrainingProviderService> _trainingProviderService;
            private Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> _managingStandardsApiClient;
            private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
            private Mock<ICacheStorageService> _cacheStorageService;
            private Fixture _autoFixture = new();
            private long _trainingProviderId;
            private ProviderDetailsModel _trainingProviderResponse;
            private ProviderDetailsModel _trainingProviderCacheResponse;
            private ProviderStandardsData _result;
            private GetCoursesListResponse _allCoursesResponse;
            private GetCoursesForProviderResponse _getProviderCoursesResponse;
            private GetCoursesListResponse _allCoursesCacheResponse;

            public ProviderCoursesServiceTestFixture()
            {
                _trainingProviderService = new Mock<ITrainingProviderService>();
                _managingStandardsApiClient = new Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>>();
                _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
                _cacheStorageService = new Mock<ICacheStorageService>();
                _serviceParameters = new ServiceParameters(Party.Provider, 123);

                _trainingProviderId = _autoFixture.Create<long>();
                _trainingProviderResponse = _autoFixture.Create<ProviderDetailsModel>();
                _trainingProviderResponse.ProviderType = ProviderType.Main;

                _trainingProviderService.Setup(x =>
                        x.GetProviderDetails(It.Is<int>(id => id == _trainingProviderId)))
                    .ReturnsAsync(_trainingProviderResponse);

                _allCoursesResponse = _autoFixture.Create<GetCoursesListResponse>();
                _coursesApiClient.Setup(x => x.Get<GetCoursesListResponse>(It.IsAny<GetCoursesExportRequest>()))
                    .ReturnsAsync(_allCoursesResponse);

                _getProviderCoursesResponse = _autoFixture.Create<GetCoursesForProviderResponse>();
                _managingStandardsApiClient.Setup(x =>
                        x.Get<GetCoursesForProviderResponse>(It.IsAny<GetCoursesForProviderRequest>()))
                    .ReturnsAsync(_getProviderCoursesResponse);

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<GetCoursesListResponse>(ProviderCoursesService.AllCoursesCacheKey))
                    .ReturnsAsync(() => null);

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<ProviderDetailsModel>(
                            $"{ProviderCoursesService.ProviderDetailsCacheKey}-{_trainingProviderId}"))
                    .ReturnsAsync(() => null);

                _providerCoursesService = new Approvals.Services.ProviderCoursesService(_serviceParameters, _trainingProviderService.Object,
                    _managingStandardsApiClient.Object,
                    _coursesApiClient.Object,
                _cacheStorageService.Object,
                    Mock.Of<ILogger<ProviderStandardsService>>());
            }

            public ProviderCoursesServiceTestFixture WithProviderType(ProviderType providerType)
            {
                _trainingProviderResponse.ProviderType = providerType;
                return this;
            }

            public ProviderCoursesServiceTestFixture WithEmployerAsCallingParty()
            {
                _serviceParameters = new ServiceParameters(Party.Employer, 456);

                _providerCoursesService = new ProviderCoursesService(_serviceParameters, _trainingProviderService.Object,
                    _managingStandardsApiClient.Object,
                    _coursesApiClient.Object,
                    _cacheStorageService.Object,
                    Mock.Of<ILogger<ProviderStandardsService>>());

                return this;
            }

            public ProviderCoursesServiceTestFixture WithAllCoursesCached()
            {
                _allCoursesCacheResponse = _autoFixture.Create<GetCoursesListResponse>();
                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<GetCoursesListResponse>(ProviderCoursesService.AllCoursesCacheKey))
                    .ReturnsAsync(_allCoursesCacheResponse);

                _coursesApiClient.Setup(x => x.Get<GetCoursesListResponse>(It.IsAny<GetCoursesExportRequest>()))
                    .ThrowsAsync(new InvalidOperationException("GetAllCoursesRequest Api Request is invalid when response is cached"));

                return this;
            }

            public ProviderCoursesServiceTestFixture WithMainProviderDetailsInCache()
            {
                _trainingProviderCacheResponse = _autoFixture.Build<ProviderDetailsModel>()
                    .With(x => x.ProviderType, ProviderType.Main)
                    .Create();

                _trainingProviderService.Setup(x =>
                        x.GetProviderDetails(It.Is<int>(id => id == _trainingProviderId)))
                    .ThrowsAsync(new InvalidOperationException("GetTrainingProviderDetails API call is invalid when response is cached"));

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<ProviderDetailsModel>(
                            $"{ProviderCoursesService.ProviderDetailsCacheKey}-{_trainingProviderId}"))
                    .ReturnsAsync(_trainingProviderCacheResponse);

                return this;
            }

            public async Task GetCoursesData()
            {
                _result = await _providerCoursesService.GetCoursesData(_trainingProviderId);
            }

            public void AssertResultIsAllCourses()
            {
                var expected = _allCoursesResponse.Courses.ToDictionary(x => x.LarsCode, y => y.Title);
                expected.Should().BeEquivalentTo(_result.Standards.ToDictionary(x => x.CourseCode, y => y.Name));
            }

            public void AssertResultIsAllCoursesFromCache()
            {
                var expected = _allCoursesCacheResponse.Courses.ToDictionary(x => x.LarsCode, y => y.Title);
                expected.Should().BeEquivalentTo(_result.Standards.ToDictionary(x => x.CourseCode, y => y.Name));
            }

            public void AssertResultIsAsDefinedInManagingCourses()
            {
                var expected = _getProviderCoursesResponse.CourseTypes.SelectMany(ct => ct.Courses).Select(x => x.LarsCode);
                expected.Should().BeEquivalentTo(_result.Standards.Select(x => x.CourseCode));
            }

            public void AssertResultIndicatesIsMainProvider(bool expectIsMainProvider)
            {
                Assert.That(_result.IsMainProvider, Is.EqualTo(expectIsMainProvider));
            }
        }
    }
}
