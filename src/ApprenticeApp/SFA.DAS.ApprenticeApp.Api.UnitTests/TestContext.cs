using SFA.DAS.ApprenticeApp.MockApis;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeApp.Api.UnitTests
{
    public class TestContext
    {
        private readonly FeatureContext _feature;
        public TestContext(FeatureContext feature) => _feature = feature;
        public ApprenticeAppOuterApi OuterApiClient { get; set; }
        public ApprenticeCommitmentsInnerApiMock ApprenticeCommitmentsInnerApi => _feature.GetOrAdd<ApprenticeCommitmentsInnerApiMock>();
        public ApprenticeAccountsInnerApiMock ApprenticeAccountsInnerApi => _feature.GetOrAdd<ApprenticeAccountsInnerApiMock>();
        public TrainingProviderInnerApiMock TrainingProviderInnerApi => _feature.GetOrAdd<TrainingProviderInnerApiMock>();
        public CommitmentsV2InnerApiMock CommitmentsV2InnerApi => _feature.GetOrAdd<CommitmentsV2InnerApiMock>(); 
        public CoursesInnerApiMock CoursesInnerApi => _feature.GetOrAdd<CoursesInnerApiMock>();
    }
}