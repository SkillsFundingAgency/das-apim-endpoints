using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;
using GetAllStandardsRequest = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses.GetAllStandardsRequest;

namespace SFA.DAS.Approvals.UnitTests.Services.ProviderCoursesService
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
        public async Task When_Calling_Party_Is_Employer_Then_All_Standards_Are_Returned()
        {
            _fixture.WithEmployerAsCallingParty();
            await _fixture.GetStandardsData();
            _fixture.AssertResultIsAllStandards();
        }

        [Test]
        public async Task When_Provider_Is_Main_Provider_Then_Courses_Defined_In_Managing_Standards_Are_Returned()
        {
            await _fixture.GetStandardsData();
            _fixture.AssertResultIsAsDefinedInManagingStandards();
        }

        [TestCase(TrainingProviderResponse.ProviderTypeIdentifier.MainProvider, true)]
        [TestCase(TrainingProviderResponse.ProviderTypeIdentifier.EmployerProvider, false)]
        [TestCase(TrainingProviderResponse.ProviderTypeIdentifier.SupportingProvider, false)]
        public async Task When_Provider_Is_Main_Provider_Then_IsMainProvider_Is_True(TrainingProviderResponse.ProviderTypeIdentifier providerType, bool expectIsMainProvider)
        {
            _fixture.WithProviderType(providerType);
            await _fixture.GetStandardsData();
            _fixture.AssertResultIndicatesIsMainProvider(expectIsMainProvider);
        }

        [TestCase(TrainingProviderResponse.ProviderTypeIdentifier.EmployerProvider)]
        [TestCase(TrainingProviderResponse.ProviderTypeIdentifier.SupportingProvider)]
        public async Task When_Provider_Is_Not_Main_Provider_Then_All_Standards_Returned(TrainingProviderResponse.ProviderTypeIdentifier providerType)
        {
            _fixture.WithProviderType(providerType);
            await _fixture.GetStandardsData();
            _fixture.AssertResultIsAllStandards();
        }

        [Test]
        public async Task When_Cached_Then_All_Standards_Are_Returned_From_Cache()
        {
            _fixture.WithEmployerAsCallingParty()
                .WithAllStandardsInCache();
            await _fixture.GetStandardsData();
            _fixture.AssertResultIsAllStandardsFromCache();
        }

        [Test]
        public async Task When_Cached_Provider_Is_Main_Provider_Then_Courses_Defined_In_Managing_Standards_Are_Returned()
        {
            _fixture.WithMainProviderDetailsInCache();
            await _fixture.GetStandardsData();
            _fixture.AssertResultIsAsDefinedInManagingStandards();
        }

        public class ProviderCoursesServiceTestFixture
        {
            private Approvals.Services.ProviderStandardsService _providerStandardsService;
            private ServiceParameters _serviceParameters;
            private Mock<ITrainingProviderService> _trainingProviderService;
            private Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> _managingStandardsApiClient;
            private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsV2ApiClient;
            private Mock<ICacheStorageService> _cacheStorageService;
            private Fixture _autoFixture = new Fixture();
            private long _trainingProviderId;
            private TrainingProviderResponse _trainingProviderResponse;
            private TrainingProviderResponse _trainingProviderCacheResponse;
            private ProviderStandardsData _result;
            private GetAllStandardsResponse _allStandardsResponse;
            private IEnumerable<GetProviderStandardsResponse> _getProviderStandardsResponse;
            private GetAllStandardsResponse _allStandardsCacheResponse;

            public ProviderCoursesServiceTestFixture()
            {
                _trainingProviderService = new Mock<ITrainingProviderService>();
                _managingStandardsApiClient = new Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>>();
                _commitmentsV2ApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
                _cacheStorageService = new Mock<ICacheStorageService>();
                _serviceParameters = new ServiceParameters(Party.Provider, 123);

                _trainingProviderId = _autoFixture.Create<long>();
                _trainingProviderResponse = _autoFixture.Create<TrainingProviderResponse>();
                _trainingProviderResponse.ProviderType.Id = (short)TrainingProviderResponse.ProviderTypeIdentifier.MainProvider;

                _trainingProviderService.Setup(x =>
                        x.GetTrainingProviderDetails(It.Is<long>(id => id == _trainingProviderId)))
                    .ReturnsAsync(_trainingProviderResponse);

                _allStandardsResponse = _autoFixture.Create<GetAllStandardsResponse>();
                _commitmentsV2ApiClient.Setup(x => x.Get<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
                    .ReturnsAsync(_allStandardsResponse);

                _getProviderStandardsResponse = _autoFixture.Create<IEnumerable<GetProviderStandardsResponse>>();
                _managingStandardsApiClient.Setup(x =>
                        x.Get<IEnumerable<GetProviderStandardsResponse>>(It.IsAny<GetProviderStandardsRequest>()))
                    .ReturnsAsync(_getProviderStandardsResponse);

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<GetAllStandardsResponse>(Approvals.Services.ProviderStandardsService.AllStandardsCacheKey))
                    .ReturnsAsync(() => null);

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<TrainingProviderResponse>(
                            $"{Approvals.Services.ProviderStandardsService.ProviderDetailsCacheKey}-{_trainingProviderId}"))
                    .ReturnsAsync(() => null);

                _providerStandardsService = new Approvals.Services.ProviderStandardsService(_serviceParameters, _trainingProviderService.Object,
                    _managingStandardsApiClient.Object,
                    _commitmentsV2ApiClient.Object,
                _cacheStorageService.Object,
                    Mock.Of<ILogger<ProviderStandardsService>>());
            }

            public ProviderCoursesServiceTestFixture WithProviderType(TrainingProviderResponse.ProviderTypeIdentifier providerType)
            {
                _trainingProviderResponse.ProviderType.Id = (short)providerType;
                return this;
            }

            public ProviderCoursesServiceTestFixture WithEmployerAsCallingParty()
            {
                _serviceParameters = new ServiceParameters(Party.Employer, 456);

                _providerStandardsService = new Approvals.Services.ProviderStandardsService(_serviceParameters, _trainingProviderService.Object,
                    _managingStandardsApiClient.Object,
                    _commitmentsV2ApiClient.Object,
                    _cacheStorageService.Object,
                    Mock.Of<ILogger<ProviderStandardsService>>());

                return this;
            }

            public ProviderCoursesServiceTestFixture WithAllStandardsInCache()
            {
                _allStandardsCacheResponse = _autoFixture.Create<GetAllStandardsResponse>();
                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<GetAllStandardsResponse>(Approvals.Services.ProviderStandardsService.AllStandardsCacheKey))
                    .ReturnsAsync(_allStandardsCacheResponse);

                _commitmentsV2ApiClient.Setup(x => x.Get<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
                    .ThrowsAsync(new InvalidOperationException("GetAllStandardsRequest Api Request is invalid when response is cached"));

                return this;
            }

            public ProviderCoursesServiceTestFixture WithMainProviderDetailsInCache()
            {
                _trainingProviderCacheResponse = _autoFixture.Build<TrainingProviderResponse>()
                    .With(x => x.ProviderType, new TrainingProviderResponse.ProviderTypeResponse { Id = (short)(short)TrainingProviderResponse.ProviderTypeIdentifier.MainProvider })
                    .Create();

                _trainingProviderService.Setup(x =>
                        x.GetTrainingProviderDetails(It.Is<long>(id => id == _trainingProviderId)))
                    .ThrowsAsync(new InvalidOperationException("GetTrainingProviderDetails API call is invalid when response is cached"));

                _cacheStorageService.Setup(x =>
                        x.RetrieveFromCache<TrainingProviderResponse>(
                            $"{Approvals.Services.ProviderStandardsService.ProviderDetailsCacheKey}-{_trainingProviderId}"))
                    .ReturnsAsync(_trainingProviderCacheResponse);

                return this;
            }

            public async Task GetStandardsData()
            {
                _result = await _providerStandardsService.GetStandardsData(_trainingProviderId);
            }

            public void AssertResultIsAllStandards()
            {
                var expected = _allStandardsResponse.TrainingProgrammes.ToDictionary(x => x.CourseCode, y => y.Name);
                CollectionAssert.AreEqual(expected, _result.Standards.ToDictionary(x => x.CourseCode, y => y.Name));
            }

            public void AssertResultIsAllStandardsFromCache()
            {
                var expected = _allStandardsCacheResponse.TrainingProgrammes.ToDictionary(x => x.CourseCode, y => y.Name);
                CollectionAssert.AreEqual(expected, _result.Standards.ToDictionary(x => x.CourseCode, y => y.Name));
            }

            public void AssertResultIsAsDefinedInManagingStandards()
            {
                var expected = _getProviderStandardsResponse.ToDictionary(x => x.LarsCode.ToString(), y => y.CourseNameWithLevel);
                CollectionAssert.AreEqual(expected, _result.Standards.ToDictionary(x => x.CourseCode, y => y.Name));
            }

            public void AssertResultIndicatesIsMainProvider(bool expectIsMainProvider)
            {
                Assert.That(_result.IsMainProvider, Is.EqualTo(expectIsMainProvider));
            }
        }
    }
}
