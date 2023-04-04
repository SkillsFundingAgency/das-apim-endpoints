using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Responses;
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
            await _fixture.GetCourses();
            _fixture.AssertResultIsAllStandards();
        }

        [Test]
        public async Task When_Provider_Is_Main_Provider_Then_Courses_Defined_In_Managing_Standards_Are_Returned()
        {
            await _fixture.GetCourses();
            _fixture.AssertResultIsAsDefinedInManagingStandards();
        }

        [TestCase(TrainingProviderResponse.ProviderTypeIdentifier.EmployerProvider)]
        [TestCase(TrainingProviderResponse.ProviderTypeIdentifier.SupportingProvider)]
        public async Task When_Provider_Is_Not_Main_Provider_Then_All_Standards_Returned(TrainingProviderResponse.ProviderTypeIdentifier providerType)
        {
            _fixture.WithProviderType(providerType);
            await _fixture.GetCourses();
            _fixture.AssertResultIsAllStandards();
        }

        public class ProviderCoursesServiceTestFixture
        {
            private Approvals.Services.ProviderCoursesService _providerCoursesService;
            private ServiceParameters _serviceParameters;
            private Mock<ITrainingProviderService> _trainingProviderService;
            private Mock<IInternalApiClient<RoatpV2ApiConfiguration>> _managingStandardsApiClient;
            private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsV2ApiClient;
            private Fixture _autoFixture = new Fixture();
            private long _trainingProviderId;
            private TrainingProviderResponse _trainingProviderResponse;
            private Dictionary<string, string> _result;
            private GetAllStandardsResponse _allStandardsResponse;
            private IEnumerable<GetProviderStandardsResponse> _getProviderStandardsResponse;

            public ProviderCoursesServiceTestFixture()
            {
                _trainingProviderService = new Mock<ITrainingProviderService>();
                _managingStandardsApiClient = new Mock<IInternalApiClient<RoatpV2ApiConfiguration>>();
                _commitmentsV2ApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
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

                _providerCoursesService = new Approvals.Services.ProviderCoursesService(_serviceParameters, _trainingProviderService.Object,
                    _managingStandardsApiClient.Object,
                    _commitmentsV2ApiClient.Object);
            }

            public ProviderCoursesServiceTestFixture WithProviderType(TrainingProviderResponse.ProviderTypeIdentifier providerType)
            {
                _trainingProviderResponse.ProviderType.Id = (short) providerType;
                return this;
            }

            public ProviderCoursesServiceTestFixture WithEmployerAsCallingParty()
            {
                _serviceParameters = new ServiceParameters(Party.Employer, 456);

                _providerCoursesService = new Approvals.Services.ProviderCoursesService(_serviceParameters, _trainingProviderService.Object,
                    _managingStandardsApiClient.Object,
                    _commitmentsV2ApiClient.Object);

                return this;
            }

            public async Task GetCourses()
            {
                _result = await _providerCoursesService.GetCourses(_trainingProviderId);
            }

            public void AssertResultIsAllStandards()
            {
                var expected = _allStandardsResponse.TrainingProgrammes.ToDictionary(x => x.CourseCode, y => y.Name);
                CollectionAssert.AreEqual(expected, _result);
            }

            public void AssertResultIsAsDefinedInManagingStandards()
            {
                var expected = _getProviderStandardsResponse.ToDictionary(x => x.LarsCode.ToString(), y => y.CourseNameWithLevel);
                CollectionAssert.AreEqual(expected, _result);
            }
        }
    }
}
