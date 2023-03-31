using System.Collections.Generic;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Requests;
using SFA.DAS.Approvals.InnerApi.ManagingStandards.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
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
        public void When_Calling_Party_Is_Employer_Then_All_Standards_Are_Returned()
        {
            _fixture.WithEmployerAsCallingParty();
            _fixture.AssertResultIsAllStandards();
        }

        [Test]
        public void When_Provider_Is_Main_Provider_Then_Courses_Defined_In_Managing_Standards_Are_Returned()
        {
            _fixture.AssertResultIsAsDefinedInManagingStandards();
        }

        [TestCase(ProviderType.EmployerProvider)]
        [TestCase(ProviderType.SupportingProvider)]
        public void When_Provider_Is_Not_Main_Provider_Then_All_Standards_Returned(ProviderType providerType)
        {
            _fixture.WithProviderType(providerType);
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
            private IEnumerable<GetProviderStandardsResponse> _getProviderStandardsResponses;

            public ProviderCoursesServiceTestFixture()
            {
                _trainingProviderService = new Mock<ITrainingProviderService>();
                _managingStandardsApiClient = new Mock<IInternalApiClient<RoatpV2ApiConfiguration>>();
                _commitmentsV2ApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
                _serviceParameters = new ServiceParameters(Party.Provider, 123);

                _trainingProviderId = _autoFixture.Create<long>();
                _trainingProviderResponse = _autoFixture.Create<TrainingProviderResponse>();
                _trainingProviderResponse.ProviderType.Id = (short) ProviderType.MainProvider;

                _trainingProviderService.Setup(x =>
                        x.GetTrainingProviderDetails(It.Is<long>(id => id == _trainingProviderId)))
                    .ReturnsAsync(_trainingProviderResponse);

                _allStandardsResponse = _autoFixture.Create<GetAllStandardsResponse>();
                _commitmentsV2ApiClient.Setup(x => x.Get<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>()))
                    .ReturnsAsync(_allStandardsResponse);

                _getProviderStandardsResponses = _autoFixture.Create<IEnumerable<GetProviderStandardsResponse>>();
                _managingStandardsApiClient.Setup(x =>
                        x.Get<IEnumerable<GetProviderStandardsResponse>>(It.IsAny<GetProviderStandardsRequest>()))
                    .ReturnsAsync(_getProviderStandardsResponses);

                _providerCoursesService = new Approvals.Services.ProviderCoursesService(_serviceParameters, _trainingProviderService.Object,
                    _managingStandardsApiClient.Object,
                    _commitmentsV2ApiClient.Object);
            }

            public ProviderCoursesServiceTestFixture WithProviderType(ProviderType providerType)
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

            public void GetCourses()
            {
                _result = _providerCoursesService.GetCourses(_trainingProviderId);
            }

            public void AssertResultIsAllStandards()
            {
                Assert.Fail("Not implemented");
            }

            public void AssertResultIsAsDefinedInManagingStandards()
            {
                Assert.Fail("Not implemented");
            }
        }

        public enum ProviderType
        {
            MainProvider,
            EmployerProvider,
            SupportingProvider
        }

    }
}
